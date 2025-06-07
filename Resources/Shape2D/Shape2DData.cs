using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace dudebattler.Resources.Shape2D;

[Tool, GlobalClass]
public partial class Shape2DData : Resource
{
    public enum ShapeType
    {
        Empty,
        Circle,
        Rectangle,
        Line,
    }

    public virtual ShapeType Type { get; } = ShapeType.Empty;
}
