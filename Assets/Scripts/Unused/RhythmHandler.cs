using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmHandler : MonoBehaviour
{
    // Y-position of notes appearance (and right border of the camera)
    [SerializeField] [Range(10, 150)] private int appearancePoint;

    // Appearance time of a note on screen (in number of beats)
    [SerializeField] [Range(1, 20)] private int beatsOnScreen;

    // Offset between two lanes of notes
    [SerializeField] [Range(0f, 10f)] private float verticalOffset = 0f;

    // Max distance to hit a note
    [SerializeField] [Range(1f, 30f)] private float actionWindow = 0f;

    // Max distance to hit a note behind the action bar
    [SerializeField] [Range(1f, 10f)] private float actionBackWindow = 0f;

    // Actual note speed
    [HideInInspector] public static float noteSpeed = 0f;

    // Static variables to makes some parameters easily accessible by other scripts
    public static float vOffset;
    public static float aWindow;
    public static float aBackWindow;
    public static float rightmostPoint;

    // Map static variables
    private void Awake()
    {
        vOffset = verticalOffset;
        aWindow = actionWindow;
        aBackWindow = actionBackWindow;
        rightmostPoint = appearancePoint;
    }

    // Compute note speed based on the distance to travel, appearance time and clock period
    public void SetNoteSpeed() => noteSpeed = (float)appearancePoint / ((beatsOnScreen - 1) * InternalClock.GetPeriod());

    // Update the note speed when tempo is changing
    void Start()
    {
        SetNoteSpeed();
        InternalClock.clockUpdateEvent.AddListener(SetNoteSpeed);
    }
}
