using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class Grid : FiniteGraph<GridCell, CellConnection, GridConnections>
{
    // Class that represent the finite graph corresponding to a grid of cells
    // There is a known set of nodes (GridCells), 
    // and a known set of connections (CellConnections) between those nodes (GridConnections)

    // Example Data 

	protected float gridHeight;
	
	protected float sizeOfCell;
	
	[SerializeField] protected int numRows;
	[SerializeField] protected int numColumns;
    protected int numCells;

    public List<GridCell> availables;
    //protected List<Connection<GridCell>> connections;
    protected GridConnections gridConnections;

    public float cellSize;

    // Example Constructor function declaration
    public Grid(int maxX, int maxZ, float size, GameObject obs, float height = 0) : base()
    {
        cellSize = size;
        availables = new List<GridCell>();
        numColumns = maxZ; numRows = maxX; numCells = maxX * maxZ; gridHeight = height;
        sizeOfCell = 1;

        // create the cells
        for (int i = 0; i < maxZ; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                int id = i * maxZ + j;
                GridCell cell = new GridCell(id, i, j, obs);
                nodes.Add(cell);
                connections.Add(new GridConnections());

                if (!nodes[^1].IsOccupied())
                {
                    availables.Add(cell);
                }
            }
        }

        for (int i = 0; i < maxZ-1; i++)
        {
            for (int j = 0; j < maxX-1; j++)
            {
                int id = i * maxX + j;
                GridCell right = nodes[id+1];
                GridCell above = nodes[id+maxX];
                GridCell rightAbove = nodes[id+maxX+1];

                if (!nodes[id].IsOccupied() && !right.IsOccupied())
                {
                    connections[id].connections.Add(new CellConnection(nodes[id],right));
                    connections[id+1].connections.Add(new CellConnection(right, nodes[id]));
                }
                if (!nodes[id].IsOccupied() && !above.IsOccupied())
                {
                    connections[id].connections.Add(new CellConnection(nodes[id], above));
                    connections[id+maxX].connections.Add(new CellConnection(above, nodes[id]));
                }
                if (!nodes[id].IsOccupied() && !rightAbove.IsOccupied() && !right.IsOccupied() && !above.IsOccupied())
                {
                    connections[id].connections.Add(new CellConnection(nodes[id], rightAbove));
                    connections[id+maxX+1].connections.Add(new CellConnection(rightAbove, nodes[id]));
                }
                if(j % maxX != 0)
                {
                    GridCell left = nodes[id - 1];
                    GridCell leftAbove = nodes[id + maxX - 1];
                    if (!nodes[id].IsOccupied() && !leftAbove.IsOccupied() && !left.IsOccupied() && !above.IsOccupied())
                    {
                        connections[id].connections.Add(new CellConnection(nodes[id], leftAbove));
                        connections[id + maxX - 1].connections.Add(new CellConnection(leftAbove, nodes[id]));
                    }
                }
            }
        }
        connections.Add(gridConnections);

        // You have basically to fill the base fields "nodes" and "connections", 
        // i.e. create your list of GridCells (with random obstacles) 
        // and then create the corresponding GridConnections for each one of them
        // based on where the obstacles are and the valid movements allowed between GridCells. 


        // TO IMPLEMENT

    }

    public List<GridCell> GetNodes()
    {
        return nodes;
    }

};
