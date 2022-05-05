using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [SerializeField] private List<int> rows;
    [SerializeField] private GameObject projectile;

    // projectiles will spawn <referenceOffset> units behind the spawner trigger
    private int referenceOffset = 10;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            foreach (int row in rows)
            {
                Vector3 pos = new Vector3(transform.position.x - referenceOffset * (FightHandler.gwidth / (float)FightHandler.gridxstep),
                                          row * (FightHandler.gheight / (float)FightHandler.gridystep),
                                          0f);

                GameObject instance = Instantiate(projectile, pos, Quaternion.identity);
                BossGrid.SnapToGrid(instance.transform);

                Destroy(gameObject);
            }
        }
    }
}
