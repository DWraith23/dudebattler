using Godot;

namespace dudebattler.Resources.Shape2D;

[Tool, GlobalClass]
public partial class CircleData : Shape2DData
{
    public override ShapeType Type { get; } = ShapeType.Circle;

    private Vector2 _position = Vector2.Zero;
    private float _radius = 10.0f;
    private Color _color = Colors.Black;
    private bool _filled = true;
    private float _width = -1.0f;
    private bool _antialiased = false;

    [Export]
    public Vector2 Position
    {
        get => _position;
        set
        {
            _position = value;
            EmitChanged();
        }
    }

    [Export]
    public float Radius
    {
        get => _radius;
        set
        {
            _radius = value;
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

    [Export]
    public bool Filled
    {
        get => _filled;
        set
        {
            _filled = value;
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
    public bool Antialiased
    {
        get => _antialiased;
        set
        {
            _antialiased = value;
            EmitChanged();
        }
    }
}

// void CanvasItem.DrawCircle(Vector2 position, float radius, Color color, bool filled = true, float width = -1, bool antialiased = false)
