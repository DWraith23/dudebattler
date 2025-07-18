using Godot;
using System;
using DudeBattler.Scenes.Dude;
using System.Collections.Generic;
using System.Threading.Tasks;
using DudeBattler.Scripts;

namespace DudeBattler.Scenes.Menus;

public partial class LoadingScreen : Control
{
    private static PackedScene Scene => GD.Load<PackedScene>("res://Scenes/Menus/loading_screen.tscn");
    public static LoadingScreen CreateInstance(CanvasItem scene)
    {
        var instance = Scene.Instantiate<LoadingScreen>();
        instance.LoadingScene = scene;
        return instance;
    }


    private Dude.Dude LoadingDude => GetNode<Dude.Dude>("CenterContainer/Control/Dude");
    private Label LoadingLabel => GetNode<Label>("Label");
    private ProgressBar LoadingProgess => GetNode<ProgressBar>("Panel/ProgressBar");

    private CanvasItem? LoadingScene { get; set; }

    private Tween? ProgressTween { get; set; }
    private DateTime StartTime { get; set; }



    public override void _Ready()
    {
        StartTime = DateTime.Now;
        AnimateLabel();
        LoadingScene!.Ready += FinishLoad;
        Main.Scenes!.AddChild(LoadingScene);
        LoadingDude.Model.Animator.Play("spin");
    }

    private async void FinishLoad()
    {
        GD.Print("Point C");
        if (ProgressTween != null && ProgressTween.IsRunning())
        {
            ProgressTween.Stop();
            ProgressTween = null;
        }
        if (DateTime.Now.Second == StartTime.Second)
        {
            QueueFree();
            return;
        }
        ProgressTween = CreateTween().SetEase(Tween.EaseType.InOut);
        ProgressTween.TweenProperty(LoadingProgess, "value", 100, 0.33d);
        await ProgressTween.Completed();
        QueueFree();
    }


    private async void AnimateLabel()
    {
        if (!IsInstanceValid(this) || LoadingLabel == null) return;

        var baseText = "Loading";
        List<string> strings =
        [
            "Loading",
            "Loading.",
            "Loading..",
            "Loading...",
        ];

        var index = strings.IndexOf(LoadingLabel.Text);
        if (index == -1) index = 0;
        if (index == 3) LoadingLabel.Text = baseText;
        else LoadingLabel.Text = strings[index + 1];

        await Task.Delay(333);
        AnimateLabel();
    }

    private void AnimateProgressBar()
    {
        var randomTime = Tools.Rand.Next(20, 30) + Tools.Rand.NextDouble();
        var ProgressTween = CreateTween().SetEase(Tween.EaseType.In);
        ProgressTween.TweenProperty(LoadingProgess, "value", 100, randomTime);
    }
}
