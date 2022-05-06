using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootItem : MonoBehaviour
{
    [SerializeField] private GameObject friendlyProjectile;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Instantiate(friendlyProjectile, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
