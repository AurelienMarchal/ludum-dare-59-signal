using Godot;
using System;
using System.Linq;

public partial class BasicProcessing : Machine
{	
	private double CompletionProcess = 0;
	private const double COMPLETION_SPEED = 15;

    public override void _Ready()
    {
        Powered = true;

    }


	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{	
		GetNode<Label>("SubViewport/Label").Text = ((int)CompletionProcess).ToString();

		if (Powered)
		{
			if (InputSignal!=null)
			{
				SignalAction NextStep = InputSignal.ProcessingSteps[0];
				if (NextStep != null)
				{
					//Fills up the completion process
					if (CompletionProcess < 100)
					{
						CompletionProcess+= delta*COMPLETION_SPEED;
						OutputSignal = null;
					}
					//Output the new signal
					else
					{
						CompletionProcess = 100;
						OutputSignal = (GameSignal)InputSignal.DuplicateDeep();
						OutputSignal.ProcessingSteps = OutputSignal.ProcessingSteps.Skip(1).ToArray(); 
						OutputSignal.Signal = NextStep.NextSignalState;

						GetNode<Label>("SubViewport/Label").Text = (string)OutputSignal.Signal;
					}
					
				}
			}
			else
			{
				OutputSignal = null;
			}
		}
		else
		{
			OutputSignal = null;
		}
	}
}
