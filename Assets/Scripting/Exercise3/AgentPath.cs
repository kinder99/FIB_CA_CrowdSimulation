using System.Collections.Generic;
using UnityEngine;

public class AgentPath : MonoBehaviour
{
    public Collider plane;

    public Vector2 goal;
    public bool moveAgent = true;

    private List<GridCell> path = null;
    private Grid_A_Star gridAStar = null;
    private GridCell startNode = null;

    // For gizmo visualization
    private float assignedColorHue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GridInitialize gridInitialize = GridInitialize.instance;
        if (gridInitialize)
        {
            moveAgent = (ComputePath() > 0);
            goal = GetNextWaypoint();
        }
        else
        {
            plane = GameObject.FindObjectOfType<CrowdGenerator>().plane;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
