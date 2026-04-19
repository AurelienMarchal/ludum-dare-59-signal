using Godot;
using System;

/* 
	This guy's only job is to create and display the physical cables that are connected using the data from his parent. That's where we'll see the cables
*/

public partial class CablesManager : Node3D
{

	private SignalRoutingPanel RoutingPanel;
	private CharacterController player;

    public override void _Ready()
    {
        RoutingPanel = GetParent<SignalRoutingPanel>();
		player = (CharacterController)GetTree().GetFirstNodeInGroup("Player");
    }

    



	//Call this when plugging a new cable so that all the cables are recreated from scratch
	public void refreshCables()
	{
		
		foreach (Node child in GetChildren())
		{
			child.QueueFree();
		}

		foreach (CableLink cable in RoutingPanel.CableList)
		{
			AddCable(cable);
		}

	}

	//Call this when plugging a new cable so that all the cables are not recreated from scratch
	public void AddCable(CableLink cable)
	{
		Vector3 start = RoutingPanel.GetPlug3DPosition(cable.A);
		Vector3 end = RoutingPanel.GetPlug3DPosition(cable.B);
		_createCableMesh(start, end);
		
	}

	private void _createCableMesh(Vector3 start, Vector3 end)
	{
		PackedScene temp = GD.Load<PackedScene>("uid://dgsibv042hcbt");
		Node3D newCable = temp.Instantiate<Node3D>();
		AddChild(newCable);
		
		newCable.Scale = new Vector3(start.DistanceTo(end), start.DistanceTo(end), start.DistanceTo(end)); 
		newCable.LookAtFromPosition(start,end);
	}
}
