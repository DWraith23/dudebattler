using System;
using Godot;

namespace DudeBattler.Scenes.Components.ResourceIcons;

[GlobalClass, Tool]
public partial class SubIconText : SubIcon
{
    private string _text;
    [Export]
    public string Text
    {
        get => _text;
        set
        {
            if (_text == value) return;
            _text = value;
            this.EmitChangedLog();
        }
    }

    private Color _color = Colors.White;
    [Export]
    public Color Color
    {
        get => _color;
        set
        {
            if (_color == value) return;
            _color = value;
            this.EmitChangedLog();
        }
    }

    private Color _outlineColor = Colors.Black;
    [Export]
    public Color OutlineColor
    {
        get => _outlineColor;
        set
        {
            if (_outlineColor == value) return;
            _outlineColor = value;
            this.EmitChangedLog();
        }
    }

    public override Label GenerateSubIcon(Quadrant quadrant, int size)
    {
        _ = size;
        var label = new Label()
        {
            Size = Vector2.Zero,
            Text = Text,
            SizeFlagsHorizontal = HorizontalFlags[quadrant],
            SizeFlagsVertical = VerticalFlags[quadrant],
            MouseFilter = Control.MouseFilterEnum.Ignore
        };
        label.SetAnchorsAndOffsetsPreset(LayoutMode[quadrant]);
        label.AddThemeColorOverride("font_color", Color);
        label.AddThemeColorOverride("font_outline_color", OutlineColor);
        label.AddThemeConstantOverride("outline_size", 4);
        return label;
    }
}
