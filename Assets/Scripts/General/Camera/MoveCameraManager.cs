using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraManager : MonoBehaviour
{

    [SerializeField] [Range(0f, 1f)] private float smoothTime = 0.2f;

    [SerializeField] private GameObject avatar;

    private Vector3 initialPos;

    // Used for smoothing movements
    public static Vector3 goalPos;
    private Vector3 refVelocity;

    private void Start()
    {
        initialPos = transform.position;
    }

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
        //transform.position = new Vector3(transform.position.x, avatar.transform.position.y, transform.position.z);
    }

    public void ResetCamPosition()
    {
        goalPos = initialPos;
    }
}
