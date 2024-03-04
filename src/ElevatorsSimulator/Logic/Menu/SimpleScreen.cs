using ElevatorsSimulator.Logic.Base;
using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Logic;
public class SimpleScreen : MenuScreen
{
    public SimpleScreen(string title) : base(title)
    {
    }

    public SimpleScreen(string title, Dictionary<string, MenuItem> menuItems) : base(title, menuItems)
    {
    }
}
