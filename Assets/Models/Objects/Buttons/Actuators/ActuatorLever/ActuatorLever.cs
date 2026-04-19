using Godot;
using System;

public partial class ActuatorLever : Actuator
{
    private bool _isIdle = true;
    private AudioStreamPlayer3D _audioUp;
    private AudioStreamPlayer3D _audioDown;

    public override void _Ready()
    {
        base._Ready();

        _audioUp = GetNode<AudioStreamPlayer3D>("AudioUp");
        _audioDown = GetNode<AudioStreamPlayer3D>("AudioDown");
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseButton trigger) return;

        if (!_mouseEntered)
            return;

        if (!trigger.Pressed)
            return;

        ActuatorBehavior(@event);
    }

    protected override void ActuatorBehavior(InputEvent @event)
    {
        base.ActuatorBehavior(@event);
        if (@event is not InputEventMouseButton trigger) return;

        if (trigger.ButtonIndex == MouseButton.Left)
        {
            if (!_isIdle)
                return;

            SetIdle(false);
            _animationPlayer.Play("Down");
            _audioDown.Play();
            _isOn = true;
        }
        else if (trigger.ButtonIndex == MouseButton.Right)
        {
            if (!_isIdle)
                return;

            SetIdle(false);
            _animationPlayer.Play("Up");
            _audioUp.Play();
            _isOn = false;
        }

        EmitSignal(SignalName.ActuatorTriggered, _isOn);
    }

    private void SetIdle(bool idle)
    {
        _isIdle = idle;
    }
}
