using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour for spikes
public class Spike : MonoBehaviour
{

    // Detect collision with player -> Reset multiplier and speed and make the player lose a life
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerLives.AddLife(-1);

            Score.SetMultiplier(1);
            Music.ResetBPM();
        }
    }
}
