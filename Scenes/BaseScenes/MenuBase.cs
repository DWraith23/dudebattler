using Godot;
using System;

namespace DudeBattler.Scenes.BaseScenes;

public partial class MenuBase : Control
{
    public Panel Background => GetNode<Panel>("Background");
    public CenterContainer Centerer => GetNode<CenterContainer>("Centerer");

}
