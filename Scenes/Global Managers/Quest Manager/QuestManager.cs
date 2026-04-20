using Godot;
using Godot.Collections;
using System;
using System.Linq;
using System.Threading.Tasks;

public partial class QuestManager : Node
{   
    //The list of basic quests, before we do the alien shit
    [Export]
    Array<Quest> QuestList;
    public Quest CurrentQuest;

    [Export]
    NodePath scannerPath;
    [Export]
    NodePath antennaPath;
    Scanner scanner;
    AntennaController antenna;

    RandomNumberGenerator rng;


    public override void _Ready()
    {
        base._Ready();
        scanner = GetNode<Scanner>(scannerPath);
        antenna = GetNode<AntennaController>(antennaPath);
        rng = new RandomNumberGenerator();
        rng.Randomize();
        SetupQuest();
    }   

    public override void _Process(double delta)
    {
        base._Process(delta);
        //If there's an active quest 
        if(CurrentQuest != null)
        {
            //If we're waiting for a response
            if(CurrentQuest.ExpectedResponse != null)
            {
                //
                if (antenna.InputSignal != null)
                {
                    //If the response is the one we expected
                    if(antenna.InputSignal.Signal.ToString() == CurrentQuest.ExpectedResponse.Signal.ToString())
                    {
                        //If the antenna has power
                        if (antenna.Powered)
                        {
                            _ = QuestCompleteAsync();
                        }
                        
                    }
                    
                }
            }
            
            
        }
    }


    //That's a bit wonky but it'll work, it's okay
    private void _setupRadar()
    {

        SignalAction action = new SignalAction
        {
            NextSignalState = CurrentQuest.Request.Signal
        };


        GameSignal scannerInput = new GameSignal
        {
            Signal = CurrentQuest.RadarPosition,
            ProcessingSteps = [action]
        };

        scanner.InputSignal = scannerInput;

    }

    //Remove a signal from the radar
    private void _stopRadar()
    {
        scanner.InputSignal = null;
    }


    public void SetupQuest()
    {
        CurrentQuest = QuestList[0];
        QuestList = QuestList.Slice(1); //Remove the first quest
        _setupRadar();
        
        if(CurrentQuest.ExpectedResponse == null)
        {
            CurrentQuest.Request.ShouldSignalQuestOnCompletion = true;
        }

        antenna.OverrideSignal = CurrentQuest.Request;
    }

    //Called by the machines when the signal is complete or by the antenna if the signal inputted is the one we expected
    public async Task QuestCompleteAsync()
    {
        GD.Print("Quest Complete");
        CurrentQuest = null;
        //Simulate upload time
        await ToSignal(GetTree().CreateTimer(rng.RandiRange(10,20)),"timeout");

        //Signal no longer appears anywhere
        antenna.OverrideSignal = null;
        _stopRadar();
        await ToSignal(GetTree().CreateTimer(1),"timeout");

        //Signal no longer appears anywhere
        if(QuestList.Count > 0)
        {
            //Wait a little bit before the new signal appears
            await ToSignal(GetTree().CreateTimer(rng.RandiRange(10,25)),"timeout");
            SetupQuest();
        }else
        {
            AlienStuff();
        }

    }


    //Inshallah y'a le temps
    public void AlienStuff()
    {
        GD.Print("BOUH LES ALIENS ILS FONT PEUR§!!!!");
    }

}
