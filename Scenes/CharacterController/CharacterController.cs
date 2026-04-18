using Godot;
using System;
using static Godot.TextServer;

public partial class CharacterController : CharacterBody3D
{
    private Node3D _head;
    private Camera3D _view;

    [Export]
    public int FallAcceleration { get; set; } = 75;

    private float _cameraAngle = 0F;
    private float _mouseSensitivity = 0.1F;
    private float _moveSpeed = 20F;
    private bool _pauseController = false;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
        _head = GetNode<Node3D>("Pivot");
        _view = GetNode<Camera3D>("Pivot/Camera");
    }

    public override void _Process(double delta)
    {
        if (Input.IsActionPressed("ui_cancel"))
        {
            if (Input.MouseMode == Input.MouseModeEnum.Captured)
            {
                _pauseController = true;
                Input.MouseMode = Input.MouseModeEnum.Visible;
            }
            else if (Input.MouseMode == Input.MouseModeEnum.Visible)
            {
                _pauseController = false;
                Input.MouseMode = Input.MouseModeEnum.Captured;
            }
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Walk(delta);
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion motion) return;

        if (_pauseController)
            return;

        _head.RotateY(Mathf.DegToRad(-motion.Relative.X * _mouseSensitivity));
        float change = -motion.Relative.Y * _mouseSensitivity;

        if (!((change + _cameraAngle) < 90F) || !((change + _cameraAngle) > -90F)) return;

        _view.RotateX(Mathf.DegToRad(change));
        _cameraAngle += change;
    }

    private void Walk(double delta)
    {
        if (_pauseController)
            return;

        Vector3 direction = new();
        Basis aim = _view.GlobalTransform.Basis;

        if (Input.IsActionPressed("ui_up"))
            direction -= aim.Z;

        if (Input.IsActionPressed("ui_down"))
            direction += aim.Z;

        if (Input.IsActionPressed("ui_left"))
            direction -= aim.X;

        if (Input.IsActionPressed("ui_right"))
            direction += aim.X;

        direction.Y -= FallAcceleration * (float)delta;

        Velocity = direction.Normalized() * _moveSpeed;

        MoveAndSlide();
    }
}