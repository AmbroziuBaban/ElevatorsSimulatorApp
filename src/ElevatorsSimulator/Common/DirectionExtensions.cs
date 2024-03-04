namespace ElevatorsSimulator.Common;
public static class DirectionExtensions
{
    public static string ToString(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                {
                    return "UP";
                }
            case Direction.Down:
                {
                    return "DOWN";
                }
            default:
                {
                    return string.Empty;
                }
        }
    }
}
