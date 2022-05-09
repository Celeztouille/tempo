using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class Death : MonoBehaviour
{
    public static bool isDead = false;

    [SerializeField] private GameObject deathScreen;
    [SerializeField] private TextMeshProUGUI scoreText, perfText, goodText, missText, progressText;

    public static int lives = 5;

    private DeathTagHandler deathTagHandler;
    [SerializeField] private PlayerActions playerActions;
    [SerializeField] private MoveCameraManager cameraManager;
    [SerializeField] private MoveCamera moveCamera;

    private VisualBeat visualBeat;


    void Start()
    {
        visualBeat = GameObject.Find("VisualBeat").GetComponent<VisualBeat>();
        deathTagHandler = GameObject.Find("DeathTagsManager").GetComponent<DeathTagHandler>();
        InternalClock.beatEvent.AddListener(CheckIfDead);
    }

    // Update is called once per frame
    void CheckIfDead()
    {
        if (lives == 0)
        {
            deathTagHandler.AddTag();
            Music.StopMusic();
            InternalClock.SetPeriod(10000f, InternalClock.ClockFormat.BeatPeriod, true);
            UpdateDeathScreen();
        }
    }

    void UpdateDeathScreen()
    {
        float progress = 100f * (InternalClock.beatsCount / (float)150);

        scoreText.text = "Your score : " + Score.GetScore().ToString();
        perfText.text = Multiplier.perfectCpt.ToString();
        goodText.text = Multiplier.goodCpt.ToString();
        missText.text = Multiplier.missCpt.ToString();
        progressText.text = "Progress : " + progress.ToString() + "%";
        DisplayTimer.StopTimer();
        visualBeat.SetBothSides(false);
        Multiplier.needSteps = true;

        isDead = true;

        deathScreen.SetActive(true);
    }

    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "Restart")
            {
                if (isDead)
                {
                    deathScreen.SetActive(false);

                    Score.SetMultiplier(1);
                    Score.ResetScore();
                    Multiplier.goodCpt = 0;
                    Multiplier.missCpt = 0;
                    Multiplier.perfectCpt = 0;
                    InternalClock.beatsCount = 0;
                    lives = 5;

                    cameraManager.ResetCamPosition();
                    moveCamera.Move(0);
                    playerActions.ResetPosition();
                    StartRhythm.ReloadRhythm();
                }
            }

            if (context.action.name == "Debug6")
            {
                lives = 0;
            }
        }
    }
}
