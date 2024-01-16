namespace MauiApp.Web;

public class ColorChanger
{
    private readonly object _lock = new();
    private readonly List<Action<string>> _actions = new();

    public void ChangeColor(string color)
    {
        lock (_lock)
        {
            foreach (var action in _actions)
            {
                action(color);
            }
        }
    }

    public void Register(Action<string> action)
    {
        lock (_lock)
        {
            _actions.Add(action);
        }
    }
}
