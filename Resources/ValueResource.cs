using System;
using DudeBattler.Scripts;
using Godot;

namespace DudeBattler.Resources;

public partial class ValueResource : Resource
{
    private float _value = 0.0f;
    [Export]
    public float Value
    {
        get => _value;
        set
        {
            if (_value != value)
            {
                _value = value;
                if (Integer)
                {
                    _value = (float)Math.Floor(_value);
                }
                EmitChanged();
            }
        }
    }

    private bool _integer = false;
    [Export]
    public bool Integer
    {
        get => _integer;
        set
        {
            if (_integer != value)
            {
                _integer = value;
                if (value)
                {
                    _value = (float)Math.Floor(_value);
                }
                EmitChanged();
            }
        }
    }

    private float _min = float.MinValue;
    [Export]
    public float Min
    {
        get => _min;
        set
        {
            if (_min != value)
            {
                _min = value;
                if (Value < value)
                {
                    Value = value;
                }
                EmitChanged();
            }
        }
    }

    private float _max = float.MaxValue;
    [Export]
    public float Max
    {
        get => _max;
        set
        {
            if (_max != value)
            {
                _max = value;
                if (Value > value)
                {
                    Value = value;
                }
                EmitChanged();
            }
        }
    }

    public int FlooredValue => (int)Math.Floor(Value);
    public int CeilValue => (int)Math.Ceiling(Value);
    public int RoundValue => (int)Math.Round(Value);

    public void RandomizeValue()
    {
        float num = Tools.Rand.Next((int)Math.Floor(Min), (int)Math.Ceiling(Max) + 1);
        if (!Integer) num += (float)Tools.Rand.NextDouble();
        Value = num;
    }
}
