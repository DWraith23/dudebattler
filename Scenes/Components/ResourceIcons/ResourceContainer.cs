using System;
using System.Linq;
using DudeBattler.Resources;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Scenes.Components.ResourceIcons;

/// <summary>
/// A container for holding and managing ResourceIcons.
/// </summary>
[Tool]
public partial class ResourceContainer : HFlowContainer
{
    // Signals for button interactions
    [Signal] public delegate void ButtonMousedOverEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonMousedAwayEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonPressedEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonRightClickedEventHandler(ResourceIcon resource);

    #region Properties

    private IconAttributes? _defaultAttributes;
    private int _buttonSize = 64;
    private int _columns = 1;
    private int _spacing = 10;
    private Color _normalBorderColor = Colors.Black;
    private Color _disabledBorderColor = Colors.DarkGray;
    private Color _hoverBorderColor = Colors.Yellow;
    private Color _pressedBorderColor = Colors.Green;
    private bool _toggleable = false;
    private bool _multiToggle = false;
    private ResourceIcon.ButtonPressType _leftMousePressType = ResourceIcon.ButtonPressType.Released;
    private ResourceIcon.ButtonPressType _rightMousePressType = ResourceIcon.ButtonPressType.Released;

    [Export] public IconAttributes? DefaultAttributes
    {
        get => _defaultAttributes;
        set
        {
            if (_defaultAttributes == value) return;
            _defaultAttributes = value;
            if (value == null)
            {
                Resources
                    .ToList()
                    .ForEach(resource => resource.Attributes = null);
            }
            else
            {
                Resources
                    .ToList()
                    .ForEach(resource => resource.Attributes = value.Duplicate<IconAttributes>(true));
            }
        }
    }


    [ExportGroup("Sizing")]
    /// <summary>
    /// Size of the buttons in the container.
    /// </summary>
    [Export]
    public int ButtonSize
    {
        get => _buttonSize;
        set
        {
            _buttonSize = Math.Max(value, 0);
            Resources.ToList().ForEach(child => child.ButtonSize = _buttonSize);
            UpdateContainerSize();
        }
    }

    /// <summary>
    /// Number of columns in the container.
    /// </summary>
    [Export]
    public int Columns
    {
        get => _columns;
        set
        {
            _columns = Math.Max(value, 1);
            UpdateContainerSize();
        }
    }

    /// <summary>
    /// Spacing between buttons in the container.
    /// </summary>
    [Export]
    public int Spacing
    {
        get => _spacing;
        set
        {
            _spacing = Math.Max(value, 0);
            UpdateContainerSize();
            AddThemeConstantOverride("h_separation", _spacing);
            AddThemeConstantOverride("v_separation", _spacing);
        }
    }

    [ExportGroup("Colors")]
    // Color properties for different button states
    [Export] public Color NormalBorderColor
    {
        get => _normalBorderColor;
        set
        {
            _normalBorderColor = value;
            Resources.ToList().ForEach(child => child.NormalBorderColor = value);
        }
    }

    [Export] public Color DisabledBorderColor
    {
        get => _disabledBorderColor;
        set
        {
            _disabledBorderColor = value;
            Resources.ToList().ForEach(child => child.DisabledBorderColor = value);
        }
    }

    [Export] public Color HoverBorderColor
    {
        get => _hoverBorderColor;
        set
        {
            _hoverBorderColor = value;
            Resources.ToList().ForEach(child => child.HoverBorderColor = value);
        }
    }

    [Export] public Color PressedBorderColor
    {
        get => _pressedBorderColor;
        set
        {
            _pressedBorderColor = value;
            Resources.ToList().ForEach(child => child.PressedBorderColor = value);
        }
    }

    [ExportGroup("Control")]
    /// <summary>
    /// Determines if buttons can be toggled.
    /// </summary>
    [Export]
    public bool Toggleable
    {
        get => _toggleable;
        set
        {
            _toggleable = value;
            Resources.ToList().ForEach(child =>
            {
                child.IsToggled = false;
                child.Toggleable = value;
            });
        }
    }

    /// <summary>
    /// Determines if multiple buttons can be toggled simultaneously.
    /// </summary>
    [Export]
    public bool MultiToggle
    {
        get => _multiToggle;
        set => _multiToggle = value;
    }

    [Export]
    public ResourceIcon.ButtonPressType LeftMousePressType
    {
        get => _leftMousePressType;
        set
        {
            if (_leftMousePressType == value) return;
            _leftMousePressType = value;
            Resources.ToList().ForEach(child => child.LeftMousePressType = value);
        }
    }

    [Export]
    public ResourceIcon.ButtonPressType RightMousePressType
    {
        get => _rightMousePressType;
        set
        {
            if (_rightMousePressType == value) return;
            _rightMousePressType = value;
            Resources.ToList().ForEach(child => child.RightMousePressType = value);
        }
    }


    #endregion

    /// <summary>
    /// Collection of ResourceIcons in the container.
    /// </summary>
    public Godot.Collections.Array<ResourceIcon> Resources =>
        [.. GetChildren().OfType<ResourceIcon>()];

    /// <summary>
    /// Number of ResourceIcons in the container.
    /// </summary>
    public int ResourceCount => GetChildren().OfType<ResourceIcon>().Count();

    /// <summary>
    /// Collection of toggled ResourceIcons.
    /// </summary>
    public Godot.Collections.Array<ResourceIcon> ToggledResources =>
        [.. Resources.Where(resource => resource.IsToggled)];

    public ResourceIcon? LastResource => GetChildren().OfType<ResourceIcon>().LastOrDefault();

    public override void _Ready()
    {
        ChildEnteredTree += OnIconAdded;
        UpdateContainerSize();
    }

    #region Events

    private void OnIconAdded(Node node)
    {
        if (node is not ResourceIcon icon || icon.Added) return;
        ConfigureIcon(icon);
        icon.Added = true;
    }

    private void OnIconRemoved(Node node)
    {
        if (node is not ResourceIcon icon || !node.IsValid()) return;
        DisconnectIconSignals(icon);
    }

    private void OnNodeToggled(ResourceIcon node)
    {
        if (!MultiToggle)
        {
            Resources.Where(resource => resource != node)
                .ToList().ForEach(resource => resource.IsToggled = false);
        }
    }

    private void EmitMouseOverSignal(ResourceIcon resource) =>
        this.EmitSignalLogged(SignalName.ButtonMousedOver, resource);
    
    private void EmitMouseAwaySignal(ResourceIcon resource) =>
        this.EmitSignalLogged(SignalName.ButtonMousedAway, resource);

    private void EmitButtonPressedSignal(ResourceIcon resource) =>
        this.EmitSignalLogged(SignalName.ButtonPressed, resource);

    private void EmitRightClickedSignal(ResourceIcon resource) =>
        this.EmitSignalLogged(SignalName.ButtonRightClicked, resource);

    #endregion

    #region Interaction

    /// <summary>
    /// Adds a new resource to the container.
    /// </summary>
    public void AddResource(DisplayResource? resource)
    {
        if (resource == null) return;
        AddChild(ResourceIcon.CreateInstance(resource));
    }

    public void AddRange(DisplayResource[]? resources)
    {
        if (resources == null) return;
        foreach (var resource in resources)
        {
            AddResource(resource);
        }
    }

    public void AddResource(DisplayResource? resource, Color borderColor)
    {
        if (resource == null) return;
        var icon = ResourceIcon.CreateInstance(resource);
        AddChild(icon);
        icon.NormalBorderColor = borderColor;
        icon.CustomColor = true;
    }

    /// <summary>
    /// Removes a resource from the container.
    /// </summary>
    public void RemoveResource(DisplayResource resource)
    {
        Resources.Where(icon => icon.Resource == resource).ToList()
            .ForEach(RemoveChild);
    }

    public void RemoveResource(ResourceIcon icon)
    {
        RemoveChild(icon);
    }

    /// <summary>
    /// Clears all resources from the container.
    /// </summary>
    public void Clear() => this.FreeChildren();

    /// <summary>
    /// Checks if a resource exists in the container.
    /// </summary>
    public bool HasResource(DisplayResource resource) =>
        Resources.Any(icon => icon.Resource == resource);
        
    #endregion

    #region Helper Methods

    private void UpdateContainerSize()
    {
        var per = ButtonSize + 16 + _spacing;
        CustomMinimumSize = new Vector2(per * _columns - (_spacing - 1), CustomMinimumSize.Y);
    }

    private void ConfigureIcon(ResourceIcon icon)
    {
        icon.ButtonSize = ButtonSize;
        if (!icon.CustomColor) icon.NormalBorderColor = NormalBorderColor;
        icon.DisabledBorderColor = DisabledBorderColor;
        icon.HoverBorderColor = HoverBorderColor;
        icon.PressedBorderColor = PressedBorderColor;
        icon.Toggleable = Toggleable;
        icon.LeftMousePressType = LeftMousePressType;
        icon.RightMousePressType = RightMousePressType;
        if (icon.Attributes == null && DefaultAttributes != null)
        {
            icon.Attributes = DefaultAttributes.Duplicate<IconAttributes>(true);
        }
        ConnectIconSignals(icon);
    }

    private void ConnectIconSignals(ResourceIcon icon)
    {
        icon.ButtonMousedOver += EmitMouseOverSignal;
        icon.ButtonMousedAway += EmitMouseAwaySignal;
        icon.ButtonPressed += EmitButtonPressedSignal;
        icon.ButtonRightClicked += EmitRightClickedSignal;
        icon.ButtonToggled += OnNodeToggled;
    }

    private void DisconnectIconSignals(ResourceIcon icon)
    {
        icon.ButtonMousedOver -= EmitMouseOverSignal;
        icon.ButtonMousedAway -= EmitMouseAwaySignal;
        icon.ButtonPressed -= EmitButtonPressedSignal;
        icon.ButtonRightClicked -= EmitRightClickedSignal;
        icon.ButtonToggled -= OnNodeToggled;
    }

    #endregion
}
