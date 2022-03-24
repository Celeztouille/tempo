using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour for walls
public class MovingItem : MonoBehaviour
{
    private float smoothTime = 0.2f;

    // Used for smoothing movements
    private Vector3 goalPos;
    private Vector3 refVelocity;

    void Start()
    {

        BossGrid.SnapToGrid(transform);// Snap all spikes on start
        InternalClock.beatEvent.AddListener(TickUpdate);
        goalPos = transform.position;
    }

    // Use the update to smooth movement
    private void Update() => transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref refVelocity, smoothTime);


    // Move all spike elements at the global scroll speed each tick
    void TickUpdate()
    {
        SmoothMove(-FightHandler.globalSpeed, 0);
    }

    private void SmoothMove(int x, int y)
    {
        // Update the goal position of the object
        goalPos = transform.position + new Vector3(x * (FightHandler.gwidth / (float)FightHandler.gridxstep),
                                                   y * (FightHandler.gheight / (float)FightHandler.gridystep),
                                                   0f);
    }
}
