using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Resources.Dude;

[GlobalClass, Tool]
public partial class StatBlock : Resource
{

    public enum Stat
    {
        Strength,
        Agility,
        Skill,
        Endurance,
        Mind,
        Will,
        Personality,
        Luck
    }

    #region Stats
    private int _strength = 1;
    private int _agility = 1;
    private int _skill = 1;
    private int _endurance = 1;
    private int _mind = 1;
    private int _will = 1;
    private int _personality = 1;
    private int _luck = 1;

    [Export]
    public int Strength
    {
        get => _strength;
        set
        {
            if (_strength == value) return;
            _strength = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Agility
    {
        get => _agility;
        set
        {
            if (_agility == value) return;
            _agility = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Skill
    {
        get => _skill;
        set
        {
            if (_skill == value) return;
            _skill = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Endurance
    {
        get => _endurance;
        set
        {
            if (_endurance == value) return;
            _endurance = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Mind
    {
        get => _mind;
        set
        {
            if (_mind == value) return;
            _mind = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Will
    {
        get => _will;
        set
        {
            if (_will == value) return;
            _will = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Personality
    {
        get => _personality;
        set
        {
            if (_personality == value) return;
            _personality = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    [Export]
    public int Luck
    {
        get => _luck;
        set
        {
            if (_luck == value) return;
            _luck = Math.Clamp(value, 1, 99);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    public Dictionary<Stat, int> StatsDictionary => new()
    {
        { Stat.Strength, Strength },
        { Stat.Agility, Agility },
        { Stat.Skill, Skill },
        { Stat.Endurance, Endurance },
        { Stat.Mind, Mind },
        { Stat.Will, Will },
        { Stat.Personality, Personality },
        { Stat.Luck, Luck }
    };

    #endregion


    #region Methods

    public int GetStat(Stat stat) => StatsDictionary[stat];

    public void SetStat(Stat stat, int value)
    {
        switch (stat)
        {
            case Stat.Strength:
                Strength = value;
                break;
            case Stat.Agility:
                Agility = value;
                break;
            case Stat.Skill:
                Skill = value;
                break;
            case Stat.Endurance:
                Endurance = value;
                break;
            case Stat.Mind:
                Mind = value;
                break;
            case Stat.Will:
                Will = value;
                break;
            case Stat.Personality:
                Personality = value;
                break;
            case Stat.Luck:
                Luck = value;
                break;
            default:
                break;
        }
    }

    public void AddToStat(Stat stat, int amount) => SetStat(stat, GetStat(stat) + amount);
    public void SubtractFromStat(Stat stat, int amount) => SetStat(stat, GetStat(stat) - amount);

    

    #endregion

}
