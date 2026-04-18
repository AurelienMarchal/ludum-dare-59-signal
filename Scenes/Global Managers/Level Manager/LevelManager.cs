using Godot;
using GodotPlugins.Game;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

/*
	This is the thing that will move us from scene to scene. It's accessible globally in the group "LevelManager" with 
	
	LevelManager lm = (LevelManager)GetTree().GetFirstNodeInGroup("LevelManager");

	ChangeLevel will execute the exit() method of the current level, then delete it, then execute the start method of the next level
	
	There's only one children to this node, and it's the current level
*/


public partial class LevelManager : Node
{	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//Call this whenever you want to change a level (go from main menu to game)
	public async Task ChangeLevel(string nextLevel)
	{	
		//Remove previous level if there's one
		if (GetChildCount() > 0)
		{
			Level PreviousLevel = (Level)GetChild(0); 
			await PreviousLevel.exit();
			PreviousLevel.QueueFree();
		}
		//Add the new one
		PackedScene NextScene = GD.Load<PackedScene>(nextLevel);
		Level NextLevel = NextScene.Instantiate<Level>();
		AddChild(NextLevel);
		await NextLevel.enter();
	}
}
