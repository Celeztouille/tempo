using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualBeat : MonoBehaviour
{
    // Avatar size in percentage of quad size (actually quad size is 2.5 so avatar size is 100% / 2.5 = 40%)
    [SerializeField] [Range(0f, 1f)] private float avatarSize = 0.4f;

    // Duration of a beat
    private float smoothTime;

    // Used for the Mathf.SmoothDamp function
    private float refVelocity;

    // Material of the gameobject
    private Material mat;

    private bool startSmash = false, isSmashing = false;

    void Start()
    {
        // Don't forget to update the smoothTime value when changing bpm
        SetSmoothTime();
        InternalClock.clockUpdateEvent.AddListener(SetSmoothTime);

        // Reset circle size each beat
        InternalClock.beatEvent.AddListener(ResetCircleSize);

        mat = GetComponent<MeshRenderer>().material;
    }

    // Just a simple SmoothDamp to get a smooth shrink of the circle
    void Update()
    {
        float size = Mathf.SmoothDamp(mat.GetFloat("_Size"), avatarSize / 2f, ref refVelocity, smoothTime);
        mat.SetFloat("_Size", size);
    }

    void SetSmoothTime()
    {
        smoothTime = InternalClock.GetPeriod();
    }
    
    void ResetCircleSize()
    {
        mat.SetFloat("_Size", 1);

        if (mat.GetFloat("_Side") == 1f)
        {
            mat.SetFloat("_Side", -1f);
        }
        else
        {
            mat.SetFloat("_Side", 1f);
        }

        if (isSmashing)
        {
            mat.SetColor("_Color", Color.blue);
            isSmashing = false;
        }
        if (startSmash)
        {
            isSmashing = true;
            startSmash = false;
        }
    }

    public void ShowSmashing()
    {
        mat.SetColor("_Color", Color.red);
        startSmash = true;
    }

    public void SetBothSides(bool value)
    {
        if (value)
            mat.SetFloat("_BothSides", 1f);
        else
            mat.SetFloat("_BothSides", 0f);
    }
}
