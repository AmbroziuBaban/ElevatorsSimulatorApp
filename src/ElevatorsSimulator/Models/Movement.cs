using ElevatorsSimulator.Common;

namespace ElevatorsSimulator.Models;
public class Movement
{
    public int SourceFloor { get; set; }
    public int DestinationFloor { get; set; }
    public int Load { get; set; }
    public Direction Direction { get; set; }

    public Movement()
    {

    }

    public Movement(int sourceFloor, int destinationFloor, int load, Direction direction)
    {
        SourceFloor = sourceFloor;
        DestinationFloor = destinationFloor;
        Load = load;
        Direction = direction;
    }

    public Movement(int sourceFloor, int destinationFloor, int load)
    : this(sourceFloor, destinationFloor, load, sourceFloor <= destinationFloor ? Direction.Up : Direction.Down)
    {
    }
}
