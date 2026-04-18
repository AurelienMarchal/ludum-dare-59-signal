using Godot;
using System;

public partial class FollowingCable : Node3D
{
    private CharacterController player;
    private SignalRoutingPanel RoutingPanel;

    public override void _Ready()
    {
        RoutingPanel = GetParent<PlayerConnector>().GetParent<SignalRoutingPanel>();
		player = (CharacterController)GetTree().GetFirstNodeInGroup("Player");
    }

    
    public override void _Process(double delta)
    {
        if (RoutingPanel.SelectedPlug != null)
        {
            Vector3 start = RoutingPanel.GetPlug3DPosition(RoutingPanel.SelectedPlug);
            Vector3 end = player.GlobalPosition;

            LookAtFromPosition(start,end);

            try
            {
                Scale = new Vector3(start.DistanceTo(end),start.DistanceTo(end), start.DistanceTo(end));
            }
            catch (SystemException)
            {
            }
        }
       
    }
}
