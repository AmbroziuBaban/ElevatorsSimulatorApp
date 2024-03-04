namespace ElevatorsSimulator.Models;
public class MenuItem
{
    public string Title { get; private set; }
    public Func<object, string> Command { get; set; }

    public MenuItem(string title, Action command)
    {
        Title = title;

        Command = (object obj) =>
        {
            command.Invoke();
            return string.Empty;
        };
    }

    public MenuItem(string title, Func<object, string> command)
    {
        Title = title;
        Command = command;
    }
}
