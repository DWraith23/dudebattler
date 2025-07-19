using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace DudeBattler.Resources.Shape2D;

[GlobalClass, Tool]
public partial class ArcData : Shape2DData
{
    public override ShapeType Type { get; } = ShapeType.Arc;

    private Vector2 _position = Vector2.Zero;
    private float _radius = 10.0f;
    private float _startAngle = 0.0f;
    private float _endAngle = 0.0f;
    private Color _color = Colors.Black;
    private float _width = -1.0f;
    private int _pointCount = 10;
    private bool _antialiased = false;

    [Export]
    public Vector2 Position
    {
        get => _position;
        set
        {
            if (_position == value) return;
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
            if (_radius == value) return;
            _radius = value;
            EmitChanged();
        }
    }

    [Export]
    public float StartAngle
    {
        get => _startAngle;
        set
        {
            if (_startAngle == value) return;
            _startAngle = value;
            EmitChanged();
        }
    }

    [Export]
    public float EndAngle
    {
        get => _endAngle;
        set
        {
            if (_endAngle == value) return;
            _endAngle = value;
            EmitChanged();
        }
    }

    [Export]
    public Color Color
    {
        get => _color;
        set
        {
            if (_color == value) return;
            _color = value;
            EmitChanged();
        }
    }

    [Export]
    public float Width
    {
        get => _width;
        set
        {
            if (_width == value) return;
            _width = value;
            EmitChanged();
        }
    }

    [Export]
    public int PointCount
    {
        get => _pointCount;
        set
        {
            if (_pointCount == value) return;
            _pointCount = value;
            EmitChanged();
        }
    }

    [Export]
    public bool Antialiased
    {
        get => _antialiased;
        set
        {
            if (_antialiased == value) return;
            _antialiased = value;
            EmitChanged();
        }
    }

}
