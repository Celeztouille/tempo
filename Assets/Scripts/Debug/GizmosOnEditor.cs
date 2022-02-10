using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class that draws the grid when BossFightManager is selected on the editor
[ExecuteAlways]
public class GizmosOnEditor : MonoBehaviour
{
    private void OnDrawGizmosSelected()
    {
        FightHandler fight = GetComponentInChildren<FightHandler>();

        for (int i = 0; i < FightHandler.gridxstep + 1; ++i)
        {
            Vector3 startLine = 1000 * Vector3.forward + fight.gridWidth * ((float)i / fight.horizontalGridStep) * Vector3.right;
            Gizmos.DrawLine(startLine, startLine + fight.gridHeight * Vector3.up);
        }

        for (int i = 0; i < FightHandler.gridystep + 1; ++i)
        {
            Vector3 startLine = 1000 * Vector3.forward + fight.gridHeight * ((float)i / fight.verticalGridStep) * Vector3.up;
            Gizmos.DrawLine(startLine, startLine + fight.gridWidth * Vector3.right);
        }
    }
}
