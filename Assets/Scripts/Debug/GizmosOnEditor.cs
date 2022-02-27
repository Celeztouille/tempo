using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that draws the grid on the editor
// White : in the camera range
// Blue : out of the camera range
// Red : one line out of 4 => mark beats
[ExecuteAlways]
public class GizmosOnEditor : MonoBehaviour
{
    private void OnDrawGizmos()
    {

        for (int i = 0; i < FightHandler.gridxstep + 1; ++i)
        {
            Vector3 startLine = FightHandler.gwidth * ((float)i / FightHandler.gridxstep) * Vector3.right;
            if (i%4 == 0)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(startLine, startLine + FightHandler.gheight * Vector3.up);
            Gizmos.color = Color.white;
        }

        for (int i = 0; i < FightHandler.gridystep + 1; ++i)
        {
            Vector3 startLine = FightHandler.gheight * ((float)i / FightHandler.gridystep) * Vector3.up;
            Gizmos.DrawLine(startLine, startLine + FightHandler.gwidth * Vector3.right);

            Gizmos.color = Color.blue;
            Gizmos.DrawLine(startLine + FightHandler.gwidth * Vector3.right, startLine + 10000f * Vector3.right);
            Gizmos.color = Color.white;
        }

        Gizmos.color = Color.blue;

        for (int i = FightHandler.gridxstep + 1; i < 2000; ++i)
        {
            Vector3 startLine = FightHandler.gwidth * ((float)i / FightHandler.gridxstep) * Vector3.right;
            if (i % 4 == 0)
            {
                Gizmos.color = Color.red;
            }
            Gizmos.DrawLine(startLine, startLine + FightHandler.gheight * Vector3.up);
            Gizmos.color = Color.blue;
        }
    }
}
