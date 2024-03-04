namespace ElevatorsSimulator.Models;
public class ElevatorTask
{
    public ElevatorRequest ElevatorRequest { get; private set; }
    public int Load { get; private set; }
    public ElevatorTask(ElevatorRequest request, int load)
    {
        ElevatorRequest = request;
        Load = load;
    }
}
