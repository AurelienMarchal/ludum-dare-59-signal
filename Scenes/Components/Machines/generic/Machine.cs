using Godot;
using Godot.Collections;
using System;

/*
	Every machine that needs to process / pass signals through needs to inherit this class

	This class implements signal in & signal out, as well as the data if it's powered or not. These will be set by the operator board and the power distribution unit respectively
*/

[GlobalClass]
public partial class Machine : Node
{	
	//This is the name of the machine, used by signal to check if this is the right one
	[Export]
	public string MachineName = "Unnamed Machine";

	//Input Signal
    public SignalAction inputSignal;

	//Output Signal (oftentimes stripped of the step we just did)
    public SignalAction outputSignal;

	//Is that machine receiving power
	public bool powered;
}
