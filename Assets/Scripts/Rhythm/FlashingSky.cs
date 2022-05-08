using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingSky : MonoBehaviour
{
    Vector3 initColor; // in HSV
    Vector3 flashColor; // in HSV
    Vector3 currentColor; // in HSV


    Vector3 currentVelocity; // ref for the smoothdamp


    // Start is called before the first frame update
    void Start()
    {
        Color init = Camera.main.backgroundColor;

        // Init the current color to the initial color;
        float hue, sat, val;
        Color.RGBToHSV(init, out hue, out sat, out val);

        currentColor = new Vector3(hue, sat, val);
        initColor = new Vector3(hue, sat, val);

        InternalClock.beatEvent.AddListener(BeatUpdate);
    }

    void BeatUpdate()
    {
        float h = initColor.x;
        float s = initColor.y;
        float v = initColor.z;

        switch (Score.GetMultiplier())
        {
            case 1:
                flashColor = new Vector3(h, s, v);
                break;
            case 2:
                h = Mathf.Clamp(h + Random.Range(-0.2f, 2f), 0f, 1f);
                v = Mathf.Clamp(v + 0.4f, 0f, 1f);
                flashColor = new Vector3(h, s + 0.1f, v);
                break;
            case 3:
                h = Mathf.Clamp(h + Random.Range(-0.3f, 3f), 0f, 1f);
                v = Mathf.Clamp(v + 0.5f, 0f, 1f);
                flashColor = new Vector3(h, s + 0.2f, v);
                break;
            case 4:
                h = Mathf.Clamp(h + Random.Range(-0.3f, 3f), 0f, 1f);
                v = Mathf.Clamp(v + 0.6f, 0f, 1f);
                flashColor = new Vector3(h, s + 0.3f, v);
                break;
            default:
                flashColor = new Vector3(Random.Range(0, 1), 0.8f, 1f);
                break;
        }

        currentColor = flashColor * 0.4f + currentColor * 0.6f;
    }

    private void Update()
    {

        currentColor = Vector3.SmoothDamp(currentColor,
                                          initColor,
                                          ref currentVelocity,
                                          InternalClock.GetPeriod() / 2f);

        float h = currentColor.x;
        float s = currentColor.y;
        float v = currentColor.z;

        Camera.main.backgroundColor = Color.HSVToRGB(h, s, v);
    }
}
