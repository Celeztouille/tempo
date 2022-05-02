using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] [Range(1, 3)] private int speed = 1;
    [SerializeField] [Range(10, 100)] private int lifetime = 30;

    [SerializeField] [Range(0f, 1f)] private float smoothTime = 0.2f;

    // Used for smoothing movements
    private Vector3 goalPos;
    private Vector3 refVelocity;

    private int beatCpt = 0;

    void Start()
    {
        InternalClock.beatEvent.AddListener(BeatUpdate);
        goalPos = transform.position;
    }

    void BeatUpdate()
    {
        if (beatCpt > lifetime)
        {
            Destroy(gameObject);
        }

        SmoothMove(FightHandler.globalSpeed + speed, 0);
        beatCpt++;
    }

    void SmoothMove(int x, int y)
    {
        // Update the goal position of the object
        goalPos += new Vector3(x * (FightHandler.gwidth / (float)FightHandler.gridxstep),
                               y * (FightHandler.gheight / (float)FightHandler.gridystep),
                               0f);
        goalPos = BossGrid.SnapToGrid(goalPos);
    }

    private void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, goalPos, ref refVelocity, smoothTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Score.SetMultiplier(1);
            Music.ResetBPM();
            Destroy(gameObject);
        }
    }
}
