using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that handles the level music
public class Music : MonoBehaviour
{
    // UNUSED private static float hstCoeff = 1.0293021f; // How much multiply frequency to step up for a half semitone
    private static float qstCoeff = 1.01454531235f; // How much multiply frequency to step up for a quarter semitone

    // FMOD event that manage the level music
    private static FMOD.Studio.EventInstance event_fmod;

    // Internal counter
    private static int semiToneParemeter = 0;

    // Init FMOD event
    void Start() => event_fmod = FMODUnity.RuntimeManager.CreateInstance("event:/LevelMusic");

    // Toggle to start the music
    public static void StartMusic() => event_fmod.start();

    // Toggle to make the music (and internal clock) one step faster
    public static void IncrementBPM()
    {
        // Max speed is after 10 steps, we cannot go further
        if (semiToneParemeter < 10)
        {
            // Speed up the music
            event_fmod.setParameterByName("BPMChangeDiscrete", semiToneParemeter + 1);

            semiToneParemeter++;

            // Speed up the internal clock
            InternalClock.MultiplyFrequency(qstCoeff);
        }


        // one tone = speed up by 1.059463^2
        // one semitone = speed up by 1.059463 
        // one half st = speed up by sqrt(1.059463)
    }

    // Toggle to make the music (and internal clock) go at normal speed
    public static void ResetBPM()
    {
        event_fmod.setParameterByName("BPMChangeDiscrete", 0);
        semiToneParemeter = 0;
        InternalClock.ResetPeriod();
    }
}
