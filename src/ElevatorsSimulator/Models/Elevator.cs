using ElevatorsSimulator.Common;

namespace ElevatorsSimulator.Models;
public class Elevator
{
    public string Id { get; set; } = "";
    public ElevatorStatus ElevatorStatus { get; private set; } = new ElevatorStatus();
    public int LoadLimit { get; set; }

    public void GoUp()
    {
        ElevatorStatus.Direction = Direction.Up;
        ElevatorStatus.CurrentFloor++;
    }

    public void GoDown()
    {
        ElevatorStatus.Direction = Direction.Down;
        ElevatorStatus.CurrentFloor--;
    }

    public void Wait()
    {
        ElevatorStatus.Direction = Direction.None;
    }

    public void ChangeLoad(int loadDifference)
    {
        if (ElevatorStatus.CurrentLoad + loadDifference > LoadLimit)
        {
            throw new Exception("Cannot load all people.");
        }
        ElevatorStatus.CurrentLoad += loadDifference;
    }
}
