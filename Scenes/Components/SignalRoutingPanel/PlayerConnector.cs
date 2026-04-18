using Godot;
using System;

public partial class PlayerConnector : Node3D
{
    private CharacterController player;
    private SignalRoutingPanel RoutingPanel;

    private FollowingCable followingCable = null;

    public override void _Ready()
    {
        RoutingPanel = GetParent<SignalRoutingPanel>();
		
    }

    
    public override void _Process(double delta)
    {
        if (RoutingPanel.SelectedPlug != null)
        {
            if(followingCable == null)
            {
                PackedScene newCableScene = GD.Load<PackedScene>("uid://2hl7f4yc14u0");
                followingCable = newCableScene.Instantiate<FollowingCable>();
                AddChild(followingCable);
            }
        }
        else
        {
             if(followingCable != null)
            {
                followingCable.QueueFree();
                followingCable=null;
            }
        }
       
    }

}
