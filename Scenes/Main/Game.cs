using Godot;
using System;

public partial class Game : Node
{

 	private LevelManager lm;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//Launch the default page
		lm = (LevelManager)GetTree().GetFirstNodeInGroup("LevelManager");
        //_  = lm.ChangeLevel("Scenes/Levels/Sandboxe/Sandboxe.tscn"); 
        _  = lm.ChangeLevel("uid://dj4sjbd4kp7rx"); 
    }

}
