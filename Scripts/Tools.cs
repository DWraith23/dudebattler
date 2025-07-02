using System;
using System.Threading.Tasks;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Scripts;

/// <summary>
/// This class contains utility methods and tools that can be used throughout the game.
/// It serves as a collection of static methods that can be called without instantiating an object.
/// </summary>
public static class Tools
{
    /// <summary>
    /// A static instance of the Random class for generating random numbers.
    /// This can be used throughout the game to ensure consistent random number generation.
    /// </summary>
    public static Random Rand => new();


    public static async Task TweenToAnimationStart(Node node, AnimationPlayer animator, string animationName, float duration)
    {
        if (animator.HasAnimation(animationName))
        {
            var animation = animator.GetAnimation(animationName);
            var count = animation.GetTrackCount();

            var tween = node.GetTree().CreateTween().SetParallel(true);

            for (int i = 0; i < count; i++)
            {
                var path = animation.TrackGetPath(i).ToString();
                var nodePath = path[..path.IndexOf(':')];
                var tweenNode = node.GetNode(nodePath);
                var property = path[(path.IndexOf(':') + 1)..];
                Variant value;
                var type = animation.TrackGetType(i);
                if (type == Animation.TrackType.Value)
                {
                    value = animation.TrackGetKeyValue(i, 0);
                }
                else if (type == Animation.TrackType.Bezier)
                {
                    value = animation.BezierTrackGetKeyValue(i, 0);
                }
                else
                {
                    GD.PrintErr($"Unsupported track type: {type}");
                    continue;
                }
                tween.TweenProperty(tweenNode, property, value, duration);
            }
            await tween.Completed();
        }
        else
        {
            GD.PrintErr($"Animation '{animationName}' not found in AnimationPlayer.");
        }
    }
}