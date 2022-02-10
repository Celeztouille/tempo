using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Absolute speed of the projectile (non relative to auto-scroll speed)
    [SerializeField] [Range(1, 5)] private int speed;

    // Number of ticks before projectile is destructed
    [SerializeField] [Range(10, 100)] private int lifetime;

    // Local tick counter
    private int tickCount = 0;

    void Start() => InternalClock.tickEvent.AddListener(TickUpdate);

    // Move projectile on each tick and destroy it when lifetime reached
    void TickUpdate()
    {
        BossGrid.Move(transform, speed, 0);
        CheckIfKilledEnemy();

        if (tickCount > lifetime)
        {
            Destroy(gameObject);
        }
        tickCount++;
    }


    // Kill enemy when projectile is colliding with one
    // Because every movement is discrete, it is possible that the collision do not occur at all
    // It is logically impossible for a projectile to be behind an enemy,
    // if it's the case, it means that the projectile has hit the enemy
    // so it will be our condition to check collision
    private void CheckIfKilledEnemy()
    {
        Ray ray = new Ray(transform.position, Vector3.left);
        RaycastHit hit;

        Physics.Raycast(ray, out hit, FightHandler.gwidth, LayerMask.GetMask("Enemy"));

        if (hit.collider != null)
        {
            Destroy(hit.collider.gameObject);
            Destroy(gameObject);
        }
    }
}
