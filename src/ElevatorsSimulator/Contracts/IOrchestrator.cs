using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Contracts;
public interface IOrchestrator
{
    public int NumberOfFloors { get; set; }
    public void AddElevator(int loadLimit);
    public string AddRequest(ElevatorRequest request);
    public string[] GetElevatorsStatus();
    public void ExecuteStep();
}
