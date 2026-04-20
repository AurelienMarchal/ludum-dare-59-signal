using Godot;
using System;
using System.Numerics;
using Vector2 = Godot.Vector2;
using Vector3 = Godot.Vector3;

public partial class Scanner : Machine
{

    const double SCAN_ROTATION_SPEED = -2F;
    private Sprite3D Ray;
    private Sprite3D AimThing;
    private Sprite3D Ping;
    private AudioStreamPlayer3D PingAudio;
    private AudioStreamPlayer3D TVHum;
    private OmniLight3D ScreenLight;
    private Label X;
    private Label Y;

    
    
    public override void _Ready()
    {
        Ray = GetNode<Sprite3D>("Ray");
        AimThing = GetNode<Sprite3D>("AimThing");
        Ping = GetNode<Sprite3D>("Ping");
        PingAudio = GetNode<AudioStreamPlayer3D>("PingSound");
        TVHum = GetNode<AudioStreamPlayer3D>("TV hum");
        ScreenLight = GetNode<OmniLight3D>("Screen Light");

        X = GetNode<Label>("SubViewport/X");
        Y = GetNode<Label>("SubViewport2/Y");
        Powered = true;
    }

    public override void _Process( double delta)
    {
        
        if (Powered)
        {
            if (InputSignal != null)
            {
                if(InputSignal.ProcessingSteps[0].MachineName == "Scanner")
                {
                    
                    OutputNewSignal();
                }
            }
        }
        _rotateRay(delta);
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        Ray.SetVisible(false);
        AimThing.SetVisible(false);
        Ping.SetVisible(false);
        TVHum.Stop();
        ScreenLight.SetVisible(false);
    }

    protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        Ray.SetVisible(true);
        AimThing.SetVisible(true);
        Ping.SetVisible(true);
        TVHum.Play();
        ScreenLight.SetVisible(true);
    }


    public void _rotateRay(double delta)
    {
        Ray.Rotate(Ray.Basis.Column2.Normalized(), (float)(SCAN_ROTATION_SPEED * delta));
    }
    
    public void _on_timer_timeout()
    {
        if (Powered)
        {
            X.Text = "X:--";
            Y.Text = "Y:--";

            if (InputSignal != null)
            {
                if(InputSignal.ProcessingSteps[0].MachineName == "Scanner")
                {
                    Vector2 pingLocation = (Vector2)InputSignal.Signal;
                    Ping.Position = new Vector3(pingLocation.X, pingLocation.Y, 0);
                    PingAudio.Play();
                    Ping.Modulate = new Color(1,1,1,1);
                    GetTree().CreateTween().TweenProperty(Ping, "modulate",new Color(1,1,1,0),2);

                    X.Text = "X:"+((Vector2)InputSignal.ProcessingSteps[1].NextSignalState).X.ToString();
                    Y.Text = "Y:"+((Vector2)InputSignal.ProcessingSteps[1].NextSignalState).Y.ToString();
                    
                }
            }
        }
    }
}
