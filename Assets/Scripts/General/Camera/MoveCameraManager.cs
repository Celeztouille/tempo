using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraManager : MonoBehaviour
{

    [SerializeField] [Range(0f, 1f)] private float smoothTime = 0.2f;

    // Used for smoothing movements
    private Vector3 goalPos;
    private Vector3 refVelocity;

    public void SmoothMove(int x, int y)
    {
        // Update the goal position of the object
        goalPos = transform.position + new Vector3(x * (FightHandler.gwidth / (float)FightHandler.gridxstep),
                                                   y * (FightHandler.gheight / (float)FightHandler.gridystep),
                                                   0f);
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref refVelocity, smoothTime);
    }
}
