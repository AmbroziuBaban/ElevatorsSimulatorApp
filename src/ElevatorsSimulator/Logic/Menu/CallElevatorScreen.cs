using ElevatorsSimulator.Common;
using ElevatorsSimulator.Logic.Base;
using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Logic;
public class CallElevatorScreen : MenuScreen
{
    private int? _requestSourceFloor;
    private int? _requestDestinationFloor;
    private int? _requestLoad;

    public CallElevatorScreen(string title) : base(title)
    {
    }

    public override void DisplayScreen()
    {
        Console.Clear();

        string text = GetGenericScreenInfo();

        text += "Current floor number=";

        if (_requestSourceFloor != null)
        {
            text += $"{_requestSourceFloor} \nDestination Floor Number=";

            if (_requestDestinationFloor != null)
            {
                text += $"{_requestDestinationFloor}\nNumber of Persons=";

                if (_requestLoad != null)
                {
                    text += $"{_requestLoad}\nAre you sure you want to call the elevator? \ny/n";
                }
            }
        }

        Console.WriteLine(text);
    }

    public override string TryExecuteCommand(string command)
    {
        string response = string.Empty;

        if (command == StringConstants.ITEM_KEY_BACK_ELEVATOR_REQUEST)
        {
            if (!_menuItems.TryGetValue(StringConstants.ITEM_KEY_BACK_ELEVATOR_REQUEST, out var menuItem))
            {
                response = "Invalid back command.";
            }
            else
            {
                response = menuItem.Command(string.Empty);
            }

            ResetRequest();
        }
        else if (_requestSourceFloor == null)
        {
            _requestSourceFloor = GetInputValue(command);
        }
        else if (_requestDestinationFloor == null)
        {
            _requestDestinationFloor = GetInputValue(command);
        }
        else if (_requestLoad == null)
        {
            _requestLoad = GetInputValue(command);
        }
        else
        {
            if (command == "y")
            {
                ElevatorRequest request = new ElevatorRequest(_requestSourceFloor.Value, _requestDestinationFloor.Value, _requestLoad.Value);

                var selectedMenuItemToExecute = _menuItems.FirstOrDefault(item => item.Value.Title == StringConstants.ITEM_TITLE_ELEVATOR_REQUEST_INPUT);
                var externalCommand = selectedMenuItemToExecute.Value.Command;

                response = externalCommand(request);
            }

            ResetRequest();
        }

        return response;
    }

    private int GetInputValue(string? input)
    {
        if (input == null || !Int32.TryParse(input, out int inputValue))
        {
            ResetRequest();
            throw new Exception("Invalid input value.");
        }

        return inputValue;
    }

    private void ResetRequest()
    {
        _requestSourceFloor = null;
        _requestDestinationFloor = null;
        _requestLoad = null;
    }
}
