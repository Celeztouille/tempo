using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAvatar : MonoBehaviour
{
    [HideInInspector] public static bool isBehindGlass = false;
    [HideInInspector] public static GameObject glassObject = null;

    private PlayerActions playerAction;
    private MoveCameraManager moveCameraManager;

    // Reference to visual beat script
    private VisualBeat visualBeat;

    private void Start()
    {
        playerAction = GameObject.Find("Avatar").GetComponent<PlayerActions>();
        moveCameraManager = GameObject.Find("Camera Manager").GetComponent<MoveCameraManager>();
        visualBeat = GameObject.Find("VisualBeat").GetComponent<VisualBeat>();

        InternalClock.beatEvent.AddListener(BeatUpdate);
    }

    private void BeatUpdate()
    {
        // Detect if player is facing a wall before moving it
        if (DetectWall())
        {
            //FightHandler.ToggleScroll(false);
            Multiplier.freezeMultiplier = true;
            Score.SetMultiplier(1);
            Music.ResetBPM();
        }
        else
        {
            playerAction.SmoothMove(FightHandler.globalSpeed, 0);
            moveCameraManager.SmoothMove(FightHandler.globalSpeed, 0);
        }
    }

    // Detect if player is stuck behind a solid wall
    public bool DetectWall()
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
            else
            {
                return true;
            }
        }
        else
        {
            // Reset glass detection tags
            isBehindGlass = false;
            glassObject = null;

            // Reactivate scroll and multiplier
            //FightHandler.ToggleScroll(true);
            Multiplier.freezeMultiplier = false;
        }


        // Update Visual Beat if a glass is approaching the player
        Physics.Raycast(ray, out hit, 2f * (FightHandler.globalSpeed * FightHandler.gwidth) / (float)FightHandler.gridxstep, LayerMask.GetMask("Solid"));
        if (hit.collider != null && hit.collider.tag == "Glass")
        {
            visualBeat.ShowSmashing();
        }

        return false;
    }
}
