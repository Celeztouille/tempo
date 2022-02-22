using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ScanInputs : MonoBehaviour
{
    // Reference to Multiplier script (contains action windows and timestamps)
    [SerializeField] private Multiplier multiplier;

    public enum InputType
    {
        Jump
    }

    // Script containing the player actions to trigger (jump, fire, parry)
    private PlayerActions playerAction;

    // Map playerAction
    private void Awake() => playerAction = GameObject.Find("Avatar").GetComponent<PlayerActions>();

    // Check if we hit the input at the right timing
    private void HitInput(InputType type)
    {

        float period = InternalClock.GetPeriod();

        // Measure time difference between timebeat and input
        float delta = Time.fixedTime - multiplier.timeBeat;
        if (delta > period / 2f)
        {
            delta -= period;
        }

        // if we hit
        if (Mathf.Abs(delta) < multiplier.goodWdw)
        {

            switch (type)
            {
                case InputType.Jump:
                    playerAction.Jump();
                    break;
                default:
                    break;
            }
        }
    }

    // Input Listener : check if an input was pressed, determines which key is pressed and call HitInput()
    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "Jump")
            {
                HitInput(InputType.Jump);
            }
        }
    }
}
