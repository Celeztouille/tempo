using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSign : MonoBehaviour
{
    private int sign = -1;
    private float speed;
    [SerializeField] private Transform cursor;

    // Start is called before the first frame update
    void Start()
    {
        InternalClock.beatEvent.AddListener(BeatUpdate);
        InternalClock.clockUpdateEvent.AddListener(TempoChange);
        speed = 1f / InternalClock.GetPeriod();
    }

    void BeatUpdate()
    {
        if (sign == 1f)
        {
            cursor.localPosition = new Vector3(0.5f, 0f, -0.005f);
        }
        else
        {
            cursor.localPosition = new Vector3(-0.5f, 0f, -0.005f);
        }
        sign *= -1;
    }

    void TempoChange()
    {
        speed = 1f / InternalClock.GetPeriod();
    }

    void Update()
    {
        cursor.localPosition += Vector3.right * sign * speed * Time.deltaTime;
    }
}
