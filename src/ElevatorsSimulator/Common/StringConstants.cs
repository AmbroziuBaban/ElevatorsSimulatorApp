namespace ElevatorsSimulator.Common;
public static class StringConstants
{

    public static readonly string SCREEN_TITLE_MAIN = "Main";
    public static readonly string SCREEN_TITLE_STATUS_ELEVATORS = "Elevators status";
    public static readonly string SCREEN_TITLE_ELEVATOR_REQUEST = "Call elevator";

    public static readonly string ITEM_TITLE_STATUS_RELOAD = "Reload status";
    public static readonly string ITEM_TITLE_BACK = "Back";
    public static readonly string ITEM_TITLE_EXIT = "Exit";
    public static readonly string ITEM_TITLE_ELEVATOR_REQUEST_INPUT = "Input request values";

    public static readonly string ITEM_KEY_BACK_ELEVATOR_REQUEST = "b";

    public static readonly string ERROR_INVALID_INPUT = "Invalid input.";
    public static readonly string ERROR_INVALID_COMMAND = "Invalid command.";
    public static readonly string ERROR_NO_ELEVATORS = "No elevators available.";
    public static readonly string ERROR_NO_ELEVATOR_FOR_ANALYSIS = "Elevator is not setup for analysis.";

    public static readonly string ERROR_INVALID_FLOOR_VALUE = "Invalid floor value.";
    public static readonly string ERROR_INVALID_LOAD_VALUE = "Invalid load value.";
    public static readonly string ERROR_NO_OPTIMAL_ELEVATOR = "No optimal elevator found.";
    public static readonly string ERROR_NO_MOVEMENT = "No movement found.";
    public static readonly string ERROR_INVALID_INPUT_COMMAND = $"{ERROR_INVALID_INPUT} {ERROR_INVALID_COMMAND}";
    public static readonly string ERROR_NO_SCREEN = "No screen that match the description found.";
    public static readonly string ERROR_ELEVATOR_LOAD_EXCEEDING_LIMIT = "Elevator load exceed limit.";  

    public static readonly string AFFIRMATIVE_COMMAND = "y";
    public static readonly string NEGATIVE_COMMAND = "n";
    public static readonly string DIRECTION_UP_INDICATOR = "UP";
    public static readonly string DIRECTION_DOWN_INDICATOR = "DOWN";
    public static readonly string DIRECTION_NONE_INDICATOR = string.Empty;
}
