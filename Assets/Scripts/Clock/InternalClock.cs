using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InternalClock : MonoBehaviour
{
    // Timer period (in seconds)
    private static float period;

    // Number of clock ticks in one beat
    private static int ticksPerBeat;

    // Event that will be invoked every clock tick
    [HideInInspector] public static UnityEvent tickEvent;

    // Event that will be invoked every beat
    [HideInInspector] public static UnityEvent beatEvent;

    // Event that will be invoked every period update
    [HideInInspector] public static UnityEvent clockUpdateEvent;

    private float timer = 0f; // Clock internal timer
    private int tickCount; // Clock tick counter
    private float timerModWindow = 0.03f; // Time window on which the event can be invoked
    private bool invokedOnce = false; // bool to check if the event was invoked this clock tick

    // Clock internal timer formatting : used for Set and Get methods
    public enum ClockFormat
    {
        Frequency,          // in Hertz
        BeatsPerMin,        // in beats per minute
        TicksPerMin,        // in ticks per minute
        BeatPeriod,         // in seconds (for a beat)
        TickPeriod          // in seconds (for a tick) 
    }

    // Generate new events on Awake
    // By default : period is 0.1 seconds and 8 ticks = 1 beat
    void Awake()
    {
        period = 0.1f;
        ticksPerBeat = 8;
        tickCount = 0;

        if (tickEvent == null)
        {
            tickEvent = new UnityEvent();
        }
        if (beatEvent == null)
        {
            beatEvent = new UnityEvent();
        }
        if (clockUpdateEvent == null)
        {
            clockUpdateEvent = new UnityEvent();
        }

    }

    void Update()
    {
        float timerMod = Mathsfs.FloatModulus(timer, period);

        if (tickEvent != null)
        {
            // If we're in the timer window and event was not already invoked
            if (!invokedOnce && timerMod < timerModWindow)
            {
                tickEvent.Invoke();
                invokedOnce = true; // Block further invocations

                if (tickCount % ticksPerBeat == 0)
                {
                    beatEvent.Invoke();
                }

                tickCount++;
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
    public static void SetPeriod(float value, ClockFormat format = ClockFormat.BeatPeriod)
    {
        switch (format)
        {
            case ClockFormat.Frequency:
                period = 1f / value;
                break;

            case ClockFormat.BeatsPerMin:
                period = ticksPerBeat / (value / 60f);
                break;

            case ClockFormat.TicksPerMin:
                period = 1f / (value / 60f);
                break;

            case ClockFormat.BeatPeriod:
                period = value / ticksPerBeat;
                break;

            case ClockFormat.TickPeriod:
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
    public static float GetPeriod(ClockFormat format = ClockFormat.BeatPeriod)
    {
        switch (format)
        {
            case ClockFormat.Frequency:
                return 1f / period;

            case ClockFormat.TicksPerMin:
                return (1f / period) * 60f;

            case ClockFormat.BeatsPerMin:
                return (1f / period) * 60f / ticksPerBeat;

            case ClockFormat.BeatPeriod:
                return period * ticksPerBeat;

            case ClockFormat.TickPeriod:
                return period;

            default:
                return 0f;
        }
    }
}