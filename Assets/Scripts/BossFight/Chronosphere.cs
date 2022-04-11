using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chronosphere : MonoBehaviour
{

    [SerializeField] private float timeSaved = 5f;

    private void Start()
    {
        BossGrid.SnapToGrid(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            DisplayTimer.SaveTime(timeSaved);
            Destroy(gameObject);
        }
    }
}
