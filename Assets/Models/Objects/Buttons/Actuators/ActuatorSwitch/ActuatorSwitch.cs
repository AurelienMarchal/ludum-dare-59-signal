using Godot;
using System;

public partial class ActuatorSwitch : Actuator
{
    private AudioStreamPlayer3D _audioOn;
    private AudioStreamPlayer3D _audioOff;

    public override void _Ready()
    {
        base._Ready();

        _audioOn = GetNode<AudioStreamPlayer3D>("AudioOn");
        _audioOff = GetNode<AudioStreamPlayer3D>("AudioOff");
    }

    protected override void ActuatorBehavior()
    {
        base.ActuatorBehavior();
        _isOn = !_isOn;
        if (_isOn)
        {
            _audioOn.Play();
            _animationPlayer.Play("On");
        }
        else
        {
            _audioOff.Play();
            _animationPlayer.Play("Off");
        }
    }
}
