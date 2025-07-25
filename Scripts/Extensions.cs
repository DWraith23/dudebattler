using System;
using System.Threading.Tasks;
using Godot;

namespace DudeBattler.Scripts;

/// <summary>
/// This class contains extension methods for various types
/// to enhance functionality and provide additional utilities.
/// </summary>
public static class Extensions
{

    #region Nodes and Resources
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

    /// <summary>
    /// Deletes all of a node's children.
    /// </summary>
    /// <param name="node"></param>
    public static void FreeChildren(this Node? node)
    {
        if (node == null) return;
        foreach (var child in node.GetChildren())
        {
            child.Delete();
        }
    }

    /// <summary>
    /// Returns 'true' if a node is non-null, in memory, not queued for deletion, and inside the tree.  Otherwise returns false.
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    public static bool IsValid(this Node? node)
    {
        if (node == null) return false;
        if (!GodotObject.IsInstanceValid(node)) return false;
        if (node.IsQueuedForDeletion()) return false;
        if (!node.IsInsideTree()) return false;
        return true;
    }

    /// <summary>
    /// A simple extension to log signals being emitted.
    /// </summary>
    /// <param name="resource"></param>
    /// <param name="signal"></param>
    /// <param name="args"></param>
    public static void EmitSignalLogged(this GodotObject? resource, StringName signal, params Variant[] args)
    {
        if (resource == null) return;
        resource.EmitSignal(signal, args);
        GD.PrintRich($"[color=light_blue]{resource} emitted signal: {signal} with args {string.Join(", ", args)}");
    }

    /// <summary>
    /// Simplified version of EmitSignalLogged which explicitly emits the Changed signal for Resources.
    /// </summary>
    /// <param name="resource"></param>
    public static void EmitChangedLogged(this Resource? resource)
    {
        if (resource == null) return;
        resource.EmitChanged();
        GD.PrintRich($"[color=light_blue]{resource} emitted Changed signal.");
    }

    /// <summary>
    /// Duplicates a resource of a given type, T.  Returns the explicit type, not the generic Resource type.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="resource"></param>
    /// <param name="deep"></param>
    /// <returns></returns>
    public static T? Duplicate<T>(this Resource? resource, bool deep = false) where T : Resource
    {
        if (resource == null) return null;
        var clone = resource.Duplicate(deep) as T;
        return clone;
    }

    public static void AdoptChild(this Node? node, Node child)
    {
        if (!node.IsValid() || !child.IsValid()) return;
        child.GetParent().RemoveChild(child);
        node?.AddChild(child);
    }

    #endregion


    #region Numbers

    public static int RoundDown(this double value) => (int)Math.Floor(value);
    public static int RoundUp(this double value) => (int)Math.Ceiling(value);
    public static int Round(this double value) => (int)Math.Round(value);

    public static int RoundDown(this float value) => (int)Math.Floor(value);
    public static int RoundUp(this float value) => (int)Math.Ceiling(value);
    public static int Round(this float value) => (int)Math.Round(value);

    #endregion


    #region Events

    public static bool IsMouseInputType(this InputEvent @event, bool left = true, bool pressed = false)
    {
        if (@event is not InputEventMouseButton mouse) return false;
        if (mouse.ButtonMask == MouseButtonMask.Left && !left) return false;
        if (mouse.ButtonMask == MouseButtonMask.Right && left) return false;
        if (mouse.Pressed && !pressed) return false;
        if (!mouse.Pressed && pressed) return false;
        return true;
    }

    #endregion
}