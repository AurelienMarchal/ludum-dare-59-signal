using Godot;
using System;

public partial class Lamp : Node3D
{
    private MeshInstance3D _meshOn;
    private MeshInstance3D _meshOff;
    private Light3D _light;

    public override void _Ready()
    {
        base._Ready();
        _meshOn = GetNode<MeshInstance3D>("MeshOn");
        _meshOff = GetNode<MeshInstance3D>("MeshOff");
        _light = GetNode<Light3D>("Light");

        UpdateLight(true);
    }

    public void UpdateLight(bool isOn)
    {
        _light.Visible = isOn;
        _meshOn.Visible = isOn;
        _meshOff.Visible = !isOn;
    }
}