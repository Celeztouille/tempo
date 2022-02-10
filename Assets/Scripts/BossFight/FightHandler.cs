using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightHandler : MonoBehaviour
{
    // Grid parameters

    // Number of steps on the x-axis
    [SerializeField] [Range(0, 500)] public int horizontalGridStep;

    // Number of steps on the y-axis
    [SerializeField] [Range(0, 50)] public int verticalGridStep;

    // Global width of the grid
    [SerializeField] [Range(20, 200)] public int gridWidth;

    // Global height of the grid
    [SerializeField] [Range(10, 100)] public int gridHeight;

    // Speed of the auto-scroll
    [SerializeField] [Range(1, 10)] public int scrollSpeed;

    // Static variables to makes some parameters easily accessible by other scripts
    public static int gridxstep;
    public static int gridystep;
    public static int gwidth;
    public static int gheight;
    public static int globalSpeed;

    // Map static variables
    private void Awake()
    {
        gridxstep = horizontalGridStep;
        gridystep = verticalGridStep;
        gwidth = gridWidth;
        gheight = gridHeight;
        globalSpeed = scrollSpeed;
    }

    void Start()
    {
        ShowCorners();
    }

    // Debug function to show the extremities of the grid
    void ShowCorners()
    {
        GameObject.Find("TopLeft").transform.position = new Vector3(0f, gheight, 1000f);
        GameObject.Find("TopRight").transform.position = new Vector3(gwidth, gheight, 1000f);
        GameObject.Find("BottomLeft").transform.position = new Vector3(0f, 0f, 1000f);
        GameObject.Find("BottomRight").transform.position = new Vector3(gwidth, 0f, 1000f);
    }
}
