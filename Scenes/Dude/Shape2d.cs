using dudebattler.Resources.Shape2D;
using Godot;
using System;

namespace DudeBattler.Scenes.Dude;

[Tool, GlobalClass]
/// <summary>
/// Shape2d is a Node2D that represents a 2D shape in the game.
/// It is used to draw and manipulate 2D shapes within the game world.
/// </summary>
public partial class Shape2d : Node2D
{
	private static string ScenePath => "res://Scenes/Dude/shape_2d.tscn";
	public static Shape2d CreateInstance()
	{
		var instance = GD.Load<PackedScene>(ScenePath).Instantiate<Shape2d>();
		return instance;
	}

	private Shape2DData? _shape;
	[Export]
	public Shape2DData? Shape
	{
		get => _shape;
		set
		{
			_shape = value;
			QueueRedraw();
			if (_shape != null) _shape.Changed += QueueRedraw;
		}
	}

	public override void _Draw()
	{
		if (Shape is LineData line)
		{
			DrawLine(line.Start, line.End, line.Color, line.Width);
		}
		else if (Shape is CircleData circle)
		{
			DrawCircle(circle.Position,
				circle.Radius,
				circle.Color,
				circle.Filled,
				circle.Width,
				circle.Antialiased
				);
		}
	}

}
