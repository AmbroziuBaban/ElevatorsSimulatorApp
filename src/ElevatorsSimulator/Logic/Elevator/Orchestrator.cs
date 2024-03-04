using ElevatorsSimulator.Common;
using ElevatorsSimulator.Contracts;
using ElevatorsSimulator.Models;


namespace ElevatorsSimulator.Logic;
public class Orchestrator : IOrchestrator
{
    private IElevatorPool _elevatorPool;
    private ITaskManager _taskManager;
    public int NumberOfFloors { get; set; }

    public Orchestrator(IElevatorPool elevatorPool, ITaskManager taskManager)
    {
        _elevatorPool = elevatorPool;
        _taskManager = taskManager;
    }

    public void AddElevator(int loadLimit)
    {
        _elevatorPool.AddElevator(loadLimit);
    }

    public string AddRequest(ElevatorRequest request)
    {
        string response = string.Empty;

        if (request.SourceFloor > NumberOfFloors - 1 || request.DestinationFloor > NumberOfFloors - 1)
        {
            response = $"{StringConstants.ERROR_INVALID_INPUT} {StringConstants.ERROR_INVALID_FLOOR_VALUE}";
        }
        else if (request.Load < 0)
        {
            response = $"{StringConstants.ERROR_INVALID_INPUT} {StringConstants.ERROR_INVALID_LOAD_VALUE}";
        }
        else
        {
            int elevatorLoadLimit = _elevatorPool.GetElevatorLimit();
            List<ElevatorRequest> requests = new List<ElevatorRequest>();

            if (request.Load > elevatorLoadLimit)
            {
                var splitRequests = SplitRequest(request, elevatorLoadLimit);
                requests.AddRange(splitRequests);
            }
            else
            {
                requests.Add(request);
            }

            var resultElevators = requests.Select(r => _taskManager.RegisterRequest(r)).ToList();
            response = resultElevators.Count > 0 ? string.Join(" & ", resultElevators) : StringConstants.ERROR_NO_OPTIMAL_ELEVATOR;
        }

        return response;
    }

    public string[] GetElevatorsStatus()
    {
        return _elevatorPool.GetElevatorInfo();
    }

    public void ExecuteStep()
    {
        var currentMovements = _taskManager.GetCurrentMovements();
        List<string> elevatorsWithCurrentMovementFinished = new List<string>();
        foreach (var currentMovement in currentMovements)
        {
            Elevator elevator = _elevatorPool.Elevators.First(e => e.Id == currentMovement.ElevatorId);
            if (elevator.ElevatorStatus.CurrentFloor == currentMovement.Movement.DestinationFloor)
            {
                elevator.Wait();
                elevatorsWithCurrentMovementFinished.Add(currentMovement.ElevatorId);
            }
            else
            {
                if (elevator.ElevatorStatus.CurrentFloor == currentMovement.Movement.SourceFloor)
                {
                    elevator.ElevatorStatus.CurrentLoad = currentMovement.Movement.Load;
                }

                if (currentMovement.Movement.Direction == Common.Direction.Up)
                {
                    elevator.GoUp();
                }
                else if (currentMovement.Movement.Direction == Common.Direction.Down)
                {
                    elevator.GoDown();
                }
            }
        }

        _taskManager.RemoveCurrentMovement(elevatorsWithCurrentMovementFinished);
    }

    private List<ElevatorRequest> SplitRequest(ElevatorRequest elevatorRequest, int elevatorLoadLimit)
    {
        List<ElevatorRequest> elevatorRequests = new List<ElevatorRequest>();

        int remLoad = elevatorRequest.Load;
        while (remLoad > elevatorLoadLimit)
        {
            elevatorRequests.Add(new ElevatorRequest(elevatorRequest.SourceFloor, elevatorRequest.DestinationFloor, elevatorLoadLimit));
            remLoad -= elevatorLoadLimit;
        }
        elevatorRequests.Add(new ElevatorRequest(elevatorRequest.SourceFloor, elevatorRequest.DestinationFloor, remLoad));

        return elevatorRequests;
    }
}
