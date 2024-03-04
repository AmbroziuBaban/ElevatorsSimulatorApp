namespace ElevatorsSimulator.Models;
public class ElevatorRequestCost
{
    public string ElevatorId { get; private set; }
    public int ElevatorMovementCost { get; private set; }

    public ElevatorRequestCost(string elevatorId, int cost)
    {
        ElevatorId = elevatorId;
        ElevatorMovementCost = cost;
    }
}
