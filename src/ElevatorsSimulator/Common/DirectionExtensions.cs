namespace ElevatorsSimulator.Common;
public static class DirectionExtensions
{
    public static string ToString(this Direction direction)
    {
        switch (direction)
        {
            case Direction.Up:
                {
                    return StringConstants.DIRECTION_UP_INDICATOR;
                }
            case Direction.Down:
                {
                    return StringConstants.DIRECTION_DOWN_INDICATOR;
                }
            default:
                {
                    return StringConstants.DIRECTION_NONE_INDICATOR;
                }
        }
    }
}
