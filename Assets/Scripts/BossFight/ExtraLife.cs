using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLife : MonoBehaviour
{
    [SerializeField] [Range(1, 4)] private int neededMultiplier = 2;
    [SerializeField] [Range(1, 5)] private int stepsRight = 1;

    private void Start()
    {
        BossGrid.SnapToGrid(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            if (Score.GetMultiplier() >= neededMultiplier)
            {
                BossGrid.Move(other.transform, stepsRight, 0);
            }
            Destroy(gameObject);
        }
    }
}
