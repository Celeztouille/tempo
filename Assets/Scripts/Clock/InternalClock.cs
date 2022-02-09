using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InternalClock : MonoBehaviour
{
    // Timer period (in seconds)
    private static float period;

    // Event that will be invoked every clock tick
    [HideInInspector] public static UnityEvent emitter;

    // Event that will be invoked every period update
    [HideInInspector] public static UnityEvent clockUpdateEvent;

    private float timer = 0f; // Clock internal timer
    private float timerModWindow = 0.05f; // Time window on which the event can be invoked
    private bool invokedOnce = false; // bool to check if the event was invoked this clock tick

    // Clock internal timer formatting : used for Set and Get methods
    public enum ClockFormat
    {
        Frequency, // in Hertz
        BPM,       // in BPM
        Period     // in seconds
    }

    // Generate new events on Awake
    // By default : period is one second (60bpm)
    void Awake()
    {
        period = 1f;

        if (emitter == null)
        {
            emitter = new UnityEvent();
        }
        if (clockUpdateEvent == null)
        {
            clockUpdateEvent = new UnityEvent();
        }

    }

    void Update()
    {
        float timerMod = Mathsfs.FloatModulus(timer, period);

        if (emitter != null)
        {
            // If we're in the timer window and event was not already invoked
            if (!invokedOnce && timerMod < timerModWindow)
            {
                emitter.Invoke();
                invokedOnce = true; // Block further invocations
            }

            // When we're out of the window : unblock invocations for the upcoming window
            if (timerMod > timerModWindow)
            {
                invokedOnce = false;
            }
        }

        // Update timer
        timer += Time.deltaTime;
    }

    // Set up clock internal timer
    public static void SetPeriod(float value, ClockFormat format = ClockFormat.Period)
    {
        switch (format)
        {
            case ClockFormat.Frequency:
                period = 1f / value;
                break;

            case ClockFormat.BPM:
                period = 1f / (value / 60f);
                break;

            case ClockFormat.Period:
                period = value;
                break;

            default:
                break;
        }

        // Send messages to functions that depends on the period
        if (clockUpdateEvent != null)
        {
            clockUpdateEvent.Invoke();
        }
        else
        {
            Debug.LogError("cannot invoke clockUpdateEvent : event is null");
        }
    }

    // Get clock internal timer
    public static float GetPeriod(ClockFormat format = ClockFormat.Period)
    {
        switch (format)
        {
            case ClockFormat.Frequency:
                return 1f / period;

            case ClockFormat.BPM:
                return (1f / period) * 60f;

            case ClockFormat.Period:
                return period;

            default:
                return 0f;
        }
    }
}