using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class to make a basic render of the rhythm section with quads
public class RhythmRender : MonoBehaviour
{
    private GameObject split1, split2, startWindow, endWindow, hitUp, hitDown;

    private void Awake()
    {
        split1 = GameObject.Find("SeparationUp");
        split2 = GameObject.Find("SeparationDown");
        startWindow = GameObject.Find("StartWindow");
        endWindow = GameObject.Find("EndWindow");
        hitUp = GameObject.Find("HitNoteUp");
        hitDown = GameObject.Find("HitNoteDown");
    }

    private void Start() => SetupBackGroundRender();

    void SetupBackGroundRender()
    {
        split1.transform.position += RhythmHandler.vOffset / 2f * Vector3.up;
        hitUp.transform.position += RhythmHandler.vOffset * Vector3.up;
        split2.transform.position += RhythmHandler.vOffset / 2f * Vector3.down;
        hitDown.transform.position += RhythmHandler.vOffset * Vector3.down;
        startWindow.transform.position += RhythmHandler.aWindow * Vector3.right;
        endWindow.transform.position -= RhythmHandler.aBackWindow * Vector3.right;
    }
}
