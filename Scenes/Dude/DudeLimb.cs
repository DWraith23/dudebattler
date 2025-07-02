using DudeBattler.Scripts;
using Godot;
using System;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Dude;

[GlobalClass, Tool]
public partial class DudeLimb : BodyPart
{
    public enum Ordering
    {
        Upper,
        Lower
    }

    public enum Side
    {
        Left,
        Right
    }

    public enum Type
    {
        Arm,
        Leg
    }



    public override Area2D? Area => GetNode<Area2D>("Area");
    public override CollisionShape2D? Collider => GetNode<CollisionShape2D>("Area/Collider");

    public BodyPart? BodyConnection => GetParent<BodyPart>();

    private float _length = 100f;
    [Export]
    public float Length
    {
        get => _length;
        set
        {
            if (_length == value) return;
            _length = value;
            QueueRedraw();
            if (!HasNode("Area") || !HasNode("Area/Collider")) return;
            if (Shape is RectangleShape2D rect)
            {
                var scaler = LimbSide == Side.Right ? 1 : -1;
                rect.Size = new(value, LineWidth);
                if (Collider != null) Collider.Position = new(value / 2 * scaler, 0);
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

    private float _lineWidth = 5f;
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

    private Ordering _limbOrder = Ordering.Upper;
    [Export]
    public Ordering LimbOrder
    {
        get => _limbOrder;
        set
        {
            if (_limbOrder == value) return;
            _limbOrder = value;
            Reposition();
            QueueRedraw();
        }
    }

    private Side _limbSide = Side.Left;
    [Export]
    public Side LimbSide
    {
        get => _limbSide;
        set
        {
            if (_limbSide == value) return;
            _limbSide = value;
            if (HasNode("Area/Collider"))
            {
                var scaler = value == Side.Right ? 1 : -1;
                Collider!.Position = new(Length / 2 * scaler, 0);
            }
            Reposition();
            QueueRedraw();
        }
    }

    private Type _limbType = Type.Arm;
    [Export]
    public Type LimbType
    {
        get => _limbType;
        set
        {
            if (_limbType == value) return;
            _limbType = value;
            Reposition();
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

    private Vector2 GetBodyConnection()
    {
        if (BodyConnection is not DudeBody body) return Vector2.Zero;
        var multiplier = LimbType == Type.Arm ? 0.25f : 1f;
        var location = body.Height * multiplier;
        return new(0, location);
    }

    private Vector2 GetLimbConnection()
    {
        if (BodyConnection is not DudeLimb limb) return Vector2.Zero;
        var scaler = LimbSide == Side.Right ? 1 : -1;
        var location = limb.Length;
        return new(location * scaler, 0);
    }

    public override void _Ready()
    {
        if (!HasNode("Area"))
        {
            GenerateNodes();
        }
        Reposition();
        QueueRedraw();
    }

    public override void _Draw()
    {
        if (Drawn || IsDrawing) DrawCircle(Vector2.Zero, LineWidth / 2, Color, true);
        var scaler = LimbSide == Side.Right ? 1 : -1;
        var start = Vector2.Zero;

        if (IsDrawing)
        {
            DrawLine(start, new Vector2(CurrentLength * scaler, 0), Color, LineWidth);
            return;
        }
        
        var end = new Vector2(scaler * Length, 0f);
        if (Drawn) DrawLine(start, end, Color, LineWidth);
    }

    public override async Task AnimateDrawing(float speed)
    {
        IsDrawing = true;
        var tween = CreateTween().SetEase(Tween.EaseType.In);
        tween.TweenProperty(this, "CurrentLength", Length, speed);
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
            Size = new(Length, LineWidth),
        };
        var scaler = LimbSide == Side.Right ? 1 : -1;
        var collider = new CollisionShape2D
        {
            Name = "Collider",
            Shape = shape,
            Position = new(Length / 2 * scaler, 0),
        };
        area.AddChild(collider, false, InternalMode.Back);
    }

    private void Reposition()
    {
        if (BodyConnection is DudeBody body)
        {
            var connection = GetBodyConnection();
            Position = connection;
            QueueRedraw();
        }
        else if (BodyConnection is DudeLimb limb)
        {
            var connection = GetLimbConnection();
            Position = connection;
            QueueRedraw();
        }
    }
}
