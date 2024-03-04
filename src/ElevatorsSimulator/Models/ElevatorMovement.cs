namespace ElevatorsSimulator.Models;
public class ElevatorMovement
{
    public string ElevatorId { get; set; }
    public Movement Movement { get; set; }

    public ElevatorMovement(string elevatorId, Movement movement)
    {
        ElevatorId = elevatorId;
        Movement = movement;
    }
}
