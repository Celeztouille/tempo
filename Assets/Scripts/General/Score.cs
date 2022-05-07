using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class that handles score and score multiplier
public class Score : MonoBehaviour
{
    // Initial score and multiplier
    private static int score = 0;
    private static int multiplier = 1;

    // Max multiplier amount
    [SerializeField] private int maxMultiplier = 5;

    // Text game object for UI display
    [SerializeField] private GameObject scoreGO, multGO;

    // Create static variables
    private static TextMeshProUGUI scoreText, multText;
    private static int maxMultStatic;

    // Bind static variables to needed component
    private void Awake()
    {   
        scoreText = scoreGO.GetComponent<TextMeshProUGUI>();
        multText = multGO.GetComponent<TextMeshProUGUI>();
        DisplayScore();

        maxMultStatic = maxMultiplier;
    }

    // Add value to score (value is weighted by the multiplier)
    public static void AddToScore(int value)
    {
        score += value * multiplier; 

        // Update UI
        DisplayScore();
    }

    // Toggle to manually setup multiplier
    public static void SetMultiplier(int value)
    {
        multiplier = Mathf.Clamp(value, 1, maxMultStatic);

        // Update UI
        DisplayScore();
    }

    public static int GetMultiplier()
    {
        return multiplier;
    }

    // Toggle to increment the multiplier by a certain amount (default = 1)
    public static void IncrementMultiplier(int value = 1)
    {
        multiplier = Mathf.Clamp(multiplier + value, 1, maxMultStatic);

        // Update UI
        DisplayScore();
    }

    // Display score and multiplier on the UI
    private static void DisplayScore()
    {
        scoreText.text = score.ToString();
        multText.text = multiplier.ToString() + "x";
    }
}
