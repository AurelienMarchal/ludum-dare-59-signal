using Godot;
using System;
using System.Numerics;
using System.Threading;

public partial class ElectricalScreen : Node3D
{
    protected ColorRect _gauge0;
    protected ColorRect _gauge1;
    protected ColorRect _gauge2;
    protected ColorRect _gauge3;
    protected ColorRect _gauge4;
    protected ColorRect _gauge5;
    protected ColorRect _gauge6;
    protected ColorRect _gauge7;
    protected ColorRect _gauge8;
    protected ColorRect _gauge9;

    private Color _offColor = Color.Color8(0, 0, 0);
    private Color _onColor = Color.Color8(0, 255, 0);

    private int _currentEnergy = 0;
    private int _maxEnergy = 10;

    public Godot.Collections.Array<ColorRect> _gauges = new Godot.Collections.Array<ColorRect>();

    [Export]
    public ActuatorSwitchElectrical[] _actuators = [];

    public override void _Ready()
    {
        _gauge0 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE0/ColorRect");
        _gauge1 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE1/ColorRect");
        _gauge2 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE2/ColorRect");
        _gauge3 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE3/ColorRect");
        _gauge4 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE4/ColorRect");
        _gauge5 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE5/ColorRect");
        _gauge6 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE6/ColorRect");
        _gauge7 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE7/ColorRect");
        _gauge8 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE8/ColorRect");
        _gauge9 = GetNode<ColorRect>("TV/SubViewport/Control/VBox/GAUGE9/ColorRect");

        _gauges.Add(_gauge9);
        _gauges.Add(_gauge8);
        _gauges.Add(_gauge7);
        _gauges.Add(_gauge6);
        _gauges.Add(_gauge5);
        _gauges.Add(_gauge4);
        _gauges.Add(_gauge3);
        _gauges.Add(_gauge2);
        _gauges.Add(_gauge1);
        _gauges.Add(_gauge0);

        UpdateEnergy(0, null);

        ConnectActuators();
    }

    private void UpdateEnergy(int amount, ActuatorSwitchElectrical actuator)
    {
        if (_currentEnergy + amount > _maxEnergy)
        {
            actuator.ForceOff();
            return;
        }

        _currentEnergy += amount;
        if (_currentEnergy > _maxEnergy)
        {
            _currentEnergy = _maxEnergy;
        }
        if (_currentEnergy < 0)
        {
            _currentEnergy = 0;
        }

        for (int i = 0; i  < _maxEnergy; i++)
        {
            SetGaugeEnable(_currentEnergy > i, _gauges[i]);
        }
    }

    private void SetGaugeEnable(bool enable, ColorRect gauge)
    {
        gauge.Color = enable ? _onColor : _offColor;
    }

    private void ConnectActuators()
    {
        foreach (ActuatorSwitchElectrical actuator in _actuators)
        {
            actuator.Connect(ActuatorSwitchElectrical.SignalName.ActuatorElectricalTriggered, new Callable(this, nameof(this.ActuatorTriggered)));
        }
    }

    private void ActuatorTriggered(bool isOn, int amount, ActuatorSwitchElectrical actuator = null)
    {
        UpdateEnergy(amount = isOn ? amount : amount * -1, actuator);
    }
}