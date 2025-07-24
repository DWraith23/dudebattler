using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace DudeBattler.Resources.Dude;

[GlobalClass, Tool]
public partial class RaceData : DisplayResource
{
    public enum Race
    {
        Human,
        Dwarf,
        Elf,
        Orc
    }

    [Export] public override string? Name { get; set; }
    [Export] public override string? Description { get; set; }
    [Export] public Race RaceType { get; set; } = Race.Human;

    [ExportGroup("Racial Features")]
    [ExportSubgroup("Visual")]
    [Export] public Color ModelColor { get; set; }
    [Export] public Vector2 Scale { get; set; }

    [ExportSubgroup("Racial Traits")]
    [Export]
    public Godot.Collections.Dictionary<StatBlock.Stat, int>? StatModifiers { get; set; } = new()
    {
        { StatBlock.Stat.Strength, 0 },
        { StatBlock.Stat.Agility, 0 },
        { StatBlock.Stat.Skill, 0 },
        { StatBlock.Stat.Endurance, 0 },
        { StatBlock.Stat.Mind, 0 },
        { StatBlock.Stat.Will, 0 },
        { StatBlock.Stat.Personality, 0 },
        { StatBlock.Stat.Luck, 0 },
    };

    // TODO: Racial quirks
}
