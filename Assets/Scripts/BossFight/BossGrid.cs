using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class handling all kinds of movements within the grid
public class BossGrid
{

    // Possible behaviours when an object is out of bounds
    public enum OutOfBounds
    {
        Allow,          // let go and continue
        Clamp,          // clamp the movement to stick at the bound of the grid
        Destroy,        // destroy the game object immediately
        DestroyAfter    // destroy the game object behind the grid
    }

    // Main function to move objects within the grid
    public static void Move(Transform tr, int x, int y, OutOfBounds allowOutOfBounds = OutOfBounds.Allow)
    {

        // Move the object
        tr.position += new Vector3(x * (FightHandler.gwidth / (float)FightHandler.gridxstep),
                                   y * (FightHandler.gheight / (float)FightHandler.gridystep),
                                   0f);


        // Selected behaviours when transform is getting out of the bounds of the grid
        // Default option : let go and continue
        // First option : destroy the game object immediately
        // Second option : destroy the game object behind the grid
        // Third option : clamp the movement to stick at the bound of the grid
        if (allowOutOfBounds == OutOfBounds.Destroy)
        {
            if (tr.position.y > FightHandler.gheight || tr.position.x > FightHandler.gwidth || tr.position.x < 0 || tr.position.y < 0)
            {
                MonoBehaviour.Destroy(tr.gameObject);
            }
        }

        if (allowOutOfBounds == OutOfBounds.DestroyAfter)
        {
            if (tr.position.y > FightHandler.gheight + 10f || tr.position.x > FightHandler.gwidth + 10f || tr.position.x < -10f || tr.position.y < -10f)
            {
                MonoBehaviour.Destroy(tr.gameObject);
            }
        }

        if (allowOutOfBounds == OutOfBounds.Clamp)
        {
            if (tr.position.y > FightHandler.gheight)
            {
                tr.position = new Vector3(tr.position.x, FightHandler.gheight, tr.position.z);
            }
            if (tr.position.x > FightHandler.gwidth)
            {
                tr.position = new Vector3(FightHandler.gwidth, tr.position.y, tr.position.z);
            }
            if (tr.position.y < 0)
            {
                tr.position = new Vector3(tr.position.x, 0, tr.position.z);
            }
            if (tr.position.x < 0)
            {
                tr.position = new Vector3(0, tr.position.y, tr.position.z);
            }
        }

    }

    // Gravity emulator : move object downwards at <fallspeed> if object is mid-air 
    // Return true if object is falling and false if object is already grounded
    public static bool Fall(Transform tr, int fallSpeed)
    {
        // Don't fall when object is at the bottom of the grid
        if (tr.position.y > 0)
        {

            Ray ray = new Ray(tr.position, Vector3.down);
            RaycastHit hit;

            // Check if is there is a platform below
            Physics.Raycast(ray, out hit, (fallSpeed * FightHandler.gheight) / (float)FightHandler.gridystep, LayerMask.GetMask("Solid"));


            // If no platform -> move object <fallspeed> steps downwards
            if (hit.collider == null)
            {
                Move(tr, 0, -fallSpeed, OutOfBounds.Clamp);

                // Reactivate scroll and multiplier when falling (we need to do that if the player is stuck behind a wall)
                FightHandler.ToggleScroll(true);

                return true; // Object is still falling -> we return true
            }

            // If there's a platform less than <fallspeed> steps below -> fall the remaining steps 
            else
            {
                // Compute remaining steps
                int stepsDown = Mathf.FloorToInt(hit.distance / ((FightHandler.gheight) / (float)FightHandler.gridystep));

                if (stepsDown > 0)
                {
                    Move(tr, 0, -stepsDown);

                    // Reactivate scroll and multiplier when falling (we need to do that if the player is stuck behind a wall)
                    FightHandler.ToggleScroll(true);
                }
                return false; // Object is now grounded -> we return false
            }
        }
        return false;
    }

    // Snap an object to the nearest point on the grid
    public static void SnapToGrid(Transform tr)
    {
        float scaleX = FightHandler.gwidth / (float)FightHandler.gridxstep;
        float scaleY = FightHandler.gheight / (float)FightHandler.gridystep;
        
        // Also scale the object to one unit of the grid
        tr.localScale = new Vector3(scaleX, scaleY, 5f);

        float posx = Mathsfs.FloatModulus(tr.position.x, scaleX);

        if (posx < scaleX / 2f)
        {
            tr.position -= posx * Vector3.right;
        }
        else
        {
            tr.position += (scaleX - posx) * Vector3.right;
        }

        float posy = Mathsfs.FloatModulus(tr.position.y, scaleY);

        if (posy < scaleY / 2f)
        {
            tr.position -= posy * Vector3.up;
        }
        else
        {
            tr.position += (scaleY - posy) * Vector3.up;
        }
    }
}
