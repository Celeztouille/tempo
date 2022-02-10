using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScanInputs : MonoBehaviour
{
    // Script containing the player actions to trigger (jump, fire, parry)
    private PlayerActions playerAction;

    // Map playerAction
    private void Awake() => playerAction = GameObject.Find("Avatar").GetComponent<PlayerActions>();

    // Check if we hit the right note at the right timing
    private void HitNote(Note.Type type)
    {

        // Set the vertical offset to launch the raycast on the right lane
        float verticalOffset = 0f;

        switch (type)
        {
            case Note.Type.Up:
                verticalOffset += RhythmHandler.vOffset;
                break;
            case Note.Type.Down:
                verticalOffset -= RhythmHandler.vOffset;
                break;
            default:
                break;
        }


        // Raycast from the back of the action window to check if we hit a note on this lane

        Vector2 origin = verticalOffset * Vector2.up - RhythmHandler.aBackWindow * Vector2.right;
        float length = RhythmHandler.aBackWindow + RhythmHandler.aWindow;

        RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right, length, LayerMask.GetMask("Note"));


        // If we hit the note : do the corresponding player action
        // relative distance of the note from the action bar is given by hit.distance - RhythmHandler.aBackWindow 
        if (hit.collider != null)
        {
            Debug.Log(hit.distance - RhythmHandler.aBackWindow);
            Destroy(hit.transform.gameObject);

            switch (type)
            {
                case Note.Type.Up:
                    playerAction.Jump();
                    break;
                case Note.Type.Mid:
                    playerAction.Fire();
                    break;
                case Note.Type.Down:
                    playerAction.Parry();
                    break;
            }
        }
    }


    // Input Listener : check if an input was pressed, determines which key is pressed and call HitNote() on the corresponding lane
    public void InputPressed(InputAction.CallbackContext context)
    {
        Vector3 origin = transform.position - RhythmHandler.aBackWindow * Vector3.right;
        float length = RhythmHandler.aBackWindow + RhythmHandler.aWindow;
       
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "HitUp")
            {
                Debug.DrawRay(origin + RhythmHandler.vOffset * Vector3.up, Vector3.right * length, Color.red, 0.1f);
                HitNote(Note.Type.Up);
            }

            if (context.action.name == "HitMid")
            {
                Debug.DrawRay(origin, Vector3.right * length, Color.green, 0.1f);
                HitNote(Note.Type.Mid);
            }

            if (context.action.name == "HitDown")
            {
                Debug.DrawRay(origin - RhythmHandler.vOffset * Vector3.up, Vector3.right * length, Color.blue, 0.1f);
                HitNote(Note.Type.Down);
            }
        }
    }
}
