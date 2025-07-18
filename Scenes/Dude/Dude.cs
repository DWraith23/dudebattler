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


	#endregion

	#region Properties, Fields, and Nodes

	public Model Model => GetNode<Model>("Model");


	#endregion
}
