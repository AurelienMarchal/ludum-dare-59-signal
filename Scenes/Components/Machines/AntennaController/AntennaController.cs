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

    //The signal in space, we don't use InputSignal because we need to have a space signal as well as an input signal for the response
    [Export]
    public GameSignal OverrideSignal;
    

    private AudioStreamPlayer3D MovingAudio;

    private Diode confirmDiode;

    public override void _Ready()
    {
        base._Ready();

        _labelX = GetNode<Label>("SubViewport/LabelX");
        _labelY = GetNode<Label>("SubViewport2/LabelY");
        
        MovingAudio = GetNode<AudioStreamPlayer3D>("Antenna/AudioStreamPlayer3D");

        confirmDiode = GetNode<Diode>("Diode");

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
            confirmDiode.TurnOff();
            //If there's a signal in space, we look into it
            if (OverrideSignal!=null)
			{
				SignalAction NextStep = OverrideSignal.ProcessingSteps[0];
				if (NextStep != null && NextStep.MachineName == MachineName)
                {
                    Godot.Vector2 target = (Godot.Vector2)OverrideSignal.Signal;
                    if(actualX == (int)target.X && actualY == (int)target.Y)
                    {
                        confirmDiode.TurnOn();
                        OutputNewSignal(OverrideSignal);
                    }
                }
            }

            //The check for if the response is correct is to just check it's input Signal, the Quest Manager is doing that
        }
        else
        {
            confirmDiode.TurnOff();
        }
            return;
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        confirmDiode.TurnOff();
        if (_labelX==null)
            _labelX = GetNode<Label>("SubViewport/LabelX");
        if (_labelY==null)
            _labelY = GetNode<Label>("SubViewport2/LabelY");

        _labelX.Text = "";
        _labelY.Text = "";

        GetTree().SetGroup("AntennaLights", "visible", false);
    }

    protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        _labelX.Text = "X : " + (_x.ToString());
        _labelY.Text = "Y : " + (_y.ToString());

        GetTree().SetGroup("AntennaLights", "visible", true);
    }

    private void ActuatorXTrigger(bool isUp)
    {
        if (!Powered)
            return;

        if (isUp)
        {
            _x++;
        }
        else
        {
            _x--;
        }
        UpdateXLabel();
    }

    private void ActuatorYTrigger(bool isUp)
    {
        if (!Powered)
            return;

        if (isUp)
        {
            _y++;
        }
        else
        {
            _y--;
        }
        UpdateYLabel();
    }

    private void ResetMachine(bool b)
    {
        if (!Powered)
            return;

        if (!b)
            return;

        _x = 0;
        _y = 0;
        UpdateXLabel();
        UpdateYLabel();
    }

    private void ValidateMachine(bool b)
    {
        if (!Powered)
            return;

        if (!b)
            return;

        if (!_antennaPath.IsEmpty)
        {
            _ = _antenna.UpdatePositionAsync(_x, _y);
            MovingAudio.Play();
        }
    }

    private void UpdateXLabel()
    {
        if (!Powered)
            return;

        _labelX.Text = "X : " + (_x.ToString());
    }

    private void UpdateYLabel()
    {
        if (!Powered)
            return;

        _labelY.Text = "Y : " + (_y.ToString());
    }
}