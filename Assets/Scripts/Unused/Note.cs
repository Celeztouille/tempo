using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour
{
    private GameObject noteDestroyer;

    // Type of Notes
    public enum Type
    {
        Up,
        Mid,
        Down,
    }

    private void Start()
    {
        noteDestroyer = GameObject.Find("NoteDestroyer");
    }

    void Update() => Move();

    // Function to scroll the note on screen at the right speed
    private void Move() => transform.position += RhythmHandler.noteSpeed * Time.deltaTime * Vector3.left;

    // Destroy note when touching the destroyer
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.name == "NoteDestroyer")
        {
            Destroy(gameObject);
        }
    }
}
