using UnityEngine;
using PathFinding;
using System.Collections.Generic;
using System.Collections;
public class GridInitialize : MonoBehaviour
{
    [SerializeField]
    private GameObject obstacle;
    [SerializeField] public int maxX;
    [SerializeField] public int maxZ;

    [SerializeField] private bool debugGrid;
    public Grid grid;

    [SerializeField] private int MaxAgentNumber;
    [SerializeField] private Agent_bis Agent;
    public float agentVelocity = 2f;
    public Collider plane;

    public static GridInitialize instance = null;

    public bool seekingAgents;
    public float seekWeight;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Awake()
    {
        grid = new Grid(maxX, maxZ, 1, obstacle, 0);
    }

    public void Start()
    {
        Vector3 min = plane.bounds.min;
        Vector3 max = plane.bounds.max;

        Simulator sim = Simulator.GetInstance();
        sim.velocity = agentVelocity;
        ref List<Agent_bis> agents = ref sim.agents;
        InstantiateCrowd(agents);
        instance = this;
    }

    // Update is called once per frame
    public void Update()
    {
        Debug.Log(grid.GetNodes());
    }

    public void InstantiateCrowd(List<Agent_bis> agents)
    {
        List<int> occupiedNodes = new List<int>();
        for (int i = 0; i < MaxAgentNumber; i++)
        {
            int idx = Random.Range(0, grid.availables.Count - 1);

            while (occupiedNodes.Contains(idx))
            {
                idx = Random.Range(0, grid.availables.Count - 1);
            }

            GridCell spawn = grid.availables[idx];
            Agent_bis agentInstance = GameObject.Instantiate(Agent,spawn.getCenter(), Quaternion.identity);
            agents.Add(agentInstance);
        }
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
