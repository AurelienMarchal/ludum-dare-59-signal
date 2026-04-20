using Godot;
using System;
using System.Reflection.Emit;

public partial class Radio : Machine
{
    [Export]
    public AudioStreamPlayer3D[] _musics = [];
    private int _currentIndex = 0;
    private int _currentVolume = -20;
    private int _maxDB = 10;
    private int _minDB = -60;
    private int _step = 10;

    private Actuator _actuatorNext;
    private Actuator _actuatorVolume;

    public override void _Ready()
    {
        base._Ready();
        ActivateMusic(Powered);

        _actuatorNext = GetNode<Actuator>("ButtonNext");
        _actuatorVolume = GetNode<Actuator>("ButtonVolume");

        _actuatorNext.Connect(Actuator.SignalName.ActuatorTriggered, new Callable(this, nameof(this.UpdateNext)));
        _actuatorVolume.Connect(Actuator.SignalName.ActuatorTriggered, new Callable(this, nameof(this.UpdateVolume)));
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        ActivateMusic(true);
    }

    protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        ActivateMusic(false);
    }

    private void ActivateMusic(bool isOn)
    {
        if (isOn && Powered)
        {
            _musics[_currentIndex].VolumeDb = _currentVolume;
            _musics[_currentIndex].Play();
        }
        else
        {
            _musics[_currentIndex].Stop();
        }
    }

    private void UpdateNext(bool isOn)
    {
        ActivateMusic(false);
        _currentIndex++;
        if (_currentIndex >= _musics.Length)
            _currentIndex = 0;
        ActivateMusic(true);
    }


    private void UpdateVolume(bool isOn)
    {
        _currentVolume += _step;
        if (_currentVolume >= _maxDB)
            _currentVolume = _minDB;

        _musics[_currentIndex].VolumeDb = _currentVolume;
    }
}
