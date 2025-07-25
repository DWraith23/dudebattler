using Godot;
using System;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Dude;

[GlobalClass, Tool]
public abstract partial class BodyPart : Node2D
{

    public virtual Area2D? Area { get; }
    public virtual CollisionShape2D? Collider { get; }
    public virtual Vector2 ConnectionPoint { get; }
    protected Shape2D? Shape => Collider?.Shape;
    public virtual Color Color { get; set; }
    public virtual float LineWidth { get; set; }

    protected abstract void GenerateNodes();

    private bool _drawn = false;
    [Export]
    public bool Drawn
    {
        get => _drawn;
        set
        {
            _drawn = value;
            QueueRedraw();
        }
    }
    protected bool IsDrawing { get; set; } = false;
    public abstract Task AnimateDrawing(float speed);
    public virtual void Reposition()
    {
        return;
    }
}
