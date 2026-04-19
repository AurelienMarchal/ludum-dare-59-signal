using Godot;
using System;
using System.Threading.Tasks;

public partial class Antenna : Machine
{
    private double _angle = 0F;
    private double _rayon = 0F;
    private double _z = 3F;

    private Node3D _azimuth;
    private Node3D _elevation;

    [Export]
    protected NodePath _azimuthPath { get; set; }
    [Export]
    protected NodePath _elevationPath { get; set; }

    public override void _Ready()
    {
        base._Ready();

        if (!_azimuthPath.IsEmpty)
        {
            _azimuth = GetNode<Node3D>(_azimuthPath);
        }

        if (!_elevationPath.IsEmpty)
        {
            _elevation = GetNode<Node3D>(_elevationPath);
        }
    }

    public async Task UpdatePositionAsync(int x, int y)
    {
        _angle = Math.Atan2(x, y);
        _rayon = Math.Atan(Math.Sqrt(x * x + y * y)/_z);

        Vector3 rotation;
        rotation.X = 0;
        rotation.Y = 0;
        rotation.Z = (float)_angle;
        Tween tween = GetTree().CreateTween();
        tween.TweenProperty(_azimuth, "rotation", rotation, 15.0f);


        Vector3 rotation2;
        rotation2.X = 0;
        rotation2.Y = (float)_rayon;
        rotation2.Z = 0;
        Tween tween2 = GetTree().CreateTween();
        tween2.TweenProperty(_elevation, "rotation", rotation2, 15.0f);
        await ToSignal(GetTree().CreateTimer(15),"timeout");
        GetParent<AntennaController>().actualX = x;
        GetParent<AntennaController>().actualY = y;
        
    }
}
