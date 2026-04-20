using Godot;
using System;
using System.Reflection.Emit;

public partial class LightHandler : Machine
{
    [Export]
    public Lamp[] _lamps = [];

    public override void _Ready()
    {
        base._Ready();
        UpdateLamps(Powered);
    }

    private void UpdateLamps(bool isOn)
    {
        foreach (Lamp lamp in _lamps)
        {
            lamp.UpdateLight(isOn);
        }
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        UpdateLamps(false);
    }

    protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        UpdateLamps(true);
    }
}