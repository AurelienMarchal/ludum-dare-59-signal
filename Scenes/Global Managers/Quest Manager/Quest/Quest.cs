using Godot;
using System;

[GlobalClass]

public partial class Quest: Resource 
{
    //The signal that we receive from space
    [Export]
    public GameSignal Request;
    
    //What we need to send to the antenna to confirm the quest. If null; 
    [Export]
    public GameSignal ExpectedResponse;

    //Keep it between -0.5 and 0.5, ideally not too much in the middle 
    [Export]
    public Vector2 RadarPosition;
}

