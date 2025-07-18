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

    [Export] public StatBlock Stats { get; set; } = new();
    [Export] public Equipment Equipment { get; set; } = new();
    [Export] public Statuses StatusEffects { get; set; } = new();


    #endregion

    #region Statistics Calculations



    #endregion


}
