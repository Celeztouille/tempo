using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    // Speed of the projectile (relative to the global speed)
    [SerializeField] [Range(1, 10)] int speed;

    void Start() => InternalClock.tickEvent.AddListener(TickUpdate);

    // Move projectile on each clock tick
    void TickUpdate()
    {
        BossGrid.Move(transform, -(FightHandler.globalSpeed + speed), 0, BossGrid.OutOfBounds.Destroy);
    }
}
