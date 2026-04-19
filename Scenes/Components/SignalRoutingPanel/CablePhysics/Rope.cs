using Godot;
using System;
using System.Numerics;

public partial class Rope : Node3D
{
    public override void _PhysicsProcess(double delta)
    {
        Godot.Vector3 parentScale = GetParent<Node3D>().Scale; //TODO gérer le fait que les trucs soient dans le monde en 3d
        Godot.Vector3 temp = new Godot.Vector3(1/parentScale.X,1/parentScale.Y,parentScale.Z);
        Scale = temp;
    }
}
