using Godot;
using System;

public partial class LedSign : Machine
{
    private Label _label; 
    [Export]
    private String _defaultMessage = "No Signal Connected";
    private String _currentMessage;

    public override void _Ready()
    {
        base._Ready();
        _label = GetNode<Label>("SubViewport/Control/Text");
        UpdateMessage();
    }

    public void UpdateMessage()
    {
        if (InputSignal != null)
        {
            SignalAction NextStep = InputSignal.ProcessingSteps[0];
            String machineName = NextStep.MachineName.ToString();
            if (machineName == null || machineName == "")
            {
                _label.Text = _defaultMessage;
                _currentMessage = _defaultMessage;
            }
            else
            {
                _label.Text = machineName;
                _currentMessage = machineName;
            }
        }
        else
        {
            _label.Text = _defaultMessage;
            _currentMessage = _defaultMessage;
        }
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        _label.Text = "";
    }

    protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        _label.Text = _currentMessage;
    }
}
