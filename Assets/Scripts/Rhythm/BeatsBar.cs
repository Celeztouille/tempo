using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Main class for the tempo bar below the level
public class BeatsBar : MonoBehaviour
{
    // Toggle spacing between two beats : <beatsOnScreen> beats will appear in a window of <appearancePoint> - 5 meters
    [SerializeField] [Range(10, 350)] private int appearancePoint;
    [SerializeField] [Range(1, 20)] private int beatsOnScreen;

    // Total number of beats to reach the end of the song
    [SerializeField] private int beatsAmount;

    // Beat bar prefab
    [SerializeField] private GameObject beat;

    // Actual note speed
    [HideInInspector] private float noteSpeed = 0f;

    // Internal counter
    private int beatsCount = 0;


    // Compute note speed based on the distance to travel, appearance time and clock period
    void SetNoteSpeed() => noteSpeed = (appearancePoint / beatsOnScreen) / InternalClock.GetPeriod();

    void Move() => transform.position += noteSpeed * Time.deltaTime * Vector3.left;


    // Manually calibrate the position of the beats bar every beat to keep it synchronised with the music
    void Calibrate()
    {
        transform.position = beatsCount * appearancePoint / beatsOnScreen * Vector3.left;
        beatsCount++;
    }

    private void Update() => Move();
      
    private void Start()
    {
        SetNoteSpeed();
        InternalClock.clockUpdateEvent.AddListener(SetNoteSpeed); // Update note speed every time we change clock period
        InternalClock.beatEvent.AddListener(Calibrate); // Recalibrate every beat


        // Instantiate all beat bars

        Vector3 firstPosition = 5f * Vector3.right - 2f * Vector3.forward + 10f * Vector3.down;
        Vector3 posShift = appearancePoint / beatsOnScreen * Vector3.right;

        for (int i=0; i<beatsAmount; ++i)
        {
            Instantiate(beat, firstPosition + i * posShift, Quaternion.identity, transform);
        }
    }

}
