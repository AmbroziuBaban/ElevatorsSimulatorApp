using ElevatorsSimulator.Common;
using ElevatorsSimulator.Contracts;
using ElevatorsSimulator.Models;
using Microsoft.Extensions.Logging;

namespace ElevatorsSimulator.Logic;
public class TaskManager : ITaskManager
{
    private IElevatorPool _elevatorPool;
    private IMovementCostAnalyzer _movementCostAnalyzer;
    private IDictionary<string, LinkedList<Movement>> _elevatorsMovements;
    private ILogger _logger;

    public TaskManager(IElevatorPool elevatorPool, IMovementCostAnalyzer movementCostAnalyzer, ILogger<ITaskManager> logger)
    {
        _elevatorsMovements = new Dictionary<string, LinkedList<Movement>>();
        _elevatorPool = elevatorPool;
        _movementCostAnalyzer = movementCostAnalyzer;
        _logger = logger;
    }

    public string RegisterRequest(ElevatorRequest request)
    {
        return ProcessRequest(request);
    }

    public List<ElevatorMovement> GetCurrentMovements()
    {
        return _elevatorsMovements.Select(em => new ElevatorMovement(em.Key, em.Value.First())).ToList();
    }

    public void RemoveCurrentMovement(List<string> elevatorsIds)
    {
        foreach (var elevatorId in elevatorsIds)
        {
            var currentElevator = _elevatorPool.Elevators.FirstOrDefault(ev => ev.Id == elevatorId);

            if (_elevatorsMovements[elevatorId].Count > 1)
            {
                if (currentElevator != null)
                {
                    var difference = _elevatorsMovements[elevatorId].Skip(1).First().Load - _elevatorsMovements[elevatorId].First().Load;
                    currentElevator.ChangeLoad(difference);
                }

                _elevatorsMovements[elevatorId].RemoveFirst();
            }
            else
            {
                if (currentElevator != null)
                {
                    var currentLoad = _elevatorsMovements[elevatorId].First().Load;
                    currentElevator.ChangeLoad(currentLoad * -1);
                }

                _elevatorsMovements.Remove(elevatorId);
            }
        }
    }

    private string ProcessRequest(ElevatorRequest request)
    {
        // Step 2. Find optimal position
        var optimalElevatorCostChange = GetElevatorsMovementAnalysisMostEfficient(request);
        var requestDirrection = request.SourceFloor <= request.DestinationFloor ? Direction.Up : Direction.Down;

        if (optimalElevatorCostChange == null)
        {
            var resonse = "No optimal elevator found.";
            _logger.LogInformation(resonse);

            return resonse;
        }

        // Step 3. Add the movement        
        if (_elevatorsMovements.TryGetValue(optimalElevatorCostChange.ElevatorId, out LinkedList<Movement>? movements))
        {
            if (movements == null)
            {
                var resonse = "No movement was found.";
                _logger.LogInformation(resonse);

                return resonse;
            }
            // Add last
            if (movements.Count - 1 < optimalElevatorCostChange.MovementStartIndex)
            {
                Movement bringElevatorMovement = new Movement(movements.Last().DestinationFloor, request.SourceFloor, 0);
                Movement movement = new Movement(request.SourceFloor, request.DestinationFloor, request.Load);

                movements.AddLast(bringElevatorMovement);
                movements.AddLast(movement);
            }
            // Add between
            else
            {
                var enumerator = movements.GetEnumerator();

                LinkedListNode<Movement>? currentNode = null;

                for (int i = 0; i < movements.Count; i++)
                {
                    if (i == 0)
                    {
                        currentNode = movements.First ?? new LinkedListNode<Movement>(new Movement());
                    }
                    if (i < optimalElevatorCostChange.MovementStartIndex)
                    {
                        currentNode = currentNode.Next;
                    }
                    else
                    {
                        int initialDestinationFloor = currentNode.Value.DestinationFloor;
                        currentNode.Value.DestinationFloor = request.SourceFloor;

                        if (optimalElevatorCostChange.MovementStartIndex == optimalElevatorCostChange.MovementEndIndex)
                        {
                            var movementForRequest = new Movement(request.SourceFloor,
                                request.DestinationFloor, currentNode.Value.Load + request.Load, requestDirrection);
                            var nodeForRequest = new LinkedListNode<Movement>(movementForRequest);

                            var movementAfterRequest = new Movement(request.DestinationFloor, initialDestinationFloor,
                                currentNode.Value.Load, currentNode.Value.Direction);
                            var nodeAfterRequest = new LinkedListNode<Movement>(movementAfterRequest);

                            movements.AddAfter(currentNode, nodeForRequest);
                            movements.AddAfter(nodeForRequest, nodeAfterRequest);

                            break;
                        }
                    }
                }
            }
        }
        else
        {
            int elevatorCurrentFloor = _elevatorPool.Elevators.First(e => e.Id == optimalElevatorCostChange.ElevatorId).ElevatorStatus.CurrentFloor;
            Movement bringElevatorMovement = new Movement(elevatorCurrentFloor, request.SourceFloor, 0);

            Movement movement = new Movement(request.SourceFloor, request.DestinationFloor, request.Load);

            LinkedList<Movement> currentElevatorMovements = new LinkedList<Movement>();
            currentElevatorMovements.AddLast(bringElevatorMovement);
            currentElevatorMovements.AddLast(movement);
            _elevatorsMovements.TryAdd(optimalElevatorCostChange.ElevatorId, currentElevatorMovements);
        }

        return optimalElevatorCostChange.ElevatorId;
    }


    private MovementCostAnalysisResult? GetElevatorsMovementAnalysisMostEfficient(ElevatorRequest request)
    {
        IList<MovementCostAnalysisResult> results = new List<MovementCostAnalysisResult>();

        foreach (var elevator in _elevatorPool.Elevators)
        {
            if (_elevatorsMovements.TryGetValue(elevator.Id, out LinkedList<Movement>? elevatorMovements))
            {
                _movementCostAnalyzer.LoadData(elevator, elevatorMovements);
                results.Add(_movementCostAnalyzer.GetAnalysisResult(request));
                _movementCostAnalyzer.ClearMovementData();
            }
            else
            {
                MovementCostAnalysisResult result = new MovementCostAnalysisResult();
                result.Cost += Math.Abs(elevator.ElevatorStatus.CurrentFloor - request.SourceFloor);
                result.MovementStartIndex = 0;
                result.MovementEndIndex = 0;
                result.ElevatorId = elevator.Id;
                results.Add(result);
            }
        }

        _movementCostAnalyzer.ClearMovementData();

        return results.OrderBy(result => result.Cost).ToList().FirstOrDefault();
    }
}
