using DudeBattler.Resources.Shape2D;
using Godot;
using System;

namespace DudeBattler.Scenes.Components;

[Tool, GlobalClass]
/// <summary>
/// Shape2d is a Node2D that represents a 2D shape in the game.
/// It is used to draw and manipulate 2D shapes within the game world.
/// </summary>
public partial class Shape2d : Node2D
{
	private static string ScenePath => "res://Scenes/Components/shape_2d.tscn";
	public static Shape2d CreateInstance()
	{
		var instance = GD.Load<PackedScene>(ScenePath).Instantiate<Shape2d>();
		return instance;
	}

	[Signal] public delegate void AreaCollisionEnteredEventHandler(Area2D area);
	[Signal] public delegate void AreaCollisionExitedEventHandler(Area2D area);

	private Shape2DData? _shape;
	[Export]
	public Shape2DData? Shape
	{
		get => _shape;
		set
		{
			_shape = value;
			QueueRedraw();
			if (_shape != null)
			{
				_shape.Changed += QueueRedraw;
				_shape.CollisionChanged += CollisionChanged;
			}
		}
	}

	public Node2D? Collider { get; set; }
	public Area2D? Area { get; set; }

	public override void _Ready()
	{
		if (Shape != null)
		{
			QueueRedraw();
			if (Collider == null)
			{
				CollisionChanged(Shape.HasCollision);
			}
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
		else if (Shape is ArcData arc)
		{
			var start = Mathf.DegToRad(arc.StartAngle);
			var end = Mathf.DegToRad(arc.EndAngle);
			DrawArc(arc.Position,
				arc.Radius,
				start,
				end,
				arc.PointCount,
				arc.Color,
				arc.Width,
				arc.Antialiased
				);
		}
	}

	private void CollisionChanged(bool hasCollision)
	{
		if (Collider != null && Collider.IsInsideTree())
			Collider?.QueueFree();
		if (Area != null && Area.IsInsideTree())
			Area?.QueueFree();
		
		Collider = null;
		Area = null;

		if (hasCollision)
		{
			if (Shape is LineData line)
			{
				var x = line.End.X != 0 ? Math.Abs(line.End.X) : line.Width;
				var y = line.End.Y != 0 ? Math.Abs(line.End.Y) : line.Width;
				var collider = new CollisionShape2D
				{
					Shape = new RectangleShape2D
					{
						Size = new(x, y),
					},
					Position = (line.Start + line.End) / 2,
				};
				Collider = collider;
			}
			else if (Shape is CircleData circle)
			{
				var collider = new CollisionShape2D
				{
					Shape = new CircleShape2D
					{
						Radius = circle.Radius
					}
				};
				Collider = collider;
			}

			if (Collider != null)
			{
				Area = new();
				AddChild(Area, false, InternalMode.Back);
				Area.AddChild(Collider, false, InternalMode.Back);
				Area.AreaEntered += (area) => EmitSignal(SignalName.AreaCollisionEntered, area);
				Area.AreaExited += (area) => EmitSignal(SignalName.AreaCollisionExited, area);
			}
		}
	}

}
