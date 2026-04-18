using Godot;
using System;


/*
    This defines where the signal needs to go, as well as how it looks like at that point. Machines will read SignalData when parsing it to be able 
*/

[GlobalClass]
public partial class SignalAction : Resource
{
    //This is the machine that is needed to process the signal further
    [Export]
    public string MachineName = "MachineToPassThrough";

    //This is the data of the signal after parsing, it can be a text, sound, image, whatever. Used to display the content of the signal at that point
    [Export]
    public Variant NextSignalState;
}
