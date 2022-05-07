using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

// Main class that gets player inputs, measure time and update scoring, multiplier, speed, music and stuff
public class Multiplier : MonoBehaviour
{
    // Set window actions : perfect, good, missed (in % of time beat)
    [SerializeField] [Range(0f, 1f)] public float perfectWdw = 0.1f;
    [SerializeField] [Range(0f, 1f)] public float goodWdw = 0.2f;

    // UNUSED How much bpm we add on each correct step
    //[SerializeField] [Range(1f, 1.1f)] private float bpmAdded = 0.5f;

    // How much time before we consider that player has not pressed the input this beat (is a period time coefficient)
    [SerializeField] [Range(1f, 2f)] private float fullBeatWindowCoeff = 1.2f;

    // Points given by a perfect beat and a good beat
    [SerializeField] private int scorePerfectStep = 100;
    [SerializeField] private int scoreGoodStep = 50;

    // Needed perfects in a row to step up the multiplier
    [SerializeField] private int perfectForMult = 5;

    // Needed perfects/goods in a row to speed up the music and clock
    [SerializeField] private int beatsPerSpeedUp = 5;

    // Chance of moving backwards one step when missing a beat (for the first time)
    [SerializeField] [Range(0f, 1f)] private float firstMissBackChance = 0.9f;

    // Chance of moving backwards one step when missing a beat (in a miss streak)
    [SerializeField] [Range(0f, 1f)] private float missBackChance = 0.2f;

    // Reference to player transform
    [SerializeField] private Transform playerTr;

    // Reference to the HitFeedback script (to manage feedback display)
    [SerializeField] private HitFeedback hitFeedback;

    // Internal counters
    private int perfCpt = 0;
    private int speedUpCpt = 0;

    // DEBUG
    private bool toggleMiss = false;

    // Multiplier, acceleration and points are freezed when player is stuck behind a wall (to prevent grinding)
    [HideInInspector] public static bool freezeMultiplier = false;

    // Timestamps of current beat and input
    [HideInInspector] public float timeBeat;
    private float timeInput;

    // Bool to check if last beat was missed or not
    private bool lastMissed = true;

    // ==== DEBUG ====
    [SerializeField] private GameObject debugText;
    // ==== DEBUG ====

    // Script containing the player actions to trigger (jump, fire, parry)
    private PlayerActions playerAction;

    // if true : player must input steps, if false : steps are automatic
    [HideInInspector] public static bool needSteps = true;

    public enum StepState
    {
        Left,
        Right,
        Jump,
    }

    // Map playerAction
    private void Awake() => playerAction = GameObject.Find("Avatar").GetComponent<PlayerActions>();


    private void Start() => InternalClock.beatEvent.AddListener(UpdateTimeBeat);

    // Keep track of the precise timestamp of each last beat
    private void UpdateTimeBeat()
    {
        timeBeat = Time.fixedTime; 
    }

    // Update all stuff depending on how precise the player is on the beat
    private void UpdateScoreMultAndBPM(float delta)
    {
        // Update all stuff only when multiplier is not frozen
        if (!freezeMultiplier)
        {
            speedUpCpt++;

            // If we reached <beatsPerSpeedUp> consecutive hits : speed up the music
            if (speedUpCpt > beatsPerSpeedUp)
            {
                Music.IncrementBPM();
                speedUpCpt = 0;
            }

            // Check if it was perfect or good and add points consequently
            if (Mathf.Abs(delta) < goodWdw)
            {
                Score.AddToScore(scorePerfectStep);
                perfCpt++;

                // UI Feedback
                if (Mathf.Abs(delta) < perfectWdw)
                {
                    hitFeedback.SetHitFeedback(HitFeedback.Precision.Perfect);
                }
                else
                {
                    hitFeedback.SetHitFeedback(HitFeedback.Precision.Good);
                }
                      

                // If we made <perfectForMult> perfect kicks in a row : add 1 to multiplier
                if (perfCpt >= perfectForMult)
                {
                    Score.IncrementMultiplier();
                    Music.IncrementFever();
                    perfCpt = 0;
                }
            }
            // Last beat wasn't missed
            lastMissed = false;
        }
    }


    // Check if we hit the input at the right timing
    private void Step(StepState step, bool hitGlass)
    {
        float period = InternalClock.GetPeriod();

        // Measure time difference between timebeat and input
        timeInput = Time.fixedTime;
        float delta = timeInput - timeBeat;

        // If time between input and last timebeat is greater than time between input and following timebeat : snap to following timebeat
        if (delta > period / 2f)
        {
            delta -= period;
        }

        // DEBUG Show current BPM
        //debugText.GetComponent<TextMeshProUGUI>().text = ((int)InternalClock.GetPeriod(InternalClock.ClockFormat.BeatsPerMin)).ToString();
        // DEBUG

        // if we hit and multiplier is not frozen
        if (Mathf.Abs(delta) < goodWdw)
        {
            // if we jump (and we are not already jumping)
            if (step == StepState.Jump && !playerAction.isJumping)
            {
                playerAction.Jump();
                UpdateScoreMultAndBPM(delta);
            }

            else if (step == StepState.Left && InternalClock.beatsCount % 2 == 1)
            {
                // If we don't hit and there's no glass OR if we hit and there's glass : pass
                if (MoveAvatar.isBehindGlass == hitGlass)
                {
                    UpdateScoreMultAndBPM(delta);

                    // Play step sound effect
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Rhythm/TopKick");

                    // Destroy glass gameObject if we hit glass
                    if (MoveAvatar.isBehindGlass)
                    {
                        Debug.Log("SMASH");
                        Destroy(MoveAvatar.glassObject);
                    }
                }
                // If we hit and there's no glass or if we don't hit and there's glass : miss
                else
                {
                    Miss();
                }
                
            }
            // Same thing goes for the right step
            else if (step == StepState.Right && InternalClock.beatsCount % 2 == 0)
            {
                // If we don't hit and there's no glass OR if we hit and there's glass : pass
                if (MoveAvatar.isBehindGlass == hitGlass)
                {
                    UpdateScoreMultAndBPM(delta);

                    // Play step sound effect
                    FMODUnity.RuntimeManager.PlayOneShot("event:/Rhythm/Snare");

                    // Destroy glass gameObject if we hit glass
                    if (MoveAvatar.isBehindGlass)
                    {
                        Debug.Log("SMASH");
                        Destroy(MoveAvatar.glassObject);
                    }
                }
                // If we hit and there's no glass or if we don't hit and there's glass : miss
                else
                {
                    Miss();

                    // Destroy glass gameObject when passing through
                    if (MoveAvatar.isBehindGlass)
                    {
                        Destroy(MoveAvatar.glassObject);
                    }
                }
            }

            // if we hit the wrong step or jumped at the wrong time
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

    // Hit left+right to jump
    bool hitLeft = false, hitRight = false;
    float timer = 0f;

    // Check if we don't hit at all for one beat long : reset bpm
    private void Update()
    {
        // Hit left+right to jump
        if (hitLeft && hitRight)
        {
            Step(StepState.Jump, false);
            hitLeft = false;
            hitRight = false;
        }
        if (timer > 0.05f)
        {
            if (hitLeft && needSteps)
                Step(StepState.Left, false);
            if (hitRight && needSteps)
                Step(StepState.Right, false);
            hitLeft = false;
            hitRight = false;

        }

        if (needSteps)
        {
            if (Time.fixedTime > timeInput + InternalClock.GetPeriod() * fullBeatWindowCoeff)
            {
                timeInput += InternalClock.GetPeriod(); // Update timeInput to get the timestamp of the missed beat (which is the last beat)
                Miss();

                // DEBUG Update BPM Display
                //debugText.GetComponent<TextMeshProUGUI>().text = InternalClock.GetPeriod(InternalClock.ClockFormat.BeatsPerMin).ToString();
                // DEBUG
            }
        }

        timer += Time.deltaTime;
    }

    // What we need to do when we miss a beat
    private void Miss()
    {
        if (toggleMiss)
        {
            Score.SetMultiplier(1); // Reset multiplier
            Music.ResetFever();
            Music.ResetBPM();       // Reset BPM
            perfCpt = 0;            // Reset internal counters
            speedUpCpt = 0;
        }


        // UI Feedback
        hitFeedback.SetHitFeedback(HitFeedback.Precision.Miss);

        // play sound effect and move player backwards if first beat missed
        if (!lastMissed)
        {
            // Move player one step backwards
            if (Random.Range(0f, 1f) < firstMissBackChance)
            {
                playerAction.SmoothMove(-1, 0);
            }
            else
            {
                // If we are behind glass : step through the glass and break it
                if (MoveAvatar.isBehindGlass)
                {
                    Destroy(MoveAvatar.glassObject);
                }
            }

            // Play miss sound effect
            FMODUnity.RuntimeManager.PlayOneShot("event:/Rhythm/Miss");
        }
        else
        {
            // Move player one step backwards
            if (Random.Range(0f, 1f) < missBackChance)
            {
                playerAction.SmoothMove(-1, 0);
            }
            else
            {
                // If we are behind glass : step through the glass and break it
                if (MoveAvatar.isBehindGlass)
                {
                    Destroy(MoveAvatar.glassObject);
                }
            }
        }
        lastMissed = true;
    }
    


    // Input Listener (for the steps) : check if an input was pressed, determines which key is pressed and call Step() with the corresponding step sent
    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "StepLeft")
            {
                hitLeft = true;
                timer = 0f;
            }
            if (context.action.name == "StepRight")
            {
                hitRight = true;
                timer = 0f;
            }
            if (context.action.name == "HitLeft")
            {
                Step(StepState.Left, true);
            }
            if (context.action.name == "HitRight")
            {
                Step(StepState.Right, true);
            }
            if (context.action.name == "Debug2")
            {
                toggleMiss = true;
            }
        }

       
    }
}
