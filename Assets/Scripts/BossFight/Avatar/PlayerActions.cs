using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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

    /* UNUSED FOR NOW

    [Header("Projectile")]

    // Projectile prefab
    [SerializeField] private GameObject projectile;

    [Header("Parry")]

    // Duration of the parry attack in ticks
    [SerializeField] [Range(1, 20)] private int parryDuration;

    // Shield prefab (renderer-only)
    [SerializeField] private GameObject shield;

    // Local instance of shield
    private GameObject shieldInstance;

    */

    // Local tick counters
    private int jumpTickCount;
    // UNUSED private int parryTickCount;

    // To check if player is jumping / parrying or not
    [HideInInspector] public bool isJumping = false;
    // UNUSED [HideInInspector] public bool isParrying = false;

    // To check if player is on the ground or not
    private bool isGrounded = true;

    // Initialise tick counters at <duration>+1 :
    // trigger will set counter at 0 and counter will trigger consequent event at <duration>
    private void Awake()
    {
        jumpTickCount = jumpDuration + 1;
        // UNUSED parryTickCount = parryDuration + 1;
    }

    private void Start() => InternalClock.tickEvent.AddListener(TickUpdate);

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
                BossGrid.Move(transform, 0, jumpHeight, BossGrid.OutOfBounds.Clamp);
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
                    BossGrid.Move(transform, 0, stepsUp);
                    isJumping = true;
                    isGrounded = false;
                    jumpTickCount = 0;
                    FightHandler.ToggleScroll(true);
                    Multiplier.freezeMultiplier = false;
                }
            }
        }
    }

    /* UNUSED
    // Fire function
    public void Fire()
    {
        Instantiate(projectile, transform.position, Quaternion.identity);
    }

    // Parry function
    public void Parry()
    {
        // Do not instantiate a new shield when already parrying
        if (!isParrying)
        {
            shieldInstance = Instantiate(shield, transform.position + 5 * Vector3.right, Quaternion.identity, transform);
        }
        isParrying = true;
        parryTickCount = 0;
    }

    // Un-parry function
    void Unparry()
    {
        isParrying = false;
        Destroy(shieldInstance);
    }*/

    // Count elapsed ticks on each ticks
    void TickUpdate()
    {
        // Trigger un-event functions for jump and parry
        if (jumpTickCount == jumpDuration)
        {
            isJumping = false;
        }
        /* UNUSED if (parryTickCount == parryDuration)
        {
            Unparry();
        }*/

        // If is not jumping : activate gravity
        if (!isJumping)
        {
            isGrounded = !BossGrid.Fall(transform, fallSpeed); // Bossgrid.Fall returns a boolean that indicates whether grounded or not
        }

        // Increment local tick counter
        jumpTickCount++;
        // UNUSED parryTickCount++;
    }
}