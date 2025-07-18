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

	private bool _loadsDrawn = false;
	[Export]
	public bool LoadsDrawn
	{
		get => _loadsDrawn;
		set
		{
			if (_loadsDrawn == value) return;
			_loadsDrawn = value;
			Model.Drawn = value;
		}
	}
	public Model Model => GetNode<Model>("Model");


	#endregion
}
