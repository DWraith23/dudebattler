using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace dudebattler.Resources.Shape2D;

[Tool, GlobalClass]
public partial class Shape2DData : Resource
{
    [Signal] public delegate void CollisionChangedEventHandler(bool hasCollision);

    public enum ShapeType
    {
        Empty,
        Circle,
        Rectangle,
        Line,
    }

    public virtual ShapeType Type { get; } = ShapeType.Empty;

    private bool _hasCollision = false;
    [Export]
    public bool HasCollision
    {
        get => _hasCollision;
        set
        {
            if (_hasCollision != value)
            {
                _hasCollision = value;
                EmitSignal(SignalName.CollisionChanged, _hasCollision);
            }
        }
    }
}
