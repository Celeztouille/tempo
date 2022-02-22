using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpawner : MonoBehaviour
{
    [SerializeField] private GameObject note;

    private void Start() => InternalClock.beatEvent.AddListener(InstantiateRandom);

    // Spawn a random note each beat
    void InstantiateRandom()
    {
        Vector3 pos = RhythmHandler.rightmostPoint * Vector3.right;

        switch (Random.Range(0, 3))
        {
            case 0:
                pos += RhythmHandler.vOffset * Vector3.up;
                break;
            case 1:
                pos -= RhythmHandler.vOffset * Vector3.up;
                break;
            default:
                break;
        }

        Instantiate(note, pos, Quaternion.identity, transform);
    }
}
