using Godot;
using System;

public partial class ImageDecodingMachine : Machine
{

	[Export]
	Material materialNoSignal;

	[Export]
	Material materialShutDown;

	[Export]
	Material materialError;

    MeshInstance3D screenMesh;

	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screenMesh = GetNode<MeshInstance3D>("ScreenMesh");
        Powered = true;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
		if (!Powered)
        {   
            screenMesh.Mesh.SurfaceSetMaterial(2, materialShutDown);
            return;
        }

        if(InputSignal == null)
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, materialNoSignal);
            return;
        }

        if(InputSignal.ProcessingSteps.Length == 0)
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, materialNoSignal);
            return;
        }

        SignalAction NextStep = InputSignal.ProcessingSteps[0];

        if(NextStep == null)
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, materialNoSignal);
            return;
        }


        if(NextStep.MachineName != MachineName)
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, materialError);
            return;
        }

        GodotObject inputSignalToGDObject = InputSignal.Signal.AsGodotObject();

        

        if(inputSignalToGDObject == null)
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, materialError);
            return;
        }

        if(inputSignalToGDObject is Material material)
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, material);
            OutputNewSignal();
        }
        else
        {
            screenMesh.Mesh.SurfaceSetMaterial(2, materialError);
            return;
        }

	}
}
