using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class Multiplier : MonoBehaviour
{
    // Set window actions : perfect, good, missed (in % of time beat)
    [SerializeField] [Range(0f, 1f)] public float perfectWdw = 0.05f;
    [SerializeField] [Range(0f, 1f)] public float goodWdw = 0.2f;

    // How much bpm we add on each correct step
    [SerializeField] [Range(1f, 1.1f)] private float bpmAdded = 0.5f;

    // How much time before we consider that player has not pressed the input this beat (is a period time coefficient)
    [SerializeField] [Range(1f, 2f)] private float fullBeatWindowCoeff = 1.2f;

    // Timestamps of current beat and input
    [HideInInspector] public float timeBeat;
    private float timeInput;

    // ==== DEBUG ====
    [SerializeField] private GameObject debugText;
    // ==== DEBUG ====

    public enum StepState
    {
        Left,
        Right
    }

    private StepState stepState = StepState.Left;

    private void Start() => InternalClock.beatEvent.AddListener(UpdateTimeBeat);


    private void UpdateTimeBeat()
    {
        timeBeat = Time.fixedTime; 
    }

    // Check if we hit the input at the right timing
    private void Step(StepState step)
    {
        float period = InternalClock.GetPeriod();

        // Measure time difference between timebeat and input
        timeInput = Time.fixedTime;
        float delta = timeInput - timeBeat;
        if (delta > period / 2f)
        {
            delta -= period;
        }

        // DEBUG
        debugText.GetComponent<TextMeshProUGUI>().SetText(InternalClock.GetPeriod(InternalClock.ClockFormat.BeatsPerMin).ToString());
        // DEBUG

        // if we hit
        if (Mathf.Abs(delta) < goodWdw)
        {
            // if we hit the right step
            if (step == stepState)
            {
                InternalClock.AddBPM(bpmAdded);
                // Get points

                // Update stepState
                if (stepState == StepState.Left)
                {
                    stepState = StepState.Right;
                }
                else
                {
                    stepState = StepState.Left;
                }
            }

            // if we hit the wrong step
            else
            {
                Miss();
            }

        }
        // if we hit too soon or too late
        else
        {
            Miss();
        }
    }

    // Check if we don't hit at all for one beat long : reset bpm
    private void Update()
    {
        if (Time.fixedTime > timeInput + InternalClock.GetPeriod() * fullBeatWindowCoeff)
        {
            timeInput += InternalClock.GetPeriod();
            Miss();

            // DEBUG
            debugText.GetComponent<TextMeshProUGUI>().SetText(InternalClock.GetPeriod(InternalClock.ClockFormat.BeatsPerMin).ToString());
            // DEBUG
        }
    }

    private void Miss()
    {
        // Reset multiplier and speed
        InternalClock.ResetBPM();
    }

    // Input Listener : check if an input was pressed, determines which key is pressed and call HitNote() on the corresponding lane
    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "StepLeft")
            {
                Step(StepState.Left);
            }
            if (context.action.name == "StepRight")
            {
                Step(StepState.Right);
            }
        }
    }
}
