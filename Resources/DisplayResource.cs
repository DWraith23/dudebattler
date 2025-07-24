using Godot;

namespace DudeBattler.Resources;

public partial class DisplayResource : Resource
{
    public virtual string? Name { get; set; } = string.Empty;
    public virtual string? Description { get; set; } = string.Empty;
    public virtual Texture2D? Icon { get; set; } = null;
}