using System.Collections.Generic;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Scenes.Components.ResourceIcons;

[GlobalClass, Tool]
public abstract partial class SubIcon : Resource
{
    public enum Quadrant
    {
        UpperLeft,
        UpperRight,
        LowerLeft,
        LowerRight
    }

    protected static Dictionary<Quadrant, Control.SizeFlags> HorizontalFlags => new()
    {
        { Quadrant.UpperLeft, Control.SizeFlags.ShrinkBegin },
        { Quadrant.UpperRight, Control.SizeFlags.ShrinkEnd },
        { Quadrant.LowerLeft, Control.SizeFlags.ShrinkBegin },
        { Quadrant.LowerRight, Control.SizeFlags.ShrinkEnd },
    };

    protected static Dictionary<Quadrant, Control.SizeFlags> VerticalFlags => new()
    {
        { Quadrant.UpperLeft, Control.SizeFlags.ShrinkBegin },
        { Quadrant.UpperRight, Control.SizeFlags.ShrinkBegin },
        { Quadrant.LowerLeft, Control.SizeFlags.ShrinkEnd },
        { Quadrant.LowerRight, Control.SizeFlags.ShrinkEnd },
    };

    protected static Dictionary<Quadrant, Control.LayoutPreset> LayoutMode => new()
    {
        { Quadrant.UpperLeft, Control.LayoutPreset.TopLeft },
        { Quadrant.UpperRight, Control.LayoutPreset.TopRight },
        { Quadrant.LowerLeft, Control.LayoutPreset.BottomLeft },
        { Quadrant.LowerRight, Control.LayoutPreset.BottomRight },
    };

    private bool _visible = true;
    [Export]
    public bool Visible
    {
        get => _visible;
        set
        {
            if (_visible == value) return;
            _visible = value;
            this.EmitChangedLogged();
        }
    }
    public virtual Control? GenerateSubIcon(Quadrant quadrant, int size) => null;
}
