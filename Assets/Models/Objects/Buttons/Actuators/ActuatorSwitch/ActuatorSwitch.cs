using Godot;
using System;

public partial class ActuatorSwitch : Actuator
{
    protected override void ActuatorBehavior()
    {
        _isOn = !_isOn;
        if (_isOn)
            _animationPlayer.Play("On");
        else
            _animationPlayer.Play("Off");
    }
}
