using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Handle invincibility time after a hit
public class InvincibilityFrames : MonoBehaviour
{
    public enum State
    {
        Vulnerable,
        Invincible,
    }

    // Is the player in the invincibility state or not
    [HideInInspector] public static State state = State.Vulnerable;

    // Duration of the invincibility state
    [SerializeField] [Range(0.1f, 5f)] float duration;
    
    // Player material
    private Material mat;

    // Internal script timer
    private float timer = 0f;

    // Bind player material on start
    void Start() => mat = GetComponent<MeshRenderer>().sharedMaterial;

    // Exit invincibility state after <duration> seconds
    void Update()
    {
        if (timer > duration)
        {
            state = State.Vulnerable;
            mat.SetColor("_Color", Color.green);
        }
        timer += Time.deltaTime;
    }

    // Change state and material + reset internal timer when activating invincibility
    public void ActivateInvincibility()
    {
        state = State.Invincible;
        timer = 0f;

        mat.SetColor("_Color", Color.yellow);
    }
}
