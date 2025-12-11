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

    //protected List<GridCell> cells;
    //protected List<Connection<GridCell>> connections;
    protected GridConnections gridConnections;

    // Example Constructor function declaration
    public Grid(int maxX, int maxZ, float cellSize, GameObject obs, float height = 0) : base()
    {
        numColumns = maxZ; numRows = maxX; numCells = maxX * maxZ; gridHeight = height;
        sizeOfCell = 1;

        // create the cells
        for (int i = 0; i < maxZ; i++)
        {
            for (int j = 0; j < maxX; j++)
            {
                int id = i * maxZ + j;
                nodes.Add(new GridCell(id, i, j, obs));
            }
        }

        for (int i = 0; i < maxZ-1; i++)
        {
            for (int j = 0;j < maxX-1; j++)
            {
                int id = i * maxX + j;
                GridCell right = nodes[id+1];
                GridCell above = nodes[id+maxX];
                GridCell rightAbove = nodes[id+maxX+1];

                if(nodes[id].IsOccupied() == right.IsOccupied() == false)
                {
                    CellConnection connectionRight = new CellConnection(nodes[id], right);
                    gridConnections.Add(connectionRight);
                }
                if (nodes[id].IsOccupied() == above.IsOccupied() == false)
                {
                    CellConnection connectionAbove = new CellConnection(nodes[id], above);
                    gridConnections.Add(connectionAbove);
                }
                if (nodes[id].IsOccupied() == rightAbove.IsOccupied() == false)
                {
                    CellConnection connectionRightAbove = new CellConnection(nodes[id], rightAbove);
                    gridConnections.Add(connectionRightAbove);
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

};
