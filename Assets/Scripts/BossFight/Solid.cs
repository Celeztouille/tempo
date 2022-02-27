using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour for solid platforms
public class Solid : MonoBehaviour
{
    void Start()
    {
        BossGrid.SnapToGrid(transform); // Snap all solid on start
        InternalClock.tickEvent.AddListener(TickUpdate);
    }

    // Move all solid elements at the global scroll speed each tick
    void TickUpdate()
    {
        BossGrid.Move(transform, -FightHandler.globalSpeed, 0);
        DetectPlayer(); // Detect player proximity presence each tick
    }

    // Detect if player is stuck behind a solid wall
    private void DetectPlayer()
    {
        Ray ray = new Ray(transform.position, Vector3.left);
        RaycastHit hit;

        // Check if is the player is right behind the platform
        Physics.Raycast(ray, out hit, (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep, LayerMask.GetMask("Player"));

        // If player is here, stop global scroll and freeze points, multiplier and stuff
        if (hit.collider != null)
        {
            FightHandler.ToggleScroll(false);
            Multiplier.freezeMultiplier = true;
            Score.SetMultiplier(1);
            Music.ResetBPM();
        }

        // Scroll and stuff is reactivated when player is jumping
    }


    /* UNUSED
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerLives.AddLife(-1);
            BossGrid.Move(other.transform, 0, 2);

            Score.SetMultiplier(1);
            InternalClock.ResetPeriod();
        }
    }
    */

}
