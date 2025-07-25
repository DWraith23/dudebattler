using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Scenes.Components.ResourceIcons;

[GlobalClass, Tool]
public partial class SubIconImage : SubIcon
{
    private float _percentCovered = 0.25f;
    [Export]
    public float PercentCovered
    {
        get => _percentCovered;
        set
        {
            if (_percentCovered == value) return;
            _percentCovered = value;
            this.EmitChangedLogged();
        }
    }

    private Texture2D? _image;
    [Export]
    public Texture2D? Image
    {
        get => _image;
        set
        {
            if (_image == value) return;
            _image = value;
            this.EmitChangedLogged();
        }
    }

    private Color _modulation = Colors.White;
    [Export]
    public Color Modulation
    {
        get => _modulation;
        set
        {
            if (_modulation == value) return;
            _modulation = value;
            this.EmitChangedLogged();
        }
    }

    public override TextureRect GenerateSubIcon(Quadrant quadrant, int size)
    {
        var rect = new TextureRect()
        {
            Texture = Image,
            ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
            CustomMinimumSize = new Vector2(size * PercentCovered, size * PercentCovered),
            MouseFilter = Control.MouseFilterEnum.Ignore,
            Modulate = Modulation,
        };

        var xOffset = quadrant == Quadrant.UpperLeft || quadrant == Quadrant.LowerLeft ? 0 : size - (size * PercentCovered);
        var yOffset = quadrant == Quadrant.UpperLeft || quadrant == Quadrant.UpperRight ? 0 : size - (size * PercentCovered);

        rect.Position = new Vector2(xOffset, yOffset);

        return rect;
    }
}
