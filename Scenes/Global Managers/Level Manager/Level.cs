using Godot;
using System;
using System.Threading.Tasks;

[GlobalClass]
public partial class Level : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	//Sets up the scene, animate itself entering, stuff like that
	public async Task enter()
	{
		string levelName = GetName();
		GD.Print("entering " +levelName);
	}

	//Closes everything up, save data, fade out
	public async Task exit()
	{
		string levelName = GetName();
		GD.Print("exiting " +levelName);
	}
}
