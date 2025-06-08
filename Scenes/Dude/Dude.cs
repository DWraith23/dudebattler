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
		await StartupAnimations();
	}

	private async Task StartupAnimations()
	{
		Task[] tasks = [];
		Animator.Play("stand");
		Animator.Play("ready");
		await Animator.ToSignal(Animator, AnimationMixer.SignalName.AnimationFinished);
	}

	#endregion

	#region Signals


	#endregion

	#region Properties and Fields

	public AnimationPlayer Animator => GetNode<AnimationPlayer>("Animator");

	#region Body Parts
	public Shape2d Head => GetNode<Shape2d>("Parts/Neck/Head");
	public Shape2d Neck => GetNode<Shape2d>("Parts/Neck");
	public Shape2d Body => GetNode<Shape2d>("Parts/Body");
	// Arms
	public Shape2d Arms => GetNode<Shape2d>("Parts/Body/Arms");
	public Shape2d LeftArm => GetNode<Shape2d>("Parts/Body/Arms/Left Arm");
	public Shape2d LeftUpperArm => GetNode<Shape2d>("Parts/Body/Arms/Left Arm/Upper Arm");
	public Shape2d LeftLowerArm => GetNode<Shape2d>("Parts/Body/Arms/Left Arm/Lower Arm");
	public Shape2d LeftHand => GetNode<Shape2d>("Parts/Body/Arms/Left Arm/Lower Arm/Hand");
	public Shape2d RightArm => GetNode<Shape2d>("Parts/Body/Arms/Right Arm");
	public Shape2d RightUpperArm => GetNode<Shape2d>("Parts/Body/Arms/Right Arm/Upper Arm");
	public Shape2d RightLowerArm => GetNode<Shape2d>("Parts/Body/Arms/Right Arm/Lower Arm");
	public Shape2d RightHand => GetNode<Shape2d>("Parts/Body/Arms/Right Arm/Lower Arm/Hand");
	// Legs
	public Shape2d Legs => GetNode<Shape2d>("Parts/Body/Legs");
	public Shape2d LeftLeg => GetNode<Shape2d>("Parts/Body/Legs/Left Leg");
	public Shape2d LeftUpperLeg => GetNode<Shape2d>("Parts/Body/Legs/Left Leg/Upper Leg");
	public Shape2d LeftLowerLeg => GetNode<Shape2d>("Parts/Body/Legs/Left Leg/Lower Leg");
	public Shape2d LeftFoot => GetNode<Shape2d>("Parts/Body/Legs/Left Leg/Lower Leg/Foot");
	public Shape2d RightLeg => GetNode<Shape2d>("Parts/Body/Legs/Right Leg");
	public Shape2d RightUpperLeg => GetNode<Shape2d>("Parts/Body/Legs/Right Leg/Upper Leg");
	public Shape2d RightLowerLeg => GetNode<Shape2d>("Parts/Body/Legs/Right Leg/Lower Leg");
	public Shape2d RightFoot => GetNode<Shape2d>("Parts/Body/Legs/Right Leg/Lower Leg/Foot");

	public Shape2d[] BodyParts =>
	[
		Head,
		Neck,
		Body,
		Arms,
		LeftArm,
		LeftUpperArm,
		LeftLowerArm,
		LeftHand,
		RightArm,
		RightUpperArm,
		RightLowerArm,
		RightHand,
		Legs,
		LeftLeg,
		LeftUpperLeg,
		LeftLowerLeg,
		LeftFoot,
		RightLeg,
		RightUpperLeg,
		RightLowerLeg,
		RightFoot
	];
	#endregion


	#endregion
}
