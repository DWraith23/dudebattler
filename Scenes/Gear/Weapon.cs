using System.Collections.Generic;
using System.Threading.Tasks;
using DudeBattler.Resources.Items;
using DudeBattler.Scenes.Components;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Scenes.Gear;

public partial class Weapon : Node2D
{
    #region Enums

    public enum WeaponType
    {
        Sword,
        
    }

    public static Dictionary<WeaponType, string> WeaponNames => new()
    {
        { WeaponType.Sword, "Sword" },
    };

    public static Dictionary<WeaponType, string> WeaponPaths => new()
    {
        { WeaponType.Sword, "res://Scenes/Gear/sword.tscn" },
    };

    #endregion

    [Export] public WeaponData? Data { get; set; }

    /// <summary>
    /// Generates a dynamic icon for the weapon if the weapon is already loaded in the scenetree.
    /// </summary>
    /// <returns></returns>
    public async Task GenerateIcon()
    {
        if (GetChild(0) is not Shape2d shape || Data == null) return;
        var viewport = new SubViewport()
        {
            Size = new Vector2I(256, 256)
        };
        var centerer = new CenterContainer();
        var control = new Control();
        AddChild(viewport);
        viewport.AddChild(centerer);
        centerer.AddChild(control);
        control.AddChild(shape.Duplicate());
        await Tools.AwaitProcessFrame(this);
        Data.Icon = viewport.GetTexture();
    }

    /// <summary>
    /// Generates an icon for a weapon that is not loaded in the scenetree.
    /// Requires an active scene to use.
    /// </summary>
    /// <param name="currentScene"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static async Task<WeaponData?> GenerateData(Node currentScene, NodePath path)
    {
        var weapon = GD.Load<PackedScene>(path).Instantiate<Weapon>();
        if (weapon == null || weapon.Data == null) return null;
        var viewport = new SubViewport()
        {
            Size = new Vector2I(weapon.Data.IconSize, weapon.Data.IconSize)
        };
        var centerer = new CenterContainer();
        centerer.SetAnchorsAndOffsetsPreset(Control.LayoutPreset.FullRect);
        var control = new Control();
        currentScene.AddChild(viewport);
        viewport.AddChild(centerer);
        centerer.AddChild(control);
        control.AddChild(weapon);
        weapon.Position = weapon.Position + weapon.Data.IconOffset;
        await Tools.AwaitProcessFrame(currentScene);
        weapon.Data.Icon = viewport.GetTexture();
        return weapon.Data;
    }
}
