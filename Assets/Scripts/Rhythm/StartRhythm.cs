using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// With this script, the player must input the Jump button to begin the level (and start clock, music, timers and stuff) 
public class StartRhythm : MonoBehaviour
{
    // Has the level started ?
    public static bool start = false;


    // Input Listener : check if an input was pressed, determines which key is pressed and call HitInput()
    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "Jump")
            {
                // Set initial BPM of the track
                InternalClock.SetPeriod(129f, InternalClock.ClockFormat.BeatsPerMin, true);
                Music.StartMusic(); // Start track and clock
                DisplayTimer.StartTimer(); // Start UI Timer
                Destroy(this); // Remove this component to not interfer with further jumps
            }
        }
    }
}
