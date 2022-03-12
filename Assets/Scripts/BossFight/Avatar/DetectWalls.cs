using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWalls : MonoBehaviour
{

    // Detect if player is stuck behind a solid wall
    // This function is subscribed to the beatEvent via the MovingItem script (to ensure it is called BEFORE moving items)
    public void DetectWall()
    {
        Ray ray = new Ray(transform.position, Vector3.right);

        Debug.DrawRay(transform.position + 0.1f * Vector3.up, (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep * Vector3.right, Color.yellow, 0.3f);

        // Check if is the player is right behind a wall
        Physics.Raycast(ray, out RaycastHit hit, (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep, LayerMask.GetMask("Solid"));

        // If player is stuck, stop global scroll and freeze points, multiplier and stuff
        if (hit.collider != null)
        {
            FightHandler.ToggleScroll(false);
            Multiplier.freezeMultiplier = true;
            Score.SetMultiplier(1);
            Music.ResetBPM();
        }

        // Scroll and stuff is reactivated when player is jumping
    }
}
