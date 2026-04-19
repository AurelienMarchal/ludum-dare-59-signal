using Godot;
using System;
using System.Text.RegularExpressions;
using System.Threading;

[GlobalClass]
public partial class Actuator : Node3D
{
    protected bool _mouseEntered = false;
    protected bool _isOn = false;
    [Signal]
    public delegate void ActuatorTriggeredEventHandler(bool isOn);

    [Export]
    protected NodePath _shaderMeshPath { get; set; }
    protected ShaderMaterial _mShaderMat;
    protected AnimationPlayer _animationPlayer;
    protected CharacterController _player;

    public override void _Ready()
    {
        // Mouse Detector SetUp
        Area3D mouseDetect = GetNode<Area3D>("MouseDetector");
        mouseDetect.Connect(Area3D.SignalName.AreaEntered, new Callable(this, nameof(this.OnAreaEntered)));
        mouseDetect.Connect(Area3D.SignalName.AreaExited, new Callable(this, nameof(this.OnAreaExited)));

        if (HasNode("AnimationPlayer"))
        {
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        // Shader SetUp
        if (!_shaderMeshPath.IsEmpty)
        {
            MeshInstance3D mShaderMesh = GetNode<MeshInstance3D>(_shaderMeshPath);
            Material mMat = mShaderMesh.MaterialOverlay;
            _mShaderMat = mMat as ShaderMaterial;
            UpdateShaderMesh(0);
        }

        _player = (CharacterController)GetTree().GetNodesInGroup("Player")[0];
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseButton trigger) return;

        if (!_mouseEntered)
            return;

        if (!trigger.Pressed)
            return;

        ActuatorBehavior(@event);

        EmitSignal(SignalName.ActuatorTriggered, _isOn);
    }

    protected virtual void ActuatorBehavior(InputEvent @event = null)
    {

    }

    private void OnAreaEntered(Area3D area)
    {
        _mouseEntered = true;
        _player.SetCurrentFocusedActuator(this);
        UpdateShaderMesh(1);
    }

    private void OnAreaExited(Area3D area)
    {
        _mouseEntered = false;
        UpdateShaderMesh(0);
    }

    private void UpdateShaderMesh(float alpha)
    {
        if (_shaderMeshPath.IsEmpty)
            return;

        _mShaderMat.SetShaderParameter("alpha", alpha);
    }

    public void ForceUnfocus()
    {
        _mouseEntered = false;
        UpdateShaderMesh(0);
    }
}