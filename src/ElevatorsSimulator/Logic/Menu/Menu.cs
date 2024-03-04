using ElevatorsSimulator.Common;
using ElevatorsSimulator.Contracts;

namespace ElevatorsSimulator.Logic;
public class Menu : IMenu
{
    private List<IMenuScreen> _menuScreens { get; set; } = new List<IMenuScreen>();
    private int _currentScreenIndex { get; set; } = 0;

    public Menu()
    {
    }

    public void AddScreen(IMenuScreen menuScreen)
    {
        _menuScreens.Add(menuScreen);
    }

    public void DisplayMenu()
    {
        _menuScreens[_currentScreenIndex].DisplayScreen();
    }

    public void ExecuteCommand(string? command)
    {
        string commandResponse = string.Empty;
        if (command == null)
        {
            commandResponse = StringConstants.ERROR_INVALID_INPUT_COMMAND;
        }
        else
        {
            commandResponse = _menuScreens[_currentScreenIndex].TryExecuteCommand(command);
        }

        Console.WriteLine(commandResponse);
    }

    public void NavigateToScreen(string screenTitle)
    {
        var screenIndex = _menuScreens.FindIndex(screen => screen.Title == screenTitle);
        if (screenIndex < 0)
        {
            Console.WriteLine(StringConstants.ERROR_NO_SCREEN);
            return;
        }

        _currentScreenIndex = screenIndex;
    }

    public void Reset()
    {
        _currentScreenIndex = 0;
    }
}
