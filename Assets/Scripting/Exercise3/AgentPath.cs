using JetBrains.Annotations;
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
            plane = GameObject.FindFirstObjectByType<GridInitialize>().plane;

            Vector3 min = plane.bounds.min;
            Vector3 max = plane.bounds.max;
            goal = new Vector2(Random.Range(min.x,max.x),Random.Range(min.z, max.z));
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(gridAStar != null)
        {
            float reachedThreshold = GridInitialize.instance.grid.cellSize / 2f;
            if (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), goal) < reachedThreshold)
            {
                if (moveAgent && path.Count == 0)
                {
                    moveAgent = (ComputePath() > 0);
                }
                if (moveAgent && path != null && path.Count > 0)
                {
                    goal = GetNextWaypoint();
                }
            }
        }
        else
        {
            if(Vector2.Distance(new Vector2(transform.position.x, transform.position.z), goal) < 0.1)
            {
                Vector3 min = plane.bounds.min;
                Vector3 max = plane.bounds.max;
                goal = new Vector2(Random.Range(min.x, max.x), Random.Range(min.z, max.z));
            }
        }
    }
    
    int ComputePath()
    {
        GridInitialize gridInitialize = GridInitialize.instance;
        Grid grid = gridInitialize.grid;
        int found = 1;

        GridCell goalNode = grid.availables[Random.Range(0,grid.availables.Count-1)];
        goal = new Vector2(goalNode.getCenter().x, goalNode.getCenter().z);

        GridHeuristic heuristic = new(goalNode);

        if(startNode == null)
        {
            Vector2 nodeFloatCoords = new Vector2(transform.position.x, transform.position.z);
            int nodeIDx = (int)(nodeFloatCoords.x * gridInitialize.maxZ) + (int)nodeFloatCoords.y;
            startNode = grid.nodes[nodeIDx];
        }

        gridAStar = new Grid_A_Star(grid.nodes.Count, 50f, 100);
        if(path != null) path.Clear();
        path = gridAStar.findpath(grid, startNode, goalNode, heuristic, ref found);

        if(found > 0 && path != null && path.Count > 0)
        {
            startNode = path[^1];
        }
        return found;
    }

    Vector2 GetNextWaypoint()
    {
        if (!moveAgent) return goal;

        Vector2 nextWaypoint = new Vector2(path[0].getCenter().x, path[0].getCenter().z);
        path.RemoveAt(0);

        return nextWaypoint;
    }
}
