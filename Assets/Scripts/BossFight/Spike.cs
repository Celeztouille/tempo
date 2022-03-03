using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// MonoBehaviour for spikes
public class Spike : MonoBehaviour
{
    void Start()
    {
        BossGrid.SnapToGrid(transform);// Snap all spikes on start
        InternalClock.tickEvent.AddListener(TickUpdate);
    }

    // Move all spike elements at the global scroll speed each tick
    void TickUpdate()
    {
        BossGrid.Move(transform, -FightHandler.globalSpeed, 0);
    }

    // Detect collision with player -> Reset multiplier and speed and make the player lose a life
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            PlayerLives.AddLife(-1);

            Score.SetMultiplier(1);
            Music.ResetBPM();

            // Play spikes sound effect
            FMODUnity.RuntimeManager.PlayOneShot("event:/World/Spikes");
        }
    }
}
