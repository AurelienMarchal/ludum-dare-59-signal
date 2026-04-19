using Godot;
using System;
using System.ComponentModel;

public partial class Plug : Actuator
{	
	//True if the plug represents the input of the machine
	[Export]
	public bool IsInput;

	//Calls the parent, saying that the player has clicked on a plug, and what it is
    protected override void ActuatorBehavior()
    {
        base.ActuatorBehavior();
		MachineConnector MC = GetParent<MachineConnector>();
		MC.InteractWithPlug(IsInput);

    }
}
