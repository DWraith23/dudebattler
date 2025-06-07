using Godot;
using System;

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

	#endregion

	#region Properties and Fields



	#endregion

	#region Dude Factory

	public override void _Draw()
	{

	}

	private void DrawLimb()
	{

	}



	#endregion
}
