using UnityEngine;
using PathFinding;
public class GridInitialize : MonoBehaviour
{
    [SerializeField]
    private GameObject obstacle;
    [SerializeField] private int maxX;
    [SerializeField] private int maxZ;

    [SerializeField] private bool debugGrid;
    private Grid grid;

    public static GridInitialize instance = null;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        grid = new Grid(maxX, maxZ, 1, obstacle, 0);
        instance = this;
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log(grid.GetNodes());
    }

    private void OnDrawGizmos()
    {
        if(!Application.isPlaying) return;

        Gizmos.color = Color.yellow;
        for(int i = 0; i < grid.GetNodes().Count; i++)
        {
            GridCell cell = grid.GetNodes()[i];
            for(int j = 0; j < grid.connections[i].connections.Count; j++)
            {
                if (debugGrid) Gizmos.DrawLine(cell.getCenter(), grid.connections[i].connections[j].toNode.getCenter());
            }
        }
    }
}
