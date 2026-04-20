using Godot;
using System;

public partial class RadioButton : Actuator
{
    private AudioStreamPlayer3D _audioTrigger;

    public override void _Ready()
    {
        base._Ready();
        _audioTrigger = GetNode<AudioStreamPlayer3D>("AudioTrigger");
    }

    protected override void ActuatorBehavior(InputEvent @event = null)
    {
        base.ActuatorBehavior();
        _isOn = !_isOn;
        _audioTrigger.Play();
    }
}