namespace ElevatorsSimulator.Models;
public class ElevatorRequest
{
    public int SourceFloor { get; set; }
    public int DestinationFloor { get; set; }
    public int Load { get; set; }

    public ElevatorRequest()
    {
    }

    public ElevatorRequest(int sourceFloor, int destinationFloor, int load)
    {
        SourceFloor = sourceFloor;
        DestinationFloor = destinationFloor;
        Load = load;
    }
}
