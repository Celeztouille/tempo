using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectWalls : MonoBehaviour
{
    [HideInInspector] public static bool isBehindGlass = false;
    [HideInInspector] public static GameObject glassObject = null;

    // Reference to visual beat script
    private VisualBeat visualBeat;

    private void Start()
    {
        visualBeat = GameObject.Find("VisualBeat").GetComponent<VisualBeat>();
    }


    // Detect if player is stuck behind a solid wall
    // This function is subscribed to the beatEvent via the MovingItem script (to ensure it is called BEFORE moving items)
    public void DetectWall()
    {
        Ray ray = new Ray(transform.position, Vector3.right);

        Debug.DrawRay(transform.position + 0.1f * Vector3.up, (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep * Vector3.right, Color.yellow, 0.3f);

        // Check if is the player is right behind a wall
        Physics.Raycast(ray, out RaycastHit hit, (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep, LayerMask.GetMask("Solid"));

        
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Glass")
            {
                isBehindGlass = true;
                glassObject = hit.collider.gameObject;
            }
            // If player is stuck, stop global scroll and freeze points, multiplier and stuff
            else
            {
                FightHandler.ToggleScroll(false);
                Multiplier.freezeMultiplier = true;
                Score.SetMultiplier(1);
                Music.ResetBPM();
            }
        }
        // Reset glass detection tags
        else
        {
            isBehindGlass = false;
            glassObject = null;
        }


        // Update Visual Beat if a glass is approaching the player
        Physics.Raycast(ray, out hit, 2f * (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep, LayerMask.GetMask("Solid"));
        if (hit.collider != null && hit.collider.tag == "Glass")
        {
            Debug.Log("bouh");
            visualBeat.ShowSmashing();
        }

        // Scroll and stuff is reactivated when player is jumping
    }
}
