using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MoveCamera : MonoBehaviour
{
    // List of possible camera positions
    [SerializeField] private List<Transform> camPoints;

    // Duration of the translation
    [SerializeField] [Range(0f, 5f)] private float translateDuration = 0.5f;

    // Duration of the rotation
    [SerializeField] [Range(0f, 5f)] private float rotateDuration = 0.8f;


    // Used by SmoothDamp
    private Vector3 refPos;

    Transform target;
    Quaternion fromRot; // Store the current rotation for the rotation lerp

    float rotTimer = 0f; // Used by rotation lerp

    private void Start()
    {
        target = transform;
        fromRot = transform.rotation;
    }

    void Update()
    {
        // SmoothDamp the position
        Vector3 pos = Vector3.SmoothDamp(transform.position, target.position, ref refPos, translateDuration);
        transform.position = pos;

        // Slerp the rotation
        transform.rotation = Quaternion.Slerp(fromRot, target.rotation, rotTimer / rotateDuration);
        rotTimer += Time.deltaTime;
    }

    // Set target position
    public void Move(int i)
    {
        if (i >= camPoints.Count)
        {
            Debug.LogError("MoveCamera.Move() : index is out of range");
        }
        else
        {
            target = camPoints[i];
            fromRot = transform.rotation;
            rotTimer = 0f;
        }
    }


    // DEBUG FUNCTION
    public void InputPressed(InputAction.CallbackContext context)
    {
        if (context.performed)  // Equivalent to Input.GetKeyDown()
        {
            if (context.action.name == "Debug1")
            {
                Move(0);
            }
            if (context.action.name == "Debug2")
            {
                Move(1);
            }
            if (context.action.name == "Debug3")
            {
                Move(2);
            }
        }
    }
}
