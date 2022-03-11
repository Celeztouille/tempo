using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using FMOD;

// Class where all the character actions are defined
public class PlayerActions : MonoBehaviour
{
    [Header("Jump")]

    // Duration of the jump in ticks
    [SerializeField] [Range(2, 20)] private int jumpDuration;

    // Height of the jump in grid coordinates
    [SerializeField] [Range(1, 10)] private int jumpHeight;

    // Fall speed in steps per tick (gravity)
    [SerializeField] [Range(1, 5)] private int fallSpeed;

    [SerializeField] [Range(0f, 1f)] private float smoothTime = 0.2f;

    [SerializeField] [Range(0f, 0.5f)] private float smoothTimeFalling = 0.05f;

    // Used for smoothing movements
    private Vector3 goalPos;
    private Vector3 refVelocity;

    // Local tick counters
    private int jumpTickCount;

    // To check if player is jumping / parrying or not
    [HideInInspector] public bool isJumping = false;

    // To check if player is on the ground or not
    private bool isGrounded = true;

    // Initialise tick counters at <duration>+1 :
    // trigger will set counter at 0 and counter will trigger consequent event at <duration>
    private void Awake()
    {
        jumpTickCount = jumpDuration + 1;

        goalPos = transform.position;
    }

    private void Start() => InternalClock.tickEvent.AddListener(TickUpdate);

    private void Update()
    {
        if (goalPos.y >= transform.position.y)
        {
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref refVelocity, smoothTime);
        }
        else
        {
            transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref refVelocity, smoothTimeFalling);
        }
    }


    // Jump function
    public void Jump()
    {
        // Don't jump if player is not grounded
        if (isGrounded)
        {
            Ray ray = new Ray(transform.position, Vector3.up);
            RaycastHit hit;

            // Check if is there is a platform above at <jumpHeight> range
            Physics.Raycast(ray, out hit, (jumpHeight * FightHandler.gheight) / (float)FightHandler.gridystep, LayerMask.GetMask("Solid"));

            // If no platform -> jump at max height
            if (hit.collider == null)
            {                
                SmoothMove(0, jumpHeight);
                Vector3 clamp = BossGrid.CheckBounds(transform, BossGrid.OutOfBounds.Clamp);
                if (clamp != Vector3.zero)
                {
                    goalPos = clamp;
                }

                isJumping = true;
                isGrounded = false;
                jumpTickCount = 0;

                // Reactivate scroll and multiplier when jumping (we need to do that if the player is stuck behind a wall)
                FightHandler.ToggleScroll(true);
                Multiplier.freezeMultiplier = false;
            }
            // If platform -> jump until reaching the ceiling
            else
            {
                // Compute steps below the platform
                int stepsUp = Mathf.FloorToInt(hit.distance / ((FightHandler.gheight) / (float)FightHandler.gridystep));

                if (stepsUp > 0)
                {
                    SmoothMove(0, stepsUp);
                    isJumping = true;
                    isGrounded = false;
                    jumpTickCount = 0;
                    FightHandler.ToggleScroll(true);
                    Multiplier.freezeMultiplier = false;
                }
            }

            // Play jump sound effect
            FMODUnity.RuntimeManager.PlayOneShot("event:/Player/Saut");
        }
    }

    // Count elapsed ticks on each ticks
    void TickUpdate()
    {
        print(isGrounded);
        // Trigger un-event functions for jump and parry
        if (jumpTickCount == jumpDuration)
        {
            isJumping = false;
        }

        // If is not jumping : activate gravity
        if (!isJumping)
        {
            isGrounded = !SmoothFall(); // SmoothFall returns a boolean that indicates whether grounded or not
        }

        // Increment local tick counter
        jumpTickCount++;
    }

    private void SmoothMove(int x, int y)
    {
        // Update the goal position of the object
        goalPos = transform.position + new Vector3(x * (FightHandler.gwidth / (float)FightHandler.gridxstep),
                                                   y * (FightHandler.gheight / (float)FightHandler.gridystep),
                                                   0f);
        goalPos = BossGrid.SnapToGrid(goalPos);
    }


    private bool SmoothFall()
    {
        // Don't fall when object is at the bottom of the grid
        if (transform.position.y > 0)
        {

            Ray ray = new Ray(transform.position, Vector3.down);
            RaycastHit hit;
            UnityEngine.Debug.DrawRay(transform.position, Vector3.down * FightHandler.gheight, Color.yellow, 1f);

            // Check if is there is a platform below
            Physics.Raycast(ray, out hit, (fallSpeed * FightHandler.gheight) / (float)FightHandler.gridystep, LayerMask.GetMask("Solid"));

            if (hit.collider == null)
            {
                SmoothMove(0, -fallSpeed);

                // Reactivate scroll and multiplier when falling (we need to do that if the player is stuck behind a wall)
                FightHandler.ToggleScroll(true);

                return true; // Object is still falling -> we return true
            }

            else 
            {
                // Compute remaining steps
                int stepsDown = Mathf.FloorToInt(hit.distance / ((FightHandler.gheight) / (float)FightHandler.gridystep));

                if (stepsDown > 0)
                {
                    SmoothMove(0, -stepsDown);

                    // Reactivate scroll and multiplier when falling (we need to do that if the player is stuck behind a wall)
                    FightHandler.ToggleScroll(true);
                }
                return false;
            }
        }
        return false;
    }
}