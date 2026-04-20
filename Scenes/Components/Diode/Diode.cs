using Godot;
using System;

public partial class Diode : Node3D
{

    private MeshInstance3D mesh;
    private OmniLight3D light;
    private Material TurnOffMaterial;

    public override void _Ready()
    {
        base._Ready();
        mesh = GetNode<MeshInstance3D>("industrial_wall_lamp");
        light = GetNode<OmniLight3D>("light");
        TurnOffMaterial = GD.Load<Material>("uid://cgkj4iuvcoktm");
    }


    public void TurnOn()
    {
        light.SetVisible(true);
        mesh.SetSurfaceOverrideMaterial(1,null);
    }

    public void TurnOff()
    {
        light.SetVisible(false);
        mesh.SetSurfaceOverrideMaterial(1,TurnOffMaterial);
    }
}
