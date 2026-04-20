using Godot;
using System;


// '.' = court
// '-' = long
// ' ' = end of word
// '/' between words


public static class AsciiToMorseTable
{

    public static int AsciiCodeStart = 32;
    public static readonly String[] Table = {
    "/",      // 32 - [SPACE]
    "-.-.--", // 33 - !
    ".-..-.", // 34 - "
    "",       // 35 - #
    "...-..-",// 36 - $
    "",       // 37 - %
    ".-...",  // 38 - &
    ".----.", // 39 - '
    "-.--.",  // 40 - (
    "-.--.-", // 41 - )
    "",       // 42 - *
    ".-.-.",  // 43 - +
    "--..--", // 44 - ,
    "-....-", // 45 - -
    ".-.-.-", // 46 - .
    "-..-.",  // 47 - /
    "-----",  // 48 - 0
    ".----",  // 49 - 1
    "..---",  // 50 - 2
    "...--",  // 51 - 3
    "....-",  // 52 - 4
    ".....",  // 53 - 5
    "-....",  // 54 - 6
    "--...",  // 55 - 7
    "---..",  // 56 - 8
    "----.",  // 57 - 9
    "---...", // 58 - :
    "-.-.-.", // 59 - ;
    "",       // 60 - <
    "-...-",  // 61 - =
    "",       // 62 - >
    "..--..", // 63 - ?
    ".--.-.", // 64 - @
    ".-",     // 65 - A
    "-...",   // 66 - B
    "-.-.",   // 67 - C
    "-..",    // 68 - D
    ".",      // 69 - E
    "..-.",   // 70 - F
    "--.",    // 71 - G
    "....",   // 72 - H
    "..",     // 73 - I
    ".---",   // 74 - J
    "-.-",    // 75 - K
    ".-..",   // 76 - L
    "--",     // 77 - M
    "-.",     // 78 - N
    "---",    // 79 - O
    ".--.",   // 80 - P
    "--.-",   // 81 - Q
    ".-.",    // 82 - R
    "...",    // 83 - S
    "-",      // 84 - T
    "..-",    // 85 - U
    "...-",   // 86 - V
    ".--",    // 87 - W
    "-..-",   // 88 - X
    "-.--",   // 89 - Y
    "--..",   // 90 - Z
    "",       // 91 - [
    "",       // 92 - \
    "",       // 93 - ]
    "",       // 94 - ^
    "..--.-", // 95 - _
    };
}



public partial class MorseProcessing : Machine
{
    private double CompletionProcess = 0;

    private int currentCharIndex = 0;

    private Label label;

    [Export]
    float timeToProcess = 0.2f;

    float timer = 0f;

    [Export]
    Color lightColorError;

    [Export]
    Color lightColorProcessing;

    [Export]
    Color lightColorDone;

    [Export]
    public bool AsciiToMorse;

    [Export]
    NodePath diodePath;

    Diode diode;

    string decodingResult = "";

    public override void _Ready()
    {
        label = GetNode<Label>("SubViewport/Label");
        currentCharIndex = 0;
        diode = GetNode<Diode>(diodePath);
        decodingResult = "";

    }

    protected override void TurnOnBehavior()
    {
        base.TurnOnBehavior();
        diode.TurnOn();
        label.Text = "";
        currentCharIndex = 0;
        decodingResult = "";
    }

    protected override void TurnOffBehavior()
    {
        base.TurnOffBehavior();
        diode.TurnOff();
        label.Text = "";
        currentCharIndex = 0;
        decodingResult = "";
    }


    public override void _Process(double delta)
	{	
		label.Text = "No Signal" ;

        if (!Powered)
        {   
            diode.TurnOff();
            label.Text = "";
            currentCharIndex = 0;
            decodingResult = "";
            CompletionProcess = 0;
            return;
        }

        else
        {
            diode.TurnOn();
        }

        if(InputSignal == null)
        {
            diode.SetColor(lightColorError);
            label.Text = "No Signal" ;
            currentCharIndex = 0;
            decodingResult = "";
            CompletionProcess = 0;
            return;
        }

        if(InputSignal.ProcessingSteps.Length == 0)
        {
            diode.SetColor(lightColorError);
            currentCharIndex = 0;
            decodingResult = "";
            CompletionProcess = 0;
            return;
        }

        SignalAction NextStep = InputSignal.ProcessingSteps[0];

        if(NextStep == null)
        {
            diode.SetColor(lightColorError);
            label.Text = "Error" ;
            currentCharIndex = 0;
            decodingResult = "";
            CompletionProcess = 0;
            return;
        }


        if(NextStep.MachineName != MachineName)
        {
            diode.SetColor(lightColorError);
            label.Text = "Error";
            currentCharIndex = 0;
            decodingResult = "";
            CompletionProcess = 0;
            return;
        }

        string inputSignalToString = InputSignal.Signal.AsString().ToUpper();

        if(inputSignalToString == null)
        {
            diode.SetColor(lightColorError);
            label.Text = "Error" ;
            currentCharIndex = 0;
            decodingResult = "";
            CompletionProcess = 0;
            return;
        }

        if(currentCharIndex == 0)
        {
            decodingResult = "";
            diode.SetColor(lightColorProcessing);
        
        }

        if(currentCharIndex >= inputSignalToString.Length)
        {
            CompletionProcess = 100;
            label.Text = ((int)CompletionProcess).ToString() + "%" + "\n" + decodingResult;
            NextStep.NextSignalState = decodingResult;
            OutputNewSignal();
            diode.SetColor(lightColorDone);
            return;
        }

        
        label.Text = ((int)CompletionProcess).ToString() + "%" + "\n" + decodingResult;

        if(timer < timeToProcess)
        {
            timer += (float)delta;
            return;
        }

        timer -= timeToProcess;


        if (AsciiToMorse)
        {
            var c = inputSignalToString[currentCharIndex];
        
            int asciiCode = (int)c;

            if(asciiCode < AsciiToMorseTable.AsciiCodeStart || 
                asciiCode > AsciiToMorseTable.AsciiCodeStart + AsciiToMorseTable.Table.Length)
            {
                
            }
            else
            {
                string morseString = AsciiToMorseTable.Table[asciiCode - AsciiToMorseTable.AsciiCodeStart];

                
                decodingResult += morseString + (morseString == "/" ? "" : " ");
                

                
            }

            CompletionProcess = (double)currentCharIndex / (double)inputSignalToString.Length * 100;


            //GD.Print((CompletionProcess).ToString() + "%" + "\n" + decodingResult.AsString());

            currentCharIndex ++;
            
        }
        else
        {
            string morseString = "";
            var c = inputSignalToString[currentCharIndex];
            
            while (c != ' ' && c != '/' && currentCharIndex < inputSignalToString.Length)
            {
                
                morseString += inputSignalToString[currentCharIndex];
                currentCharIndex++;
                c = inputSignalToString[currentCharIndex];
            }

            


            if(morseString != "")
            {
                for(int i = 0; i < AsciiToMorseTable.Table.Length; i++)
                {
                    if(morseString == AsciiToMorseTable.Table[i])
                    {
                        
                        decodingResult += (char)(i + AsciiToMorseTable.AsciiCodeStart);
                        
                        break;
                    }
                }

                
            }

            if(c == '/')
            {
                decodingResult += " ";
            }

            CompletionProcess = (double)currentCharIndex / (double)inputSignalToString.Length * 100;

            GD.Print(((int)CompletionProcess).ToString() + "%" + "\n" + decodingResult);

            currentCharIndex ++;

            
        }

        
	}


    private void OnActuatorSwitchActuatorTriggered(bool isOn)
    {
        AsciiToMorse = isOn;
        CompletionProcess = 0;
        currentCharIndex = 0;
        decodingResult = "";
    }
}
