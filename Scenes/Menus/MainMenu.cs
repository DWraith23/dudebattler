using DudeBattler.Scenes.BaseScenes;
using Godot;
using System;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Menus;

public partial class MainMenu : MenuBase
{
    private static PackedScene Scene => GD.Load<PackedScene>("res://Scenes/Menus/main_menu.tscn");
    public static MainMenu CreateInstance() => Scene.Instantiate<MainMenu>();

    private PanelContainer MainPanel => Centerer.GetNode<PanelContainer>("Panel");
    private VBoxContainer Contents => MainPanel.GetNode<VBoxContainer>("Contents");
    private Button NewGame => Contents.GetNode<Button>("New Game");
    private Button Continue => Contents.GetNode<Button>("Continue");
    private Button LoadGame => Contents.GetNode<Button>("Load Game");
    private Button Options => Contents.GetNode<Button>("Options");
    private Button Credits => Contents.GetNode<Button>("Credits");
    private Button ExitGame => Contents.GetNode<Button>("Exit Game");


    public override void _Ready()
    {
        ConnectSignals();
    }

    private void ConnectSignals()
    {

    }


}
