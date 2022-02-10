using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// Debug class to spawn enemies, walls and projectiles with debug keys
public class SpawnHazards : MonoBehaviour
{
    [SerializeField] private GameObject solid, enemy, projectile;

    // Wall spawner
    public void SpawnSolid(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(solid, new Vector3(FightHandler.gwidth, 0f, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth-5f, 0f, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth-10f, 0f, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth-15f, 0f, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth-20f, 0f, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth-25f, 0f, 1000f), Quaternion.identity);

            Instantiate(solid, new Vector3(FightHandler.gwidth, FightHandler.gheight / (float)FightHandler.gridystep, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 5f, FightHandler.gheight / (float)FightHandler.gridystep, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 10f, FightHandler.gheight / (float)FightHandler.gridystep, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 15f, FightHandler.gheight / (float)FightHandler.gridystep, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 20f, FightHandler.gheight / (float)FightHandler.gridystep, 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 25f, FightHandler.gheight / (float)FightHandler.gridystep, 1000f), Quaternion.identity);
            
            Instantiate(solid, new Vector3(FightHandler.gwidth, 2 * (FightHandler.gheight / (float) FightHandler.gridystep), 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 5f, 2 * (FightHandler.gheight / (float)FightHandler.gridystep), 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 10f, 2 * (FightHandler.gheight / (float)FightHandler.gridystep), 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 15f, 2 * (FightHandler.gheight / (float)FightHandler.gridystep), 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 20f, 2 * (FightHandler.gheight / (float)FightHandler.gridystep), 1000f), Quaternion.identity);
            Instantiate(solid, new Vector3(FightHandler.gwidth - 25f, 2 * (FightHandler.gheight / (float)FightHandler.gridystep), 1000f), Quaternion.identity);
        }
    }

    // Enemy spawner
    public void SpawnEnemy(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(enemy, new Vector3(FightHandler.gwidth, 0f, 1000f), Quaternion.identity);
        }
    }

    // Projectile spawner
    public void SpawnProjectile(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Instantiate(projectile, new Vector3(FightHandler.gwidth, 0f, 1000f), Quaternion.identity);
        }
    }
}
