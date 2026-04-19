using Godot;
using System;

public partial class InteractiveKeyPad : Node3D
{
    


    [Signal]
    public delegate void KeyPressedEventHandler(string keyString);




    private void OnKeyPressed(string keyString)
    {
        EmitSignal(SignalName.KeyPressed, keyString);
        GD.Print(keyString + " pressed.");
    }
    
}
