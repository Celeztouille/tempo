using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Script that handles the HitFeedback shader | tell the player if beat was perfect, good or missed
public class HitFeedback : MonoBehaviour
{
    // Fadeout time for the display
    [SerializeField] [Range(0.1f, 2f)] float fadeOutTime = 0.3f;

    public enum Precision
    {
        Perfect,
        Good,
        Miss
    }

    private Precision precision = Precision.Miss;

    // Material of the displayed object
    private Material mat;

    // Needed for the smooth damping functions
    private float currentSliderValuePerfect = 0f;
    private float currentSliderValueGood = 0f;
    private float currentSliderValueMiss = 0f;
    private float refVelocityPerfect;
    private float refVelocityGood;
    private float refVelocityMiss;

    // Bind material
    void Start() => mat = GetComponent<MeshRenderer>().sharedMaterial;


    // Smoothdamp the shader sliders to reach 0
    void Update()
    {
        currentSliderValuePerfect = Mathf.SmoothDamp(currentSliderValuePerfect, 0f, ref refVelocityPerfect, fadeOutTime);
        currentSliderValueGood = Mathf.SmoothDamp(currentSliderValueGood, 0f, ref refVelocityGood, fadeOutTime);
        currentSliderValueMiss = Mathf.SmoothDamp(currentSliderValueMiss, 0f, ref refVelocityMiss, fadeOutTime);

        mat.SetFloat("_SliderPerfect", currentSliderValuePerfect);
        mat.SetFloat("_SliderGood", currentSliderValueGood);
        mat.SetFloat("_SliderMiss", currentSliderValueMiss);
    }

    // Instantanely set the corresponding slider to the value of 1 when hitting a note
    public void SetHitFeedback(Precision hit)
    {
        precision = hit;
        switch (precision)
        {
            case Precision.Perfect:
                currentSliderValuePerfect = 1f;
                break;
            case Precision.Good:
                currentSliderValueGood = 1f;
                break;
            case Precision.Miss:
                currentSliderValueMiss = 1f;
                break;
        }
    }
}
