using Godot;
using System;

public partial class Audio : Machine
{

    private AudioStreamPlayer3D audio;

    public override void _Ready()
    {
        base._Ready();
        audio = GetNode<AudioStreamPlayer3D>("AudioStreamPlayer3D");
    }

    public override void _Process(double delta)
    {
        if(InputSignal != null)
        {
            if(InputSignal.Signal.AsGodotObject() is AudioStream stream)
            {
                if (!audio.Playing)
                {
                    audio.Stream = stream;
                    audio.Play();
                }
                OutputNewSignal();
            }
            else
            {
                audio.Stop();
            }
        }
        else
        {
            audio.Stop();
        }
    }



}
