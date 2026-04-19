using Godot;
using Godot.Collections;
using System;
using System.Linq;

/*
	Every machine that needs to process / pass signals through needs to inherit this class

	This class implements signal in & signal out, as well as the data if it's powered or not. These will be set by the operator board and the power distribution unit respectively
*/

[GlobalClass]
public partial class Machine : Node3D
{	
	//This is the name of the machine, used by signal to check if this is the right one
	[Export]
	public string MachineName = "Unnamed Machine";

	//Input Signal
	[Export]
    public GameSignal InputSignal;

	//Output Signal (oftentimes stripped of the step we just did)
	[Export]
    public GameSignal OutputSignal;

	//Is that machine receiving power
	public bool Powered;

	//Call this when processing 
	public void OutputNewSignal()
	{
		SignalAction NextStep = InputSignal.ProcessingSteps[0];
		if(NextStep != null)
		{
			OutputSignal = (GameSignal)InputSignal.DuplicateDeep();
			OutputSignal.ProcessingSteps = OutputSignal.ProcessingSteps.Skip(1).ToArray(); 
			OutputSignal.Signal = NextStep.NextSignalState;
		}
	}
}
