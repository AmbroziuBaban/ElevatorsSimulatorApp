using ElevatorsSimulator.Common;

namespace ElevatorsSimulator.Models;
public class ElevatorStatus
{
    public int CurrentFloor { get; set; }
    public Direction Direction { get; set; }
    public int CurrentLoad { get; set; }
}
