using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Solid : MonoBehaviour
{
    void Start() => InternalClock.tickEvent.AddListener(TickUpdate);

    // Move all solid elements at the global scroll speed each tick
    void TickUpdate()
    {
        BossGrid.Move(transform, -FightHandler.globalSpeed, 0, BossGrid.OutOfBounds.DestroyAfter);
    }

}
