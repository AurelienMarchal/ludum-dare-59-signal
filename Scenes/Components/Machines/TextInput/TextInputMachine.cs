using Godot;
using System;

public partial class TextInputMachine : Machine
{
	[Export]
	NodePath labelNodePath;
	
	Label label;

	string text;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>(labelNodePath);
		text = "";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		label.Text = text == "" ? "text .." : text;
	}

	private void OnKeyPadKeyPressed(string keyString)
	{

		if(keyString == "*")
		{
			text = text.Left(text.Length - 1);
		}
		else
		{
			text += keyString;
		}
		
	}
}
