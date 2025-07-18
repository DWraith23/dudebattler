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

    /// <summary>
    /// Checks if a node is null and still in memory.
    /// If so, queues it to free from memory.
    /// </summary>
    /// <param name="node"></param>
    public static void Delete(this Node? node)
    {
        if (node == null) return;
        if (GodotObject.IsInstanceValid(node)) node.QueueFree();
    }

    public static void EmitSignalLogged(this Resource? resource, StringName signal, params Variant[] args)
    {
        if (resource == null) return;
        resource.EmitSignal(signal, args);
        GD.PrintRich($"[color=light_blue]{resource} emitted signal: {signal} with args {string.Join(", ", args)}");
    }


}