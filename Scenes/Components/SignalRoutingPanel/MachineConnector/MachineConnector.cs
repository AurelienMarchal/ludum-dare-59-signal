using Godot;
using System;

public partial class MachineConnector : Node3D
{

	[Export]
	public NodePath TargetMachine;

    public override void _Ready()
    {
        TargetMachine=TargetMachine.ToString().Right(3);
    }


	//Someone Clicked on a plug, we need to get that information up to the global pannel since it's him that will do most of the processing
	public void InteractWithPlug(bool IsInput)
	{
		SignalRoutingPanel Panel = GetParent<SignalRoutingPanel>();
		Panel.InteractedWithPlug(new PlugPosition(TargetMachine, IsInput));
	}

}
