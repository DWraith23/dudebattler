using System;
using Godot;

namespace DudeBattler.Resources.Shape2D;

[Tool, GlobalClass]
public partial class LineData : Shape2DData
{
    public override ShapeType Type { get; } = ShapeType.Line;

    private Vector2 _start = Vector2.Zero;
    private Vector2 _end = new(10, 10);
    private float _width = 1.0f;
    private Color _color = Colors.Black;

    [Export]
    public Vector2 Start
    {
        get => _start;
        set
        {
            _start = value;
            EmitChanged();
        }
    }

    [Export]
    public Vector2 End
    {
        get => _end;
        set
        {
            _end = value;
            EmitChanged();
        }
    }

    [Export]
    public float Width
    {
        get => _width;
        set
        {
            _width = value;
            EmitChanged();
        }
    }

    [Export]
    public Color Color
    {
        get => _color;
        set
        {
            _color = value;
            EmitChanged();
        }
    }
}
