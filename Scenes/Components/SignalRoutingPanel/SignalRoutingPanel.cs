using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


//Defines a position in the panel
public class PlugPosition(NodePath targetMachine, bool isInput)
{
	public NodePath TargetMachine = targetMachine;
	public bool IsInput = isInput;
}

//Defines a cable connecting 2 plugs
public class CableLink(PlugPosition a, PlugPosition b)
{
	public PlugPosition A = a;
	public PlugPosition B = b;
}

public partial class SignalRoutingPanel : Node3D
{
	//The list of current cables
	public List<CableLink> CableList = [];

	//The plug that we put the first side of the cable. If it's not null it means we have one side of the cable in our hand. 
	public PlugPosition SelectedPlug = null;

	private CablesManager CM;

    public override void _Ready()
    {
        base._Ready();
		CM = GetNode<CablesManager>("CablesManager");
    }

	//This returns a tuple with the cable that has the plug, as well as a true if it's A that's plugged, false if B 
	private Tuple<CableLink,bool> _getCableLinkContainingPlug(PlugPosition plug)
	{
		foreach (CableLink cable in CableList)
		{
			if(_plugsAreEqual(cable.A,plug))
				return Tuple.Create(cable,true);
			
			if(_plugsAreEqual(cable.B,plug))
				return Tuple.Create(cable,false);
		}
		return null;
	}

	public void InteractedWithPlug(PlugPosition newPlug)
	{
		//We didn't have a plug in hand
		if(SelectedPlug == null)
		{
			//See if there's already a cable there
			Tuple<CableLink,bool> foundCable = _getCableLinkContainingPlug(newPlug);
			
			//If there's already something, we remove the cable, put the part we selected in our hand
			if (foundCable != null)
			{
				CableLink cable = foundCable.Item1;
				bool isA = foundCable.Item2;
				if (isA)
				{
					SelectedPlug = cable.B;
				}
				else
				{
					SelectedPlug = cable.A;
				}
				//Remove the link, we don't need it
				CableList.Remove(cable);
			}
			else
			{
				//If it didn't exist before, just add it to our hand.
				SelectedPlug = newPlug;
			}
		}
		else //We had something in our hand, so we'll probably create a new cable.
		{
			//If we clicked on the same one we had in hand, we delete it 
			if (_plugsAreEqual(SelectedPlug,newPlug))
			{
				SelectedPlug = null;
				return;
			}

			//See if there's already a cable there
			Tuple<CableLink,bool> foundCable = _getCableLinkContainingPlug(newPlug);
			if (foundCable != null) //There's already a cable there, so we don't do anything
			{
				return;
			}
			else
			{
				CableLink newCable = new CableLink(SelectedPlug, newPlug);
				CableList.Add(newCable);
				SelectedPlug = null;
			}
			
		}

		CM.refreshCables();
		
	}

	private bool _plugsAreEqual(PlugPosition a, PlugPosition b)
	{
		return a.IsInput == b.IsInput && a.TargetMachine == b.TargetMachine;
	}

	//Finds the 3D position of a plug
	public Vector3 GetPlug3DPosition(PlugPosition plug)
	{
		foreach (Node3D child in GetChildren())
		{
			MachineConnector mc = (MachineConnector)child; //This may not work if it's the cable manager, we'll see
			if (mc.TargetMachine != null)
			{
				if(mc.TargetMachine == plug.TargetMachine)
				{
					//We're not the cable manager
					if (plug.IsInput)
					{
						return mc.GetNode<Node3D>("Input").GlobalPosition;
					}
					else
					{
						return mc.GetNode<Node3D>("Output").GlobalPosition;
					}
				};
			}
			
		}

		return new Vector3(0,0,0);
	}

}
