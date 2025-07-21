using System;
using DudeBattler.Resources;
using DudeBattler.Scripts;
using Godot;
using static DudeBattler.Scenes.Components.ResourceIcons.SubIcon;

namespace DudeBattler.Scenes.Components.ResourceIcons;

/// <summary>
/// A scene that specializes in displaying Resources with names and icons. Essentially turns a DisplayResource into a button.
/// </summary>
[GlobalClass, Tool]
public partial class ResourceIcon : PanelContainer
{
    /// <summary>
    /// Controls how the ButtonPressed and ButtonRightClicked signals are emitted.
    /// </summary>
    public enum ButtonPressType
    {
        /// <summary>
        /// Emits the signal when the button is pressed.
        /// </summary>
        Pressed,
        /// <summary>
        /// Emits the signal when the button is released.
        /// </summary>
        Released,
        /// <summary>
        /// Emits the signal when the button is pressed and released.
        /// </summary>
        Both,
        None,
    }

    // Signals for button interactions
    [Signal] public delegate void ButtonMousedOverEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonMousedAwayEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonPressedEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonRightClickedEventHandler(ResourceIcon resource);
    [Signal] public delegate void ButtonToggledEventHandler(ResourceIcon node);

    #region Properties
    private IconAttributes? _attributes;
    private int _buttonSize = 64;
    private Color _normalBorderColor = Colors.Black;
    private Color _disabledBorderColor = Colors.DarkGray;
    private Color _hoverBorderColor = Colors.Yellow;
    private Color _pressedBorderColor = Colors.Green;
    private bool _toggleable = false;
    private bool _enabled = true;
    private bool _isHovered = false;
    private bool _isToggled = false;

    [Export] public IconAttributes? Attributes
    {
        get => _attributes;
        set
        {
            if (_attributes == value) return;
            _attributes = value;

            UpdateSubIcons();

            if (_attributes != null)
            {
                _attributes.Changed += UpdateSubIcons;
            }
        }
    }

    /// <summary>
    /// Size of the button.
    /// </summary>
    [Export]
    public int ButtonSize
    {
        get => _buttonSize;
        set
        {
            _buttonSize = Math.Max(value, 0);
            UpdateButtonSize();
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
            UpdateBorderColor();
        }
    }

    [Export] public Color DisabledBorderColor
    {
        get => _disabledBorderColor;
        set
        {
            _disabledBorderColor = value;
            UpdateBorderColor();
        }
    }

    [Export] public Color HoverBorderColor
    {
        get => _hoverBorderColor;
        set
        {
            _hoverBorderColor = value;
            UpdateBorderColor();
        }
    }

    [Export] public Color PressedBorderColor
    {
        get => _pressedBorderColor;
        set
        {
            _pressedBorderColor = value;
            UpdateBorderColor();
        }
    }

    [ExportGroup("Control")]
    /// <summary>
    /// Determines if the button can be toggled.
    /// </summary>
    [Export]
    public bool Toggleable
    {
        get => _toggleable;
        set
        {
            _toggleable = value;
            if (!value)
            {
                IsToggled = false;
            }
        }
    }

    /// <summary>
    /// Determines if the button is enabled.
    /// </summary>
    [Export]
    public bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            UpdateBorderColor();
        }
    }

    /// <summary>
    /// Determines if the button is currently being hovered over.
    /// </summary>
    [Export]
    public bool IsHovered
    {
        get => _isHovered;
        set
        {
            _isHovered = value;
            UpdateBorderColor();
        }
    }

    /// <summary>
    /// Determines if the button is currently toggled.
    /// </summary>
    [Export]
    public bool IsToggled
    {
        get => _isToggled;
        set
        {
            if (!Enabled || !Toggleable)
            {
                _isToggled = false;
                return;
            }
            _isToggled = value;
            UpdateBorderColor();
            if (value)
            {
                this.EmitSignalLogged(SignalName.ButtonToggled, this);
            }
        }
    }

    [Export] public ButtonPressType LeftMousePressType { get; set; } = ButtonPressType.Pressed;
    [Export] public ButtonPressType RightMousePressType { get; set; } = ButtonPressType.None;

    public bool Added { get; set; } = false;
    public bool CustomColor { get; set; } = false;

    public int Width => (int)Size.X;

    #endregion

    #region Nodes, Resources, and Setup
    private TextureButton Button => GetNode<TextureButton>("Button");
    private Control AttributeContainer => GetNode<Control>("Attribute Container");
    private DisplayResource? _resource;

    [ExportGroup("Testing")]
    /// <summary>
    /// The DisplayResource associated with this ResourceIcon.
    /// </summary>
    [Export]
    public DisplayResource? Resource
    {
        get => _resource;
        set
        {
            if (_resource != null)
            {
                _resource.Changed -= SetResource;
            }
            _resource = value;
            SetResource();
            if (_resource != null)
            {
                _resource.Changed += SetResource;
            }
        }
    }

    public override void _Ready()
    {
        ConnectSignals();
    }

    private void ConnectSignals()
    {
        Button.GuiInput += OnGuiInput;
        Button.MouseEntered += OnMouseEntered;
        Button.MouseExited += OnMouseExited;
        Button.FocusEntered += OnFocusEntered;
        Button.FocusExited += OnFocusExited;
    }

    private void UpdateSubIcons()
    {
        AttributeContainer.FreeChildren();

        if (Attributes == null)
        {
            return;
        }

        if (Attributes.UpperLeft != null)
        {
            var sub = Attributes.UpperLeft.GenerateSubIcon(Quadrant.UpperLeft, ButtonSize);
            if (sub != null)
            {
                sub.Visible = Attributes.UpperLeft.Visible;
                AttributeContainer.AddChild(sub);
            }
        }

        if (Attributes.UpperRight != null)
        {
            var sub = Attributes.UpperRight.GenerateSubIcon(Quadrant.UpperRight, ButtonSize);
            if (sub != null)
            {
                sub.Visible = Attributes.UpperRight.Visible;
                AttributeContainer.AddChild(sub);
            }
        }

        if (Attributes.LowerLeft != null)
        {
            var sub = Attributes.LowerLeft.GenerateSubIcon(Quadrant.LowerLeft, ButtonSize);
            if (sub != null)
            {
                sub.Visible = Attributes.LowerLeft.Visible;
                AttributeContainer.AddChild(sub);  
            } 
        }

        if (Attributes.LowerRight != null)
        {
            var sub = Attributes.LowerRight.GenerateSubIcon(Quadrant.LowerRight, ButtonSize);
            if (sub != null)
            {
                sub.Visible = Attributes.LowerRight.Visible;
                AttributeContainer.AddChild(sub);
            }
        }

        if (Attributes.OverlayText != string.Empty)
        {
            var label = new Label()
            {
                Text = Attributes.OverlayText,
                SizeFlagsVertical = SizeFlags.ShrinkCenter,
                SizeFlagsHorizontal = SizeFlags.ShrinkCenter,
                MouseFilter = MouseFilterEnum.Ignore,
                HorizontalAlignment = HorizontalAlignment.Center,
            };
            label.SetAnchorsAndOffsetsPreset(LayoutPreset.Center);
            AttributeContainer.AddChild(label);
            while (label.Size.X > ButtonSize && label.GetThemeFontSize("font_size") > 10)
            {
                var size = label.GetThemeFontSize("font_size");
                label.AddThemeFontSizeOverride("font_size", size - 1);
            }
        }
    }

    #endregion

    #region Events

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept"))
        {
            OnButtonPressed();
            GetViewport().SetInputAsHandled();
        }
    }

    private void OnGuiInput(InputEvent @event)
    {
        if (@event is not InputEventMouseButton mouse) return;
        if (mouse.ButtonIndex == MouseButton.Right)
        {
            HandleMouseClick(mouse, RightMousePressType, OnRightClick);
        }
        else if (mouse.ButtonIndex == MouseButton.Left)
        {
            HandleMouseClick(mouse, LeftMousePressType, OnButtonPressed);
        }
    }

    private void HandleMouseClick(InputEventMouseButton mouse, ButtonPressType pressType, Action clickAction)
    {
        bool shouldTrigger = (mouse.Pressed && pressType == ButtonPressType.Pressed) ||
                             (!mouse.Pressed && pressType == ButtonPressType.Released);

        if (pressType == ButtonPressType.Both) shouldTrigger = true;
        if (pressType == ButtonPressType.None) shouldTrigger = false;

        if (shouldTrigger)
        {
            clickAction();
            GetViewport()?.SetInputAsHandled();
        }
    }

    private void OnMouseEntered()
    {
        if (Resource == null) return;
        IsHovered = true;
        this.EmitSignalLogged(SignalName.ButtonMousedOver, this);
    }

    private void OnMouseExited()
    {
        if (Resource == null) return;
        IsHovered = false;
        this.EmitSignalLogged(SignalName.ButtonMousedAway, this);
    }

    private void OnButtonPressed()
    {
        if (!Enabled || Resource == null) return;
        if (Toggleable)
        {
            IsToggled = !IsToggled;
        }
        this.EmitSignalLogged(SignalName.ButtonPressed, this);
    }

    private void OnFocusEntered() => OnMouseEntered();

    private void OnFocusExited() => OnMouseExited();

    private void OnRightClick()
    {
        if (Resource == null) return;
        this.EmitSignalLogged(SignalName.ButtonRightClicked, this);
    }

    #endregion

    #region Controls

    private void UpdateBorderColor()
    {
        Color color = Enabled ? NormalBorderColor : DisabledBorderColor;
        if (IsHovered) color = HoverBorderColor;
        if (IsToggled) color = PressedBorderColor;

        var stylebox = GetThemeStylebox("panel").Duplicate<StyleBoxFlat>(true);
        stylebox!.BorderColor = color;
        AddThemeStyleboxOverride("panel", stylebox);
    }

    private void UpdateButtonSize()
    {
        if (Button != null)
        {
            Button.CustomMinimumSize = new Vector2(_buttonSize, _buttonSize);
        }
    }

    private void SetResource()
    {
        if (Button == null) return;

        if (Resource == null)
        {
            Button.TextureNormal = null;
            Button.TooltipText = "";
        }
        else
        {
            Button.TextureNormal = Resource.Icon;
            // Button.TooltipText = $"{Resource.Name}: {Resource.Description}";
        }
    }

    #endregion

    #region Attribute Manipulation

    public void ChangeTextOverlay(string text)
    {
        Attributes ??= new();
        Attributes.OverlayText = text;
    }

    public void ChangeVisibility(Quadrant quadrant, bool visible)
    {
        if (Attributes == null) return;
        var sub = Attributes.SubIcons[quadrant];
        if (sub != null) sub.Visible = visible;
    }

    public void ChangeText(Quadrant quadrant, string text)
    {
        if (Attributes == null || Attributes.SubIcons[quadrant] is not SubIconText subText) return;

        subText.Text = text;
    }

    public void ChangeColor(Quadrant quadrant, Color color)
    {
        if (Attributes == null) return;
        if (Attributes.SubIcons[quadrant] is SubIconImage image)
        {
            image.Modulation = color;
        }
        else if (Attributes.SubIcons[quadrant] is SubIconText text)
        {
            text.Color = color;
        }
    }

    public void ChangeOutline(Quadrant quadrant, Color color)
    {
        if (Attributes == null || Attributes.SubIcons[quadrant] is not SubIconText text) return;

        text.OutlineColor = color;
    }

    #endregion

    /// <summary>
    /// Creates a new instance of ResourceIcon with the specified resource.
    /// </summary>
    public static ResourceIcon CreateInstance(DisplayResource resource)
    {
        var instance = GD
            .Load<PackedScene>("res://Scenes/Components/ResourceIcons/resource_icon.tscn")
            .Instantiate<ResourceIcon>();
        instance.Resource = resource;
        return instance;
    }

    /// <summary>
    /// Creates a new instance of ResourceIcon with the specified resource and size.
    /// </summary>
    public static ResourceIcon CreateInstance(DisplayResource resource, int size)
    {
        var instance = CreateInstance(resource);
        instance.ButtonSize = size;
        return instance;
    }
}
