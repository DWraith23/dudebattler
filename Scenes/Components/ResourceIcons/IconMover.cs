using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Scenes.Components.ResourceIcons;

public partial class IconMover : CenterContainer
{
	private static PackedScene Scene => GD.Load<PackedScene>("res://Scenes/Components/ResourceIcons/icon_mover.tscn");
	public static IconMover CreateInstance(ResourceIcon icon)
	{
		var instance = Scene.Instantiate<IconMover>();
		instance.Icon = icon;
		return instance;
	}

	[Signal] public delegate void MouseReleasedEventHandler(ResourceIcon icon, Vector2 coordinates);

	public ResourceIcon? Icon { get; set; }

    public override void _Ready()
    {
		if (Icon == null)
		{
			QueueFree();
			return;
		}
		this.AdoptChild(Icon);
    }

    public override void _Process(double delta)
    {
		GlobalPosition = GetGlobalMousePosition();
    }



	public override void _Input(InputEvent @event)
	{
		if (!@event.IsMouseInputType(true, false)) return;
		if (Icon == null)
		{
			QueueFree();
			return;
		}
		var coord = GetGlobalMousePosition();
		this.EmitSignalLogged(SignalName.MouseReleased, Icon, coord);
		QueueFree();
	}

}
