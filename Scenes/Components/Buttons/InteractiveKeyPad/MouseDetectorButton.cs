using Godot;
using System;

public partial class MouseDetectorButton : Area3D
{
	
    [Export]
    string keyString;

	[Signal]
    public delegate void KeyPressedEventHandler(string keyString);
	
	protected bool _mouseHovered = false;

    Sprite3D hoveredSprite; 
  
	public override void _Ready()
	{
		hoveredSprite = GetNode<Sprite3D>("HoveredSprite");
	}

    public override void _Input(InputEvent @event)
    {
        if (@event is not InputEventMouseButton trigger) return;

        if (!_mouseHovered)
            return;

        if (!trigger.Pressed)
            return;

       

        EmitSignal(SignalName.KeyPressed, keyString);
    }



	private void OnAreaEntered(Area3D area)
    {
        if(GetTree().GetNodeCountInGroup("KeyHovered") == 0)
        {
            GD.Print(keyString + " hovered");
            _mouseHovered = true;
            hoveredSprite.Visible = true;
            AddToGroup("KeyHovered");
            
            
        }
        else
        {
            var node = GetTree().GetFirstNodeInGroup("KeyHovered");

            if(node is MouseDetectorButton mouseDetectorButton)
            {
                mouseDetectorButton._mouseHovered = false;
                mouseDetectorButton.hoveredSprite.Visible = false;
                mouseDetectorButton.RemoveFromGroup("KeyHovered");
            }

            GD.Print(keyString + " hovered");
            _mouseHovered = true;
            hoveredSprite.Visible = true;
            AddToGroup("KeyHovered");
        }
        
    }

    private void OnAreaExited(Area3D area)
    {
        if (_mouseHovered)
        {
            _mouseHovered = false;
            hoveredSprite.Visible = false;
        
            RemoveFromGroup("KeyHovered");
        }
        
    }
}
