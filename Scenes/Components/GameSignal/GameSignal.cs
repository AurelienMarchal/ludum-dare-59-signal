using Godot;
using System;

[GlobalClass]
public partial class GameSignal : Resource
{
	//Name of the signal
	[Export]
	public string SignalId = "UnnamedSignal";

	//The list of steps needed to process the signal, exemple : noise Reduction -> text parser
	[Export]
	public SignalAction[] ProcessingSteps = [];

	//The current state of the signal, can be audio or image depending on what we do, in the shadow, la série.
	[Export]
	public Variant Signal;

}
