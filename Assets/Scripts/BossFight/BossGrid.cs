using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class handling all kinds of movements within the grid
public class BossGrid
{

    // Possible behaviours when an object is out of bounds
    public enum OutOfBounds
    {
        Allow,
        Clamp,
        Destroy,
        DestroyAfter
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
        // Second option : clamp the movement to stick at the bound of the grid
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
        if (tr.position.y > 0)
        {

            Ray ray = new Ray(tr.position, Vector3.down);
            RaycastHit hit;

            Physics.Raycast(ray, out hit, FightHandler.gheight / (float)FightHandler.gridystep, LayerMask.GetMask("Solid"));

            // DEBUG : Draw plateform detector
            Debug.DrawRay(tr.position, Vector3.down * FightHandler.gheight / (float)FightHandler.gridystep, Color.yellow, 0.2f);
            // DEBUG

            if (hit.collider == null)
            {
                Move(tr, 0, -fallSpeed, OutOfBounds.Clamp);
                return true;
            }
            return false;
        }
        return false;
    }
}
