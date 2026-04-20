using Godot;
using Godot.NativeInterop;
using System;

public partial class NoiseReductionMachine : Machine
{

	MeshInstance3D screenMesh;

	RandomNumberGenerator rng;

	float[] cleanSignalFrequecies;
	float[] cleanSignalAmplitudes;

	float[] cleanSignalPhases;

	int cleanSignalWaveCount;

	[Export]
    Color lightColorError;

    [Export]
    Color lightColorDone;

	[Export]
    NodePath diodePath;

    Diode diode;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		screenMesh = GetNode<MeshInstance3D>("ScreenMesh");
		rng = new RandomNumberGenerator();
		GenerateCleanSignal();
		diode = GetNode<Diode>(diodePath);
	}

	protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        diode.TurnOn();
		GenerateCleanSignal();
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        diode.TurnOff();
    }

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (!Powered)
        {   
            DisplayNoSignal();
			diode.SetColor(lightColorError);
            return;
        }

        if(InputSignal == null)
        {
            DisplayNoSignal();
			OutputSignal = null;
			diode.SetColor(lightColorError);
            return;
        }

        if(InputSignal.ProcessingSteps.Length == 0)
        {
            DisplayNoSignal();
			diode.SetColor(lightColorError);
            return;
        }

        SignalAction NextStep = InputSignal.ProcessingSteps[0];

        if(NextStep == null)
        {
            DisplayNoSignal();
			diode.SetColor(lightColorError);
            return;
        }


        if(NextStep.MachineName != MachineName)
        {
            DisplayNoisySignal();
			diode.SetColor(lightColorError);
            return;
        }

		diode.SetColor(lightColorDone);

        DisplayNoisySignal();
		DiplayCleanSignal();
		OutputNewSignal();
	}

	private void DisplayNoSignal()
	{
		var rightScreenMat = screenMesh.Mesh.SurfaceGetMaterial(2);

		if(rightScreenMat is ShaderMaterial rightShaderMaterial)
		{
			rightShaderMaterial.SetShaderParameter("frequencies", new float[0]);
			rightShaderMaterial.SetShaderParameter("amplitudes", new float[0]);
			rightShaderMaterial.SetShaderParameter("phases", new float[0]);
			
			
			rightShaderMaterial.SetShaderParameter("wave_count", 0);
		}


		var leftScreenMat = screenMesh.Mesh.SurfaceGetMaterial(3);

		if(leftScreenMat is ShaderMaterial leftShaderMaterial)
		{

			leftShaderMaterial.SetShaderParameter("frequencies", new float[0]);
			leftShaderMaterial.SetShaderParameter("amplitudes", new float[0]);
			leftShaderMaterial.SetShaderParameter("phases", new float[0]);
			leftShaderMaterial.SetShaderParameter("wave_count",  0);
		}
	}

	private void GenerateCleanSignal()
	{
		cleanSignalWaveCount = rng.RandiRange(3, 5);
		cleanSignalFrequecies = new float[cleanSignalWaveCount];
		cleanSignalAmplitudes = new float[cleanSignalWaveCount];
		cleanSignalPhases = new float[cleanSignalWaveCount];

		for (int i = 0; i < cleanSignalWaveCount; i++)
		{
			cleanSignalFrequecies[i] = rng.RandfRange(0.1f, 5f);
			cleanSignalAmplitudes[i] = rng.RandfRange(0f, 0.1f);
			cleanSignalPhases[i] = rng.RandfRange(0f, Mathf.Pi);
		}

		
	}

	private void DiplayCleanSignal()
	{
		var rightScreenMat = screenMesh.Mesh.SurfaceGetMaterial(2);

		if(rightScreenMat is ShaderMaterial rightShaderMaterial)
		{
			rightShaderMaterial.SetShaderParameter("frequencies", cleanSignalFrequecies);
			rightShaderMaterial.SetShaderParameter("amplitudes", cleanSignalAmplitudes);
			rightShaderMaterial.SetShaderParameter("phases", cleanSignalPhases);
			
			rightShaderMaterial.SetShaderParameter("scroll_speed", -0.2f);
			rightShaderMaterial.SetShaderParameter("wave_count", cleanSignalWaveCount);
		}
	}

	private void DisplayNoisySignal()
	{
		var leftScreenMat = screenMesh.Mesh.SurfaceGetMaterial(3);

		if(leftScreenMat is ShaderMaterial leftShaderMaterial)
		{

			leftShaderMaterial.SetShaderParameter("frequencies", cleanSignalFrequecies);
			leftShaderMaterial.SetShaderParameter("amplitudes", cleanSignalAmplitudes);
			leftShaderMaterial.SetShaderParameter("phases", cleanSignalPhases);
			leftShaderMaterial.SetShaderParameter("scroll_speed", 0.4f);
			leftShaderMaterial.SetShaderParameter("wave_count",  cleanSignalWaveCount);
			leftShaderMaterial.SetShaderParameter("noise_amount", 1.0f);
		}
	}


}
