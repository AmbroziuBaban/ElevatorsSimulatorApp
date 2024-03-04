namespace ElevatorsSimulator.Contracts;
public interface IMenu
{
    public void AddScreen(IMenuScreen menuScreen);
    public void DisplayMenu();
    public void ExecuteCommand(string? command);
    public void NavigateToScreen(string screenTitle);
    public void Reset();
}
