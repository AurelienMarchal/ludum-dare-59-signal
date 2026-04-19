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
    public bool AsciiToMorse;

    public override void _Ready()
    {
        Powered = false;
        label = GetNode<Label>("SubViewport/Label");
        currentCharIndex = 0;
    }


    public override void _Process(double delta)
	{	
		label.Text = "Error" ;

        if (!Powered)
        {   
            OutputSignal = null;
            return;
        }

        if(InputSignal == null)
        {
            OutputSignal = null;
            return;
        }

        SignalAction NextStep = InputSignal.ProcessingSteps[0];

        if(NextStep == null)
        {
            OutputSignal = null;
            return;
        }


        if(NextStep.MachineName != MachineName)
        {
            OutputSignal = null;
            return;
        }

        

        string inputSignalToString = InputSignal.Signal.AsString().ToUpper();

        

        if(inputSignalToString == null)
        {
            OutputSignal = null;
            return;
        }

        if(currentCharIndex == 0)
        {
            NextStep.NextSignalState = "";
        }

        if(currentCharIndex >= inputSignalToString.Length)
        {
            CompletionProcess = 100;
            label.Text = ((int)CompletionProcess).ToString() + "%" + "\n" + NextStep.NextSignalState.AsString();
            OutputNewSignal();
            return;
        }

        label.Text = ((int)CompletionProcess).ToString() + "%" + "\n" + NextStep.NextSignalState.AsString();

        

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

                
                NextStep.NextSignalState += morseString + (morseString == "/" ? "" : " ");
                

                
            }

            CompletionProcess = (double)currentCharIndex / (double)inputSignalToString.Length * 100;


            GD.Print((CompletionProcess).ToString() + "%" + "\n" + NextStep.NextSignalState.AsString());

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
                        NextStep.NextSignalState = NextStep.NextSignalState.AsString() + (char)(i + AsciiToMorseTable.AsciiCodeStart);
                        break;
                    }
                }

                
            }

            if(c == '/')
                {
                    NextStep.NextSignalState = NextStep.NextSignalState.AsString() + " ";
                }

            

            CompletionProcess = (double)currentCharIndex / (double)inputSignalToString.Length * 100;

            GD.Print(((int)CompletionProcess).ToString() + "%" + "\n" + NextStep.NextSignalState.AsString());

            currentCharIndex ++;

            
        }

        
	}


    private void OnActuatorSwitchActuatorTriggered(bool isOn)
    {
        Powered = isOn;
        if (isOn)
        {
            currentCharIndex = 0;
        }
    }
}
