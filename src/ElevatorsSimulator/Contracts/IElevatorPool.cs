using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Contracts;
public interface IElevatorPool
{
    public IEnumerable<Elevator> Elevators { get; }
    public void AddElevator(int loadLimit);
    public int GetElevatorLimit();
    public string[] GetElevatorInfo();

}
