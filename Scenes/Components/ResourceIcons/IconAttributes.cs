using System.Collections.Generic;
using DudeBattler.Scripts;
using Godot;
using static DudeBattler.Scenes.Components.ResourceIcons.SubIcon;

namespace DudeBattler.Scenes.Components.ResourceIcons;

[GlobalClass, Tool]
public partial class IconAttributes : Resource
{
    public Dictionary<Quadrant, SubIcon?> SubIcons => new()
    {
        { Quadrant.UpperRight, UpperRight },
        { Quadrant.LowerRight, LowerRight },
        { Quadrant.UpperLeft, UpperLeft },
        { Quadrant.LowerLeft, LowerLeft },
    };

    public List<SubIcon?> SubIconList =>
    [
        UpperRight,
        LowerRight,
        UpperLeft,
        LowerLeft,
    ];

    private string _overlayText = string.Empty;
    private SubIcon? _upperLeft;
    private SubIcon? _upperRight;
    private SubIcon? _lowerLeft;
    private SubIcon? _lowerRight;

    [Export]
    public string OverlayText
    {
        get => _overlayText;
        set
        {
            if (_overlayText == value) return;
            _overlayText = value;
            this.EmitChangedLogged();
        }
    }

    [Export]
    public SubIcon? UpperLeft
    {
        get => _upperLeft;
        set
        {
            if (_upperLeft == value) return;
            _upperLeft = value;
            this.EmitChangedLogged();
            if (_upperLeft != null) _upperLeft.Changed += SendChangedSignal;
        }
    }

    [Export]
    public SubIcon? UpperRight
    {
        get => _upperRight;
        set
        {
            if (_upperRight == value) return;
            _upperRight = value;
            this.EmitChangedLogged();
            if (_upperRight != null) _upperRight.Changed += SendChangedSignal;
        }
    }

    [Export]
    public SubIcon? LowerLeft
    {
        get => _lowerLeft;
        set
        {
            if (_lowerLeft == value) return;
            _lowerLeft = value;
            this.EmitChangedLogged();
            if (_lowerLeft != null) _lowerLeft.Changed += SendChangedSignal;
        }
    }

    [Export]
    public SubIcon? LowerRight
    {
        get => _lowerRight;
        set
        {
            if (_lowerRight == value) return;
            _lowerRight = value;
            this.EmitChangedLogged();
            if (_lowerRight != null) _lowerRight.Changed += SendChangedSignal;
        }
    }
    
    private void SendChangedSignal() => this.EmitChangedLogged();
}
