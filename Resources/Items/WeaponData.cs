using Godot;

namespace DudeBattler.Resources.Items;

[GlobalClass]
public partial class WeaponData : ItemData
{
    [Export]
    public override string? Name { get; set; } = string.Empty;
    [Export(PropertyHint.MultilineText)]
    public override string? Description { get; set; } = string.Empty;

    [Export] public int IconSize { get; set; } = 128;
    [Export] public Vector2 IconOffset { get; set; } = Vector2.Zero;

    // Weapon stats
    [Export] public Vector2I DamageDice { get; set; } = new(1, 6);

}
