using ElevatorsSimulator.Contracts;
using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Logic.Base;
public abstract class MenuScreen : IMenuScreen
{
    public string Title { get; private set; }
    protected Dictionary<string, MenuItem> _menuItems;

    public MenuScreen(string title) : this(title, new Dictionary<string, MenuItem>())
    {
    }

    public MenuScreen(string title, Dictionary<string, MenuItem> menuItems)
    {
        Title = title;
        _menuItems = menuItems;
    }

    public void AddMenuItem(MenuItem menuItem, string prefferedKey = "")
    {
        _menuItems.Add(prefferedKey == string.Empty ? _menuItems.Count().ToString() : prefferedKey, menuItem);
    }

    public virtual void DisplayScreen()
    {
        Console.Clear();
        string text = $"----------{Title}----------\n\n";
        foreach (var menuItem in _menuItems)
        {
            text += $"{menuItem.Key}. {menuItem.Value.Title}\n";
        }
        Console.WriteLine(text);
    }

    public virtual string TryExecuteCommand(string command)
    {

        var selectedMenuItemToExecute = _menuItems.GetValueOrDefault(command);
        if (selectedMenuItemToExecute == null)
        {
            return "Invalid command.";
        }

        return selectedMenuItemToExecute.Command.Invoke(string.Empty);
    }

    protected string GetGenericScreenInfo()
    {
        string text = $"----------{Title}----------\n\n";
        foreach (var menuItem in _menuItems)
        {
            text += $"{menuItem.Key}. {menuItem.Value.Title}\n";
        }

        return text;
    }
}
