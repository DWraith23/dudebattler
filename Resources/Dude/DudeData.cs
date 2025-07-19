using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DudeBattler.Resources;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Resources.Dude;

[GlobalClass, Tool]
public partial class DudeData : DisplayResource
{
    #region Signals
    [Signal] public delegate void DiedEventHandler();


    #endregion

    #region Initialization

    public DudeData()
    {
        ConnectSignals();
    }

    private void ConnectSignals()
    {
        Stats.Changed += () => this.EmitSignalLogged(Resource.SignalName.Changed);
        Equipment.Changed += () => this.EmitSignalLogged(Resource.SignalName.Changed);
        StatusEffects.Changed += () => this.EmitSignalLogged(Resource.SignalName.Changed);
    }

    #endregion


    #region Properties and Fields

    #region Fields

    private int _currentHealth = 0;
    private int _currentEnergy = 0;
    private int _currentMana = 0;
    private int _currentShields = 0;


    #endregion

    // Resources
    [Export] public StatBlock Stats { get; set; } = new();
    [Export] public Equipment Equipment { get; set; } = new();
    [Export] public Statuses StatusEffects { get; set; } = new();

    // Vitals
    public int MaxHealth => GetMaxHealth();
    public int MaxEnergy => GetMaxEnergy();
    public int MaxMana => GetMaxMana();

    public int CurrentHealth
    {
        get => _currentHealth;
        set
        {
            if (_currentHealth == value) return;
            _currentHealth = Math.Clamp(value, 0, MaxHealth);
            this.EmitSignalLogged(Resource.SignalName.Changed);
            if (_currentHealth <= 0)
            {
                this.EmitSignalLogged(SignalName.Died);
            }
        }
    }

    public int CurrentEnergy
    {
        get => _currentEnergy;
        set
        {
            if (_currentEnergy == value) return;
            _currentEnergy = Math.Clamp(value, 0, MaxEnergy);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }
    
    public int CurrentMana
    {
        get => _currentMana;
        set
        {
            if (_currentMana == value) return;
            _currentMana = Math.Clamp(value, 0, MaxMana);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    public int CurrentShields
    {
        get => _currentShields;
        set
        {
            if (_currentShields == value) return;
            _currentShields = Math.Clamp(value, 0, MaxMana);
            this.EmitSignalLogged(Resource.SignalName.Changed);
        }
    }

    #endregion

    #region Statistics Calculations

    private int GetMaxHealth()
    {
        var basic = 15;
        var stats = (int)Math.Floor(Stats.Endurance * 1.5f);
        stats += (int)Math.Floor(Stats.Strength * 0.5f);
        // TODO: Effects/other
        return basic + stats;
    }

    private int GetMaxEnergy()
    {
        var basic = 20;
        var stats = (int)Math.Floor(Stats.Endurance * 2f);
        stats += (int)Math.Floor(Stats.Strength * 0.5f);
        stats += (int)Math.Floor(Stats.Agility * 0.5f);
        // TODO: Effects/other
        return basic + stats;
    }

    private int GetMaxMana()
    {
        var basic = 15;
        var stats = (Stats.Mind * 1.5f).RoundDown();
        stats += Stats.Will;
        // TODO: Effects/other
        return basic + stats;
    }


    #endregion


}
