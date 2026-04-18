using Godot;
using System;

public partial class Rope : Node3D
{
    public override void _PhysicsProcess(double delta)
    {
        Vector3 parentScale = GetParent<Node3D>().Scale;
        Vector3 temp = new Vector3(1/parentScale.X,1/parentScale.Y,parentScale.Z);
        Scale = temp;
    }        
}
