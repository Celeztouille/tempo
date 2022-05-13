using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashingShader : MonoBehaviour
{

    Material[] mats;
    List<Vector3> initColors; // in HSV
    List<Vector3> initRimColors; // in HSV
    Vector3 flashColor; // in HSV
    Vector3 flashRimColor; // in HSV
    List<Vector3> currentColors; // in HSV
    List<Vector3> currentRimColors; // in HSV


    Vector3 currentVelocity; // ref for the smoothdamp
    Vector3 currentRimVelocity; // ref for the smoothdamp

    public static bool firstFlash = true;


    private void Awake()
    {
        initColors = new List<Vector3>();
        initRimColors = new List<Vector3>();
        currentColors = new List<Vector3>();
        currentRimColors = new List<Vector3>();

        mats = GetComponent<MeshRenderer>().materials;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i=0; i < mats.Length; ++i)
        {
            Color init = mats[i].GetColor(Shader.PropertyToID("_ShadowColor"));
            Color initRim = mats[i].GetColor(Shader.PropertyToID("_RimColor"));

            // Init the current color to the initial color;
            float hue, sat, val;
            Color.RGBToHSV(init, out hue, out sat, out val);

            currentColors.Add(new Vector3(hue, sat, val));
            initColors.Add(new Vector3(hue, sat, val));


            // Init the current rim color to the initial color;
            Color.RGBToHSV(initRim, out hue, out sat, out val);

            currentRimColors.Add(new Vector3(hue, sat, val));
            initRimColors.Add(new Vector3(hue, sat, val));
        }


        InternalClock.beatEvent.AddListener(BeatUpdate);
    }

    void BeatUpdate()
    {
        if (!firstFlash)
        {
            for (int i = 0; i < mats.Length; ++i)
            {
                float h = initColors[i].x;
                float s = initColors[i].y;
                float v = initColors[i].z;
                float hr = initRimColors[i].x;
                float sr = initRimColors[i].y;
                float vr = initRimColors[i].z;



                switch (Score.GetMultiplier())
                {
                    case 1:
                        h = Mathf.Clamp(h + Random.Range(-0.3f, 3f), 0f, 1f);
                        v = Mathf.Clamp(v + 0.5f, 0f, 1f);
                        flashColor = new Vector3(h, s + 0.2f, v);
                        hr = Mathf.Clamp(hr + Random.Range(-0.3f, 3f), 0f, 1f);
                        flashRimColor = new Vector3(hr, sr, vr);
                        break;
                    case 2:
                        h = Mathf.Clamp(h + Random.Range(-0.3f, 3f), 0f, 1f);
                        v = Mathf.Clamp(v + 0.6f, 0f, 1f);
                        flashColor = new Vector3(h, s + 0.3f, v);
                        hr = Mathf.Clamp(hr + Random.Range(-0.4f, 4f), 0f, 1f);
                        flashRimColor = new Vector3(hr, sr, vr);
                        break;
                    default:
                        flashColor = new Vector3(Random.Range(0, 1), 0.8f, 1f);
                        flashRimColor = new Vector3(Random.Range(0, 1), sr, vr);
                        break;
                }

                if (Score.GetMultiplier() < 4)
                {
                    flashColor.z = 0.5f;
                    flashRimColor.z = 0.5f;
                }

                currentColors[i] = flashColor;
                currentRimColors[i] = flashRimColor;
            }
        }
        else
        {
            firstFlash = false;
        }

    }

    private void Update()
    {
       for (int i=0; i<mats.Length; ++i)
        {
            currentColors[i] = Vector3.SmoothDamp(currentColors[i],
                                                  initColors[i],
                                                  ref currentVelocity,
                                                  InternalClock.GetPeriod() / 1.3f);

            currentRimColors[i] = Vector3.SmoothDamp(currentRimColors[i],
                                                     initRimColors[i],
                                                     ref currentRimVelocity,
                                                     InternalClock.GetPeriod() / 1.3f);

            float h = currentColors[i].x;
            float s = currentColors[i].y;
            float v = currentColors[i].z;
            float hr = currentRimColors[i].x;
            float sr = currentRimColors[i].y;
            float vr = currentRimColors[i].z;

            mats[i].SetColor(Shader.PropertyToID("_ShadowColor"), Color.HSVToRGB(h, s, v));
            mats[i].SetColor(Shader.PropertyToID("_RimColor"), Color.HSVToRGB(hr, sr, vr));
        }
    }
}
