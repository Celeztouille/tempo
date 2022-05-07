using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Class that display level time
public class DisplayTimer : MonoBehaviour
{
    private TextMeshProUGUI timerText;

    private static float timer = 0f;
    private static bool hasStarted = false; // Timer starts when player is giving signal

    // Bind Text object
    void Start() => timerText = GetComponent<TextMeshProUGUI>();
    
    // Toggle to start the timer
    public static void StartTimer()
    {
        hasStarted = true;
        timer = 0f;
    }

    public static void StopTimer()
    {
        hasStarted = false;
    }

    public static void SaveTime(float time)
    {
        timer -= time;
    }

    void Update()
    {
        if (hasStarted)
        {
            timerText.text = timer.ToString();
        }
        else
        {
            timerText.text = "";
        }
        
        timer += Time.deltaTime;
    }
}
