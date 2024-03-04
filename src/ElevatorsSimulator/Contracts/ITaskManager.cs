using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Contracts;
public interface ITaskManager
{
    /// <summary>
    /// Assign the request to an elevator and return  that Elevator Id
    /// </summary>
    /// <param name="request"></param>
    /// <returns>elevatorId</returns>
    /// <exception cref="Exception"></exception>
    public string RegisterRequest(ElevatorRequest request);
    public List<ElevatorMovement> GetCurrentMovements();
    public void RemoveCurrentMovement(List<string> elevatorsIds);
}
