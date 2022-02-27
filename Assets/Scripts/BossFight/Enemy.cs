using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Speed of the enemy (relative to the global speed)
    [SerializeField] [Range(0, 10)] int speed;

    // Fall speed in steps per tick (gravity)
    [SerializeField] [Range(1, 5)] int fallSpeed;

    void Start() => InternalClock.tickEvent.AddListener(TickUpdate);

    // Move enemy on each clock tick
    void TickUpdate()
    {
        BossGrid.Move(transform, -(FightHandler.globalSpeed + speed), 0, BossGrid.OutOfBounds.Destroy);
        BossGrid.Fall(transform, fallSpeed);
    }

}
