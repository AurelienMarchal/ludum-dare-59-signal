using Godot;
using System;

public partial class ActuatorSwitchElectrical : Actuator
{
    private AudioStreamPlayer3D _audioOn;
    private AudioStreamPlayer3D _audioOff;
    private AudioStreamPlayer3D _audioError;

    private bool _isBlocked = false;

    [Signal]
    public delegate void ActuatorElectricalTriggeredEventHandler(bool isOn, int amount, ActuatorSwitchElectrical actuator);

    [Export]
    protected int _powerAmountRequired { get; set; }

    [Export]
    private bool StartsOn = false;

    [Export]
    protected Machine _machine { get; set; }

    public override void _Ready()
    {
        base._Ready();
        if (!StartsOn)
        {
            UpdateMachinePower(_isOn);
        }
        else
        {
            _isOn = true;
            _animationPlayer.Play("On");
            UpdateMachinePower(_isOn);
        }

        _audioOn = GetNode<AudioStreamPlayer3D>("AudioOn");
        _audioOff = GetNode<AudioStreamPlayer3D>("AudioOff");
        _audioError = GetNode<AudioStreamPlayer3D>("AudioError");
    }

    public override void _Input(InputEvent @event)
    {
        if (_isBlocked)
            return;

        if (@event is not InputEventMouseButton trigger) return;

        if (!_mouseEntered)
            return;

        if (!trigger.Pressed)
            return;

        ActuatorBehavior(@event);
        
        EmitSignal(SignalName.ActuatorElectricalTriggered, [_isOn, _powerAmountRequired, this]);
    }

    protected override void ActuatorBehavior(InputEvent @event = null)
    {
        base.ActuatorBehavior();
        _isOn = !_isOn;
        if (_isOn)
        {
            UpdateMachinePower(true);
            _audioOn.Play();
            _animationPlayer.Play("On");
        }
        else
        {
            UpdateMachinePower(false);
            _audioOff.Play();
            _animationPlayer.Play("Off");
        }
    }

    public void ForceOff()
    {
        AsyncForceOff();
    }

    public async void AsyncForceOff()
    {
        // Before behavior
        _isBlocked = true;
        _isOn = false;
        UpdateMachinePower(false);
        await ToSignal(GetTree().CreateTimer(0.3), "timeout");
        // After behavior
        _audioError.Play();
        _animationPlayer.Play("Off");
        _isBlocked = false;
    }

    private void UpdateMachinePower(bool isPowered)
    {
        if (_machine != null)
        {
            _machine.SetPower(isPowered);
        }
    }
}
