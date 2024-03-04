using ElevatorsSimulator.Models;

namespace ElevatorsSimulator.Contracts;
public interface IMenuScreen
{
    public string Title { get; }
    public void AddMenuItem(MenuItem backItem, string prefferedKey = "");
    public void DisplayScreen();
    public string TryExecuteCommand(string command);
}
