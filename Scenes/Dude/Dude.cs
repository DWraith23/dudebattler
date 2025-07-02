using DudeBattler.Scripts;
using Godot;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Dude;

[Tool, GlobalClass]
public partial class Dude : Node2D
{

	#region Generation
	private static string ScenePath => "res://Scenes/Dude/dude.tscn";
	public static Dude CreateInstance()
	{
		var instance = GD.Load<PackedScene>(ScenePath).Instantiate<Dude>();

		return instance;
	}

	public override async void _Ready()
	{
		foreach (var part in BodyParts)
		{
			await part.AnimateDrawing(0.2f);
		}
	}

	#endregion

	#region Signals


	#endregion

	#region Properties and Fields

	public AnimationPlayer Animator => GetNode<AnimationPlayer>("Animator");

	#region Body Parts
	public DudeHead Head => GetNode<DudeHead>("Model/Head");
	public DudeBody Body => GetNode<DudeBody>("Model/Head/Body");
	public DudeLimb LeftUpperArm => GetNode<DudeLimb>("Model/Head/Body/Left Upper Arm");
	public DudeLimb LeftLowerArmr => GetNode<DudeLimb>("Model/Head/Body/Left Upper Arm/Left Lower Arm");
	public DudeLimb RightUpperArm => GetNode<DudeLimb>("Model/Head/Body/Right Upper Arm");
	public DudeLimb RightLowerArm => GetNode<DudeLimb>("Model/Head/Body/Right Upper Arm/Right Lower Arm");
	public DudeLimb LeftUpperLeg => GetNode<DudeLimb>("Model/Head/Body/Left Upper Leg");
	public DudeLimb LeftLowerLeg => GetNode<DudeLimb>("Model/Head/Body/Left Upper Leg/Left Lower Leg");
	public DudeLimb RightUpperLeg => GetNode<DudeLimb>("Model/Head/Body/Right Upper Leg");
	public DudeLimb RightLowerLeg => GetNode<DudeLimb>("Model/Head/Body/Right Upper Leg/Right Lower Leg");

	public BodyPart[] BodyParts =>
	[
		Head,
		Body,
		LeftUpperArm,
		LeftLowerArmr,
		RightUpperArm,
		RightLowerArm,
		LeftUpperLeg,
		LeftLowerLeg,
		RightUpperLeg,
		RightLowerLeg
	];
	#endregion


	#endregion
}
