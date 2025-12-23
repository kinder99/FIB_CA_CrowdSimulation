using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using PathFinding;

public class GridCell : Node 
{
	public GridCell(int i, int x, int z, GameObject obs):base(i) {
		id = i;
		X = x;
		Z = z;
        occupied = (Random.Range(0.0f, 1.0f) <= 0.2);
        Debug.Log(occupied);
        center = new Vector3(X, 0.0f, Z);
        obstacle = obs;
        if (occupied) { GameObject.Instantiate(obs, new Vector3(X,0,Z), Quaternion.identity); }
    }
	public GridCell(GridCell n):base(n) {
        // TO IMPLEMENT
    }

    public bool IsOccupied()
    {
        return occupied;
    }

    public Vector3 GetCenter()
    {
        return center;
    }

    // Your class that represents a grid cell node derives from Node

    // You add any data needed to represent a grid cell node

    // EXAMPLE DATA
    protected GameObject obstacle;

    protected int X;
	protected int Z;

	protected bool occupied;

	protected Vector3 center;

	// You also add any constructors and methods to implement your grid cell node class

	// TO IMPLEMENT
};
