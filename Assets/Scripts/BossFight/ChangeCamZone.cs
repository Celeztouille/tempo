using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamZone : MonoBehaviour
{
    [SerializeField] [Range(1, 4)] private int neededMultiplier = 2;
    [SerializeField] [Range(1, 5)] private int stepsRight = 1;
    [SerializeField] [Range(0, 2)] private int camPosition;

    private MoveCamera moveCamera;

    // Start is called before the first frame update
    void Start()
    {
        moveCamera = GameObject.Find("FirstCam").GetComponent<MoveCamera>();
        BossGrid.SnapToGrid(transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            moveCamera.Move(camPosition);

            // Deactivate steps mechanics on camera position 1 and 2, reactivate it on position 0
            if (camPosition != 0)
            {
                Multiplier.needSteps = false;
            }
            else
            {
                Multiplier.needSteps = true;
            }

            // Gain extra life with multiplier
            if (Score.GetMultiplier() >= neededMultiplier)
            {
                BossGrid.Move(other.transform, stepsRight, 0);
            }

            Destroy(gameObject);
        }
    }
}
