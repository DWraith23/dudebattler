using System.Threading.Tasks;
using Godot;

namespace DudeBattler.Scripts;

/// <summary>
/// This class contains extension methods for various types
/// to enhance functionality and provide additional utilities.
/// </summary>
public static class Extensions
{

    public static async Task Completed(this Tween tween) => await tween.ToSignal(tween, Tween.SignalName.Finished);

    public static void Delete(this Node? node)
    {
        if (node == null) return;
        if (GodotObject.IsInstanceValid(node)) node.QueueFree();
        node = null;
    }
}