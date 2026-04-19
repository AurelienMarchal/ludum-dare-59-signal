using Godot;
using System;
using System.Numerics;

public partial class AntennaController : Machine
{
    private Label _labelX;
    private Label _labelY;

    private int _x = 0;
    private int _y = 0;

    //Where the antenna is currently pointing
    public int actualX = 0;
    public int actualY = 0;

    private ActuatorLever _actuatorLeverX;
    private ActuatorLever _actuatorLeverY;

    private ActuatorButtonLever _actuatorReset;
    private ActuatorButtonLever _actuatorValidate;

    [Export]
    protected NodePath _antennaPath { get; set; }
    private Antenna _antenna;

    public override void _Ready()
    {
        base._Ready();
        Powered = true;

        _labelX = GetNode<Label>("SubViewport/LabelX");
        _labelY = GetNode<Label>("SubViewport2/LabelY");

        _actuatorLeverX = GetNode<ActuatorLever>("ActuatorLeverX");
        _actuatorLeverY = GetNode<ActuatorLever>("ActuatorLeverY");

        _actuatorReset = GetNode<ActuatorButtonLever>("ActuatorButtonLeverReset");
        _actuatorValidate = GetNode<ActuatorButtonLever>("ActuatorButtonLeverValidate");

        _actuatorLeverX.Connect(ActuatorLever.SignalName.ActuatorTriggered, new Callable(this, nameof(this.ActuatorXTrigger)));
        _actuatorLeverY.Connect(ActuatorLever.SignalName.ActuatorTriggered, new Callable(this, nameof(this.ActuatorYTrigger)));

        _actuatorReset.Connect(ActuatorButtonLever.SignalName.ActuatorTriggered, new Callable(this, nameof(this.ResetMachine)));
        _actuatorValidate.Connect(ActuatorButtonLever.SignalName.ActuatorTriggered, new Callable(this, nameof(this.ValidateMachine)));

        if (!_antennaPath.IsEmpty)
        {
            _antenna = GetNode<Antenna>(_antennaPath);
        }
    }

    public override void _Process(double delta)
    {
        if (Powered)
        {
            if (InputSignal!=null)
			{
				SignalAction NextStep = InputSignal.ProcessingSteps[0];
				if (NextStep != null && NextStep.MachineName == MachineName)
                {
                    Godot.Vector2 target = (Godot.Vector2)InputSignal.Signal;
                    if(actualX == (int)target.X && actualY == (int)target.Y)
                    {
                        OutputNewSignal();
                    }
                }
            }
        }
            return;
    }

    private void ActuatorXTrigger(bool isUp)
    {
        if (isUp)
        {
            _x++;
        }
        else
        {
            _x--;
            if (_x < 0)
                _x = 0;
        }
        UpdateXLabel();
    }

    private void ActuatorYTrigger(bool isUp)
    {
        if (isUp)
        {
            _y++;
        }
        else
        {
            _y--;
            if (_y < 0)
                _y = 0;
        }
        UpdateYLabel();
    }

    private void ResetMachine(bool b)
    {
        if (!b)
            return;

        _x = 0;
        _y = 0;
        UpdateXLabel();
        UpdateYLabel();
    }

    private void ValidateMachine(bool b)
    {
        if (!b)
            return;

        if (!_antennaPath.IsEmpty)
            _ = _antenna.UpdatePositionAsync(_x, _y);
    }

    private void UpdateXLabel()
    {
        _labelX.Text = "X : " + (_x.ToString());
    }

    private void UpdateYLabel()
    {
        _labelY.Text = "Y : " + (_y.ToString());
    }
}