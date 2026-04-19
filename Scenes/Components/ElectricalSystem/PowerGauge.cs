using Godot;
using System;
using System.Reflection.Metadata;
using System.Threading;

public partial class PowerGauge : Node3D
{
    private Node3D _pivotIndicator;

    public override void _Ready()
    {
        _pivotIndicator = GetNode<Node3D>("Mesh/Indicator");
    }

    public void UpdateGauge(bool isOn)
    {
        if (_pivotIndicator == null)
            _pivotIndicator = GetNode<Node3D>("Mesh/Indicator");

        Vector3 rotation;
        rotation.X = 0;
        rotation.Y = isOn ? -217.7F : -140F;
        rotation.Z = 0;
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_pivotIndicator, "rotation", rotation, isOn ? 0.4 : 0.2);
    }
}
