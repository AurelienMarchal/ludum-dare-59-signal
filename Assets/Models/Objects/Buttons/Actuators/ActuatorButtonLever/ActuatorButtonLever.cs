using Godot;
using System;

public partial class ActuatorButtonLever : Actuator
{
    private bool _isIdle = true;
    private AudioStreamPlayer3D _audioTrigger;

    public override void _Ready()
    {
        base._Ready();

        _audioTrigger = GetNode<AudioStreamPlayer3D>("AudioTrigger");
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

        if ((trigger.ButtonIndex == MouseButton.Left) || (trigger.ButtonIndex == MouseButton.Right))
        {
            if (!_isIdle)
                return;

            SetIdle(false);
            if (_animationPlayer != null)
                _animationPlayer.Play("Up");
            _audioTrigger.Play();
            _isOn = true;
        }

        EmitSignal(SignalName.ActuatorTriggered, _isOn);
    }

    private void SetIdle(bool idle)
    {
        _isIdle = idle;
        _isOn = false;
    }
}
