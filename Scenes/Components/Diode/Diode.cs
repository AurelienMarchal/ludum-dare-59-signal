using Godot;
using System;

public partial class Diode : Node3D
{

    [Export]
    NodePath meshInstance3D;

    [Export]
    int surfaceIndex;

    [Export]
    Color lightColor;

    private MeshInstance3D mesh;
    private OmniLight3D light;
    private Material TurnOffMaterial;

    public override void _Ready()
    {
        base._Ready();
        mesh = GetNode<MeshInstance3D>(meshInstance3D);
        light = GetNode<OmniLight3D>("light");
        TurnOffMaterial = GD.Load<Material>("uid://cgkj4iuvcoktm");
        SetColor(lightColor);
    }


    public void TurnOn()
    {
        light.SetVisible(true);
        mesh.SetSurfaceOverrideMaterial(surfaceIndex, null);
    }

    public void TurnOff()
    {
        light.SetVisible(false);
        mesh.SetSurfaceOverrideMaterial(surfaceIndex, TurnOffMaterial);
    }

    public void SetColor(Color color)
    {
        lightColor = color;
        light.LightColor = lightColor;
        (mesh.Mesh.SurfaceGetMaterial(surfaceIndex) as StandardMaterial3D ).AlbedoColor = lightColor;
    }
}
