using DudeBattler.Resources.Dude;
using DudeBattler.Scripts;
using Godot;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Dude;

[Tool]
public partial class Model : Node2D
{

    #region Generation

    public override async void _Ready()
    {
        if (Engine.IsEditorHint())
        {

            return;
        }

        if (!Drawn) await AnimateDrawing(0.1f);
        foreach (var part in BodyParts)
        {
            if (!part.HasNode("Area") || part.Area == null) continue;
            part.Area.Visible = UsesCollision;
            part.Area.Monitorable = UsesCollision;
            part.Area.Monitoring = UsesCollision;
        }
    }

    #endregion

    #region Signals


    #endregion

    #region Properties and Fields

    public AnimationPlayer Animator => GetNode<AnimationPlayer>("Animator");

    #region Body Parts
    public DudeHead Head => GetNode<DudeHead>("Head");
    public Face Face => Head.GetNode<Face>("Face");
    public DudeBody Body => Head.GetNode<DudeBody>("Body");
    public DudeLimb LeftUpperArm => Body.GetNode<DudeLimb>("Left Upper Arm");
    public DudeLimb LeftLowerArm => LeftUpperArm.GetNode<DudeLimb>("Left Lower Arm");
    public Marker2D LeftHand => LeftLowerArm.GetNode<Marker2D>("Hand");
    public DudeLimb RightUpperArm => Body.GetNode<DudeLimb>("Right Upper Arm");
    public DudeLimb RightLowerArm => RightUpperArm.GetNode<DudeLimb>("Right Lower Arm");
    public Marker2D RightHand => RightLowerArm.GetNode<Marker2D>("Hand");
    public DudeLimb LeftUpperLeg => Body.GetNode<DudeLimb>("Left Upper Leg");
    public DudeLimb LeftLowerLeg => LeftUpperLeg.GetNode<DudeLimb>("Left Lower Leg");
    public DudeLimb RightUpperLeg => Body.GetNode<DudeLimb>("Right Upper Leg");
    public DudeLimb RightLowerLeg => RightUpperLeg.GetNode<DudeLimb>("Right Lower Leg");

    public BodyPart[] BodyParts =>
    [
        Head,
        Body,
        LeftUpperArm,
        LeftLowerArm,
        RightUpperArm,
        RightLowerArm,
        LeftUpperLeg,
        LeftLowerLeg,
        RightUpperLeg,
        RightLowerLeg,
        Face
    ];

    private bool _drawn = false;
    [Export]
    public bool Drawn
    {
        get => _drawn;
        set
        {
            if (_drawn == value || !HasNode("Head")) return;
            _drawn = value;
            foreach (var part in BodyParts)
            {
                part.Drawn = value;
            }
        }
    }

    private bool _usesCollision = false;
    [Export]
    public bool UsesCollision
    {
        get => _usesCollision;
        set
        {
            if (_usesCollision == value || !HasNode("Head")) return;
            _usesCollision = value;
            foreach (var part in BodyParts)
            {
                if (!part.HasNode("Area") || part.Area == null) continue;
                part.Area.Visible = value;
                part.Area.Monitorable = value;
                part.Area.Monitoring = value;
            }
        }
    }

    private RaceData? _race;
    [Export]
    public RaceData? Race
    {
        get => _race;
        set
        {
            if (_race == value) return;
            _race = value;
            SetRacialCharacteristics();
        }
    }

    #endregion


    #endregion

    #region Methods

    public async Task AnimateDrawing(float speed)
    {
        foreach (var part in BodyParts)
        {
            await part.AnimateDrawing(speed);
        }
        Drawn = true;
    }

    private void SetRacialCharacteristics()
    {
        if (Race == null)
        {
            GD.PrintErr("Null race.");
            return;
        }

        Scale = Race.Scale;
        foreach (var part in BodyParts.Where(p => p != Face))
        {
            part.Color = Race.ModelColor;
            var width = part is DudeBody
                ? Race.LineThickness * 2f
                : Race.LineThickness;
            part.LineWidth = width;
        }
    }

    #endregion
}
