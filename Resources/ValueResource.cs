using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Godot;

namespace dudebattler.Resources;

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
                    _value = (float)Math.Floor(value);
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

    public int FlooredValue => (int)Math.Floor(Value);
    public int CeilValue => (int)Math.Ceiling(Value);
    public int RoundValue => (int)Math.Round(Value);
}
