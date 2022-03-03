using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class that handles the healthbar of the player
public class PlayerLives : MonoBehaviour
{
    // Initial number of lives
    [SerializeField] [Range(1, 10)] private static int lives = 3;

    [SerializeField] private GameObject livesGO;

    // Components to bind 
    private static TextMeshProUGUI livesText;
    private static InvincibilityFrames invincibilityFrames;

    // Bind compononents
    private void Awake()
    {
        livesText = livesGO.GetComponent<TextMeshProUGUI>();
        invincibilityFrames = GetComponent<InvincibilityFrames>();
        DisplayLives();
    }

    // Toggle to update the number of lives, can also activate invincibility frames if needed
    public static void AddLife(int value, bool activateInvincibilityFrames = true)
    {
        // If we try to substract a life and avatar is invincible : do nothing
        if (value > 0 || InvincibilityFrames.state == InvincibilityFrames.State.Vulnerable)
        {
            lives += value;

            // Play spikes sound effect
            FMODUnity.RuntimeManager.PlayOneShot("event:/World/Spikes");
        }

        if (activateInvincibilityFrames)
        {
            invincibilityFrames.ActivateInvincibility();
        }

        // Update UI
        DisplayLives();
    }

    // Update display of remaining lives
    public static void DisplayLives() => livesText.text = "Lives : " + lives.ToString();
}
