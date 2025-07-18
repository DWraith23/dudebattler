using DudeBattler.Resources.Game;
using DudeBattler.Scenes.Menus;
using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DudeBattler.Scenes;

/// <summary>
/// Main scene for the Dude Battler game.  This is what is initially loaded when the game starts.
/// It serves as the entry point for the game and manages the main game loop.
/// All static resources and game logic should be initialized here.
/// </summary>
public partial class Main : Node
{
	#region Properties, Fields, Nodes
	public static CanvasLayer? Scenes { get; set; }
	public static CanvasLayer? Menus { get; set; }
	public static CanvasItem? ActiveScene { get; set; }
	private static List<Control> OpenMenus => [];

	public static GameData? Game { get; set; }
	#endregion


	#region Initialization
	public override void _Ready()
	{
		InitializeGame();
		ChangeScene(MainMenu.CreateInstance());
	}

	private void InitializeGame()
	{

		Scenes = GetNode<CanvasLayer>("Scenes");
		Menus = GetNode<CanvasLayer>("Menus");

	}


	#endregion


	#region Methods

	/// <summary>
	/// Removes the current ActiveScene and loads the passed new scene.
	/// </summary>
	/// <param name="item"></param>
	public static void ChangeScene(CanvasItem item)
	{
		// Cancel if null reference is passed
		if (item == null)
		{
			GD.PrintErr("Invalid scene change, cancelling.");
			return;
		}
		// Cancel if Scenes has not been initialized
		if (Scenes == null)
		{
			GD.PrintErr($"Scenes not initialized, cancelling {item.Name} change.");
			return;
		}

		ActiveScene?.QueueFree();
		ActiveScene = item;
		Scenes.AddChild(LoadingScreen.CreateInstance(item));
	}

	public static void CreateUI(Control menu)
	{
		// Cancel if null reference is passed
		if (menu == null)
		{
			GD.PrintErr("Invalid menu creation, cancelling.");
			return;
		}
		// Cancel if Menus has not been initialized
		if (Menus == null)
		{
			GD.PrintErr($"Menus not initialized, cancelling {menu.Name} creation.");
			return;
		}

		menu.TreeExited += () => OpenMenus.Remove(menu);
		OpenMenus.Add(menu);
		Menus.AddChild(menu);
	}

	public static void SwapActiveUI(Control newUi)
	{
		if (newUi == null) return;
		if (OpenMenus.Count == 0)
		{
			CreateUI(newUi);
			return;
		}

		var active = OpenMenus.Last();
		if (active == newUi) return;
		if (IsInstanceValid(active)) active.QueueFree();
		OpenMenus.Remove(active);
		CreateUI(newUi);
	}

	#endregion

}
