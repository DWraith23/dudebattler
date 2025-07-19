using DudeBattler.Scripts;
using Godot;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DudeBattler.Scenes.Dude;

[GlobalClass, Tool]
public partial class Face : BodyPart
{

    public enum FaceOption
    {
        None,
        Smiling,
        Frowning,
        Neutral,
        // Evil,
        EyesOnly,
    }

    private Dictionary<FaceOption, Action> FaceDrawings => new()
    {
        { FaceOption.Smiling, DrawSmiling },
        { FaceOption.Frowning, DrawFrowning },
        { FaceOption.Neutral, DrawNeutral },
        { FaceOption.EyesOnly, DrawEyesOnly },
    };

    private Marker2D LeftEye => GetNode<Marker2D>("Left Eye");
    private Marker2D RightEye => GetNode<Marker2D>("Right Eye");
    private Marker2D MouthStart => GetNode<Marker2D>("Mouth Start");
    private Marker2D MouthEnd => GetNode<Marker2D>("Mouth End");
    
    private FaceOption _faceType = FaceOption.None;
    [Export]
    public FaceOption FaceType
    {
        get => _faceType;
        set
        {
            if (_faceType == value) return;
            _faceType = value;
            QueueRedraw();
        }
    }

    private float _eyeSize = 5f;
    [Export]
    public float EyeSize
    {
        get => _eyeSize;
        set
        {
            if (_eyeSize == value) return;
            _eyeSize = value;
            QueueRedraw();
        }
    }

    private Color _eyeColor = Colors.Black;
    [Export]
    public Color EyeColor
    {
        get => _eyeColor;
        set
        {
            if (_eyeColor == value) return;
            _eyeColor = value;
            QueueRedraw();
        }
    }

    private float _currentAngle = 0f;
    private float CurrentAngle
    {
        get => _currentAngle;
        set
        {
            if (_currentAngle == value) return;
            _currentAngle = value;
            QueueRedraw();
        }
    }

    private float _currentLength = 0f;
    private float CurrentLength
    {
        get => _currentLength;
        set
        {
            if (_currentLength == value) return;
            _currentLength = value;
            QueueRedraw();
        }
    }

    private float _currentSize = 0f;
    private float CurrentSize
    {
        get => _currentSize;
        set
        {
            if (_currentSize == value) return;
            _currentSize = value;
            QueueRedraw();
        }
    }



    public override void _Draw()
    {
        if (!Drawn) return;
        if (FaceDrawings.TryGetValue(_faceType, out Action? value))
        {
            value();
        }
    }

    private void DrawSmiling()
    {
        var size = IsDrawing ? CurrentSize : EyeSize;
        DrawCircle(LeftEye.Position, size, EyeColor, true);
        DrawCircle(RightEye.Position, size, EyeColor, true);

        var mouthCenterX = (MouthStart.Position.X + MouthEnd.Position.X) / 2;
        var mouthCenterY = MouthStart.Position.Y;
        var mouthCenter = new Vector2(mouthCenterX, mouthCenterY);
        var startAngle = 0f;
        var endAngle = IsDrawing ? Mathf.DegToRad(CurrentAngle) : Mathf.DegToRad(180f);

        DrawArc(mouthCenter, MouthEnd.Position.X, endAngle, startAngle, 18, Colors.Black, 10f);
    }

    private void DrawFrowning()
    {
        var size = IsDrawing ? CurrentSize : EyeSize;
        DrawCircle(LeftEye.Position, size, EyeColor, true);
        DrawCircle(RightEye.Position, size, EyeColor, true);

        var mouthCenterX = (MouthStart.Position.X + MouthEnd.Position.X) / 2;
        var mouthCenterY = MouthStart.Position.Y;
        var mouthCenter = new Vector2(mouthCenterX, mouthCenterY * 3);
        var startAngle = 0f;
        var endAngle = Mathf.DegToRad(-180f);

        DrawArc(mouthCenter, MouthEnd.Position.X, endAngle, startAngle, 18, Colors.Black, 10f);
    }

    private void DrawNeutral()
    {
        var size = IsDrawing ? CurrentSize : EyeSize;
        DrawCircle(LeftEye.Position, size, EyeColor, true);
        DrawCircle(RightEye.Position, size, EyeColor, true);

        DrawLine(MouthStart.Position, MouthStart.Position + new Vector2(CurrentLength, 0), Colors.Black, 10f);
    }

    private void DrawEyesOnly()
    {
        var size = IsDrawing ? CurrentSize : EyeSize;
        DrawCircle(LeftEye.Position, size, EyeColor, true);
        DrawCircle(RightEye.Position, size, EyeColor, true);
    }

    public override async Task AnimateDrawing(float speed)
    {
        var angle = FaceType == FaceOption.Smiling ? 180f : -180f;
        var length = MouthEnd.Position.X - MouthStart.Position.X;
        var size = EyeSize;

        IsDrawing = true;
        var tween = CreateTween().SetParallel(true).SetEase(Tween.EaseType.In);
        tween.TweenProperty(this, "CurrentAngle", angle, speed);
        tween.TweenProperty(this, "CurrentLength", length, speed);
        tween.TweenProperty(this, "CurrentSize", size, speed);
        await tween.Completed();
        IsDrawing = false;
        Drawn = true;
        QueueRedraw();
    }

    protected override void GenerateNodes()
    {
        return;
    }
}
