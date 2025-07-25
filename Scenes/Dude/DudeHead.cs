using DudeBattler.Scripts;
using Godot;
using System;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Dude;

[GlobalClass, Tool]
public partial class DudeHead : BodyPart
{

    public override Area2D? Area => GetNode<Area2D>("Area");
    public override CollisionShape2D? Collider => GetNode<CollisionShape2D>("Area/Collider");
    public override Vector2 ConnectionPoint => new(0, Radius);


    private float _radius = 50f;
    [Export] public float Radius
    {
        get => _radius;
        set
        {
            if (_radius == value) return;
            _radius = value;
            QueueRedraw();
            if (!HasNode("Area") || !HasNode("Area/Collider")) return;
            if (Shape is CircleShape2D circle)
            {
                circle.Radius = value;
            }
        }
    }

    private Color _color = Colors.Black;
    [Export] public override Color Color
    {
        get => _color;
        set
        {
            if (_color == value) return;
            _color = value;
            QueueRedraw();
        }
    }

    private bool _filled = false;
    [Export] public bool Filled
    {
        get => _filled;
        set
        {
            if (_filled == value) return;
            _filled = value;
            QueueRedraw();
        }
    }

    private float _lineWidth = 5f;
    [Export] public override float LineWidth
    {
        get => _lineWidth;
        set
        {
            if (_lineWidth == value) return;
            _lineWidth = value;
            QueueRedraw();
        }
    }

    private float _arcAngle = 0f;
    private float ArcAngle
    {
        get => _arcAngle;
        set
        {
            if (_arcAngle == value) return;
            _arcAngle = value;
            QueueRedraw();
        }
    }

    public override void _Ready()
    {
        if (!HasNode("Area"))
        {
            GenerateNodes();
        }
    }

    public override void _Draw()
    {
        if (IsDrawing)
        {
            DrawArc(Vector2.Zero, Radius, 0f, ArcAngle * 0.0174533f, (int)Math.Max(2f, ArcAngle), Color, LineWidth);
            return;
        }
        if (Drawn)
        {
            DrawCircle(Vector2.Zero, Radius, Color, Filled, LineWidth);
        }

    }

    public override async Task AnimateDrawing(float speed)
    {
        IsDrawing = true;
        var tween = CreateTween().SetEase(Tween.EaseType.In);
        tween.TweenProperty(this, "ArcAngle", 360f, speed);
        await tween.Completed();
        IsDrawing = false;
        Drawn = true;
        QueueRedraw();
    }


    protected override void GenerateNodes()
    {
        var area = new Area2D()
        {
            Name = "Area"
        };
        AddChild(area, false, InternalMode.Back);

        var shape = new CircleShape2D()
        {
            Radius = Radius
        };

        var collider = new CollisionShape2D
        {
            Name = "Collider",
            Shape = shape
        };
        area.AddChild(collider, false, InternalMode.Back);
    }
}
