using ElevatorsSimulator.Common;
using ElevatorsSimulator.Contracts;
using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Logic;
public class MovementCostAnalyzer : IMovementCostAnalyzer
{
    private Elevator? _elevator;
    private LinkedList<Movement> _movements = new LinkedList<Movement>();

    public void ClearMovementData()
    {
        _elevator = null;
        _movements = new LinkedList<Movement>();
    }

    public void LoadData(Elevator elevator, LinkedList<Movement> data)
    {
        _elevator = elevator;
        _movements = data;
    }

    public MovementCostAnalysisResult GetAnalysisResult(ElevatorRequest request)
    {
        MovementCostAnalysisResult result = new MovementCostAnalysisResult();
        if (_elevator == null)
        {
            throw new Exception("Elevator is not setup for analysis.");
        }
        result.ElevatorId = _elevator.Id;

        Direction requestMovementDirection = request.SourceFloor <= request.DestinationFloor ? Direction.Up : Direction.Down;

        for (int i = 0; i < _movements.Count; i++)
        {
            var currentMovement = _movements.ElementAt(i);

            int sourceFloor;
            int destinationFloor;
            if (i == 0)
            {
                sourceFloor = _elevator.ElevatorStatus.CurrentFloor;
            }
            else
            {
                sourceFloor = currentMovement.SourceFloor;
            }

            destinationFloor = currentMovement.DestinationFloor;

            // Check to see if the request movement is in the same direction with current
            if (requestMovementDirection == currentMovement.Direction)
            {
                // Check to see if request movement start floor is in between current movement start floor and current movement end floor
                if ((sourceFloor <= request.SourceFloor && destinationFloor >= request.SourceFloor && currentMovement.Direction == Direction.Up) ||
                 (sourceFloor >= request.SourceFloor && destinationFloor <= request.SourceFloor && currentMovement.Direction == Direction.Down))
                {
                    // Check to see if the request movement is a subpart of the current movement
                    if ((currentMovement.Direction == Direction.Up && request.DestinationFloor <= destinationFloor) ||
                    (currentMovement.Direction == Direction.Down && request.DestinationFloor >= destinationFloor))
                    {
                        if (currentMovement.Load + request.Load <= _elevator.LoadLimit)
                        {
                            result.MovementStartIndex = i;
                            result.MovementEndIndex = i;
                            result.Cost += Math.Abs(request.SourceFloor - currentMovement.SourceFloor);
                            return result;
                        }
                    }
                    else
                    {
                        var movementEndIndex = FindSequenceEndIndex(i + 1, requestMovementDirection, request);
                        if (movementEndIndex > -1)
                        {
                            result.MovementStartIndex = i;
                            result.MovementEndIndex = movementEndIndex;
                            result.Cost += Math.Abs(request.SourceFloor - currentMovement.SourceFloor);

                            return result;
                        }
                    }
                }
            }

            result.Cost += Math.Abs(sourceFloor - destinationFloor);
        }

        result.Cost += Math.Abs(_movements.Last().DestinationFloor - request.SourceFloor);
        result.MovementStartIndex = _movements.Count + 1;
        result.MovementEndIndex = _movements.Count + 1;

        return result;
    }

    private int FindSequenceEndIndex(int index, Direction requestDirection, ElevatorRequest request)
    {
        if (index > _movements.Count - 1)
        {
            return index - 1;
        }
        if (_elevator == null)
        {
            throw new Exception("Elevator is not setup for analysis.");
        }

        Movement currentMovement = _movements.ElementAt(index);

        if (currentMovement.Direction == requestDirection)
        {
            if (request.Load + currentMovement.Load <= _elevator.LoadLimit)
            {
                if ((requestDirection == Direction.Up && currentMovement.DestinationFloor >= request.DestinationFloor) ||
                 (requestDirection == Direction.Down && currentMovement.DestinationFloor <= request.DestinationFloor))
                {
                    return index;
                }
                else
                {
                    return FindSequenceEndIndex(index + 1, requestDirection, request);
                }
            }
        }

        return -1;
    }
}
