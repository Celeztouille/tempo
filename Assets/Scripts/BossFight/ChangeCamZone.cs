using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeCamZone : MonoBehaviour
{

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
            Destroy(gameObject);
        }
    }
}
