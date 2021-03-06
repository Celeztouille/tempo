using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// With this script, the player must input the Jump button to begin the level (and start clock, music, timers and stuff) 
public class StartRhythm : MonoBehaviour
{
    private const float beatPeriod = 0.4651f;

    private Animator Run;

    // Has the level started ?
    public static bool start = false;
    private static bool hasMusicStarted = false;

    private static float timer = beatPeriod * 32f + 2f; // 2 seconds before launching music

    private static TextMeshProUGUI readyText;

    void Awake()
    {
        readyText = GameObject.Find("Ready 321").GetComponent<TextMeshProUGUI>();

        Run = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!start)
        {
            if (!hasMusicStarted && timer < beatPeriod * 32f)
            {
                FMODUnity.RuntimeManager.PlayOneShot("event:/Rhythm/Intro");
                readyText.text = "Ready ?";
                hasMusicStarted = true;
            }

            if (timer < beatPeriod)
            {
                readyText.text = "Go !";
            }
            else if (timer < beatPeriod * 2f)
            {
                readyText.text = "1";
            }
            else if (timer < beatPeriod * 3f)
            {
                readyText.text = "2";
            }
            else if (timer < beatPeriod * 4f)
            {
                readyText.text = "3";
            }

            if (timer < 0)
            {
                InternalClock.SetPeriod(129f, InternalClock.ClockFormat.BeatsPerMin, true);
                Music.StartMusic(); // Start track and clock
                DisplayTimer.StartTimer(); // Start UI Timer
                start = true;
                if (Run != null)
                {
                    Run.SetTrigger("fastRun");
                }
                readyText.transform.gameObject.SetActive(false); // Remove Text
                
            }
            timer -= Time.deltaTime;
        }
    }

    public static void ReloadRhythm()
    {
        timer = beatPeriod * 32f + 2f; // Reload with 2 seconds delay to sync up the music
        hasMusicStarted = false;
        start = false;

        readyText.transform.gameObject.SetActive(true);
        readyText.text = "";
    }
}
