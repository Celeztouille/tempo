using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

// With this script, the player must input the Jump button to begin the level (and start clock, music, timers and stuff) 
public class StartRhythm : MonoBehaviour
{
    private const float beatPeriod = 0.4651f;

    // Has the level started ?
    public static bool start = false;

    private static float timer = beatPeriod * 32f;

    private static TextMeshProUGUI readyText;

    void Awake()
    {
        readyText = GameObject.Find("Ready 321").GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (!start)
        {
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
                readyText.transform.gameObject.SetActive(false); // Remove Text
            }
            timer -= Time.deltaTime;
        }
    }

    public static void ReloadRhythm()
    {
        timer = beatPeriod * 32f;
        start = false;

        readyText.transform.gameObject.SetActive(true);
        readyText.text = "Ready ?";
    }
}
