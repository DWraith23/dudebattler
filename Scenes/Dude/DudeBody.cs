using DudeBattler.Scripts;
using Godot;
using System;
using System.Threading.Tasks;


namespace DudeBattler.Scenes.Dude;

[GlobalClass, Tool]
public partial class DudeBody : BodyPart
{

    public override Area2D? Area => GetNode<Area2D>("Area");
    public override CollisionShape2D? Collider => GetNode<CollisionShape2D>("Area/Collider");
    public override Vector2 ConnectionPoint => Vector2.Zero;

    public DudeHead? Head => GetParent<DudeHead>();

    private float _height = 350;
    [Export]
    public float Height
    {
        get => _height;
        set
        {
            if (_height == value) return;
            _height = value;
            QueueRedraw();
            if (Shape is RectangleShape2D rect)
            {
                rect.Size = new(LineWidth, value);
            }
            if (Collider != null)
            {
                Collider.Position = new(0, value / 2);
            }
        }
    }

    private Color _color = Colors.Black;
    [Export]
    public Color Color
    {
        get => _color;
        set
        {
            if (_color == value) return;
            _color = value;
            QueueRedraw();
        }
    }

    private float _lineWidth = 10f;
    [Export]
    public float LineWidth
    {
        get => _lineWidth;
        set
        {
            if (_lineWidth == value) return;
            _lineWidth = value;
            QueueRedraw();
        }
    }

    private float _currentLength = 0f;
    private float CurrentLength
    {
        get => _currentLength;
        set
        {
            if (_currentLength == value) return;
            _currentLength = value;
            QueueRedraw();
        }
    }

    public override void _Ready()
    {
        if (!HasNode("Area"))
        {
            GenerateNodes();
        }
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (Head != null) Position = new(0, Head.Radius);
        var start = Vector2.Zero;
        if (IsDrawing)
        {
            DrawLine(start, new Vector2(0, CurrentLength), Color, LineWidth);
            return;
        }
        
        var end = new Vector2(0, Height);
        if (Drawn) DrawLine(start, end, Color, LineWidth);
    }

    public override async Task AnimateDrawing(float speed)
    {
        IsDrawing = true;
        var tween = CreateTween().SetEase(Tween.EaseType.In);
        tween.TweenProperty(this, "CurrentLength", Height, speed);
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

        var shape = new RectangleShape2D()
        {
            Size = new(LineWidth, Height),
        };

        var collider = new CollisionShape2D
        {
            Name = "Collider",
            Shape = shape,
            Position = new(0, Height / 2),
        };
        area.AddChild(collider, false, InternalMode.Back);
    }
}
