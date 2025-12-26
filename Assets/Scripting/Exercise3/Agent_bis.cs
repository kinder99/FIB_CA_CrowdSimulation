using System.Collections.Generic;
using TreeEditor;
using UnityEngine;

public class Agent_bis : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float radius;
    [SerializeField]
    public Vector3 velocity;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    public AgentPath pathManager; 
    [SerializeField]
    private Rigidbody rb;

    public bool isSteering;

    public bool isSeeking;
    public float seekWeight;

    public bool isAvoiding;
    public float avoidWeight;

    public bool isAvoidingAgents;
    public float agentAvoidWeight;

    public Vector3 avoidanceForce;
    public Vector3 agentAvoidanceForce;

    private List<GameObject> obstaclesInTrigger;
    private List<GameObject> agentsInTrigger;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<CapsuleCollider>().radius = radius;
        obstaclesInTrigger = new List<GameObject>();
        agentsInTrigger = new List<GameObject>();
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 vel)
    {
        this.velocity = vel;
        rb.linearVelocity = vel;
    }

    public void AddVelocity(Vector3 vel)
    {
        this.velocity += vel;
        rb.linearVelocity += vel;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public float GetRadius()
    {
        return radius;
    }

    public AgentPath GetPathManager()
    {
        return pathManager;
    }

    public Rigidbody GetRigidbody()
    {
        return rb;
    }

    void Update()
    {
        //rb.transform.position += velocity * Time.deltaTime;
        //rb.linearVelocity = velocity;
        //transform.position += velocity * Time.deltaTime;
        position = transform.position;
    }

    public Vector3 GetAvoidanceForce()
    {
        return avoidanceForce;
    }

    public Vector3 GetAgentAvoidanceForce()
    {
        return agentAvoidanceForce;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Obstacle") && !obstaclesInTrigger.Contains(other.gameObject))
            obstaclesInTrigger.Add(other.gameObject);
        if(other.CompareTag("Agent"))
            agentsInTrigger.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        //obstacle avoidance
        float minDist = 9999f;
        GameObject toCheck = null;
        foreach(GameObject go in obstaclesInTrigger)
        {
            if(Vector3.Distance(this.transform.position, go.transform.position) < minDist)
            {
                toCheck = go;
            }
        }
        if(toCheck != null)
            Avoidance(toCheck.transform);

        //agent avoidance
        float agentDist = 9999f;
        GameObject twoCheck = null;
        foreach (GameObject go in agentsInTrigger)
        {
            if (Vector3.Distance(this.transform.position, go.transform.position) < agentDist)
            {
                twoCheck = go;
            }
        }
        if (twoCheck != null)
            AgentAvoidance(twoCheck.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Obstacle"))
            avoidanceForce = Vector3.zero;
            obstaclesInTrigger.Remove(other.gameObject);
        if(other.CompareTag("Agent"))
            agentAvoidanceForce = Vector3.zero;
            agentsInTrigger.Remove(other.gameObject);
    }

    public void Avoidance(Transform other)
    {
        Vector3 origin = transform.position;
        Vector3 obstaclePos = other.transform.position;
        Vector3 dir = (obstaclePos - origin).normalized;

        Vector3 proj = velocity - dir;
        avoidanceForce = Vector3.Cross(proj, Vector3.up) * 1 / (Vector3.Distance(obstaclePos, origin) * 0.3f); //goes to the left

        // if the obstacle is to the left, we want to go right
        if (Vector3.Dot(dir.normalized, this.transform.right.normalized) < 0)
        {
            avoidanceForce = -avoidanceForce;
        }
    }

    public void AgentAvoidance(Transform other)
    {
        Vector3 origin = transform.position;
        Vector3 obstaclePos = other.transform.position;
        Vector3 dir = (obstaclePos - origin).normalized;

        Vector3 proj = velocity - dir;
        agentAvoidanceForce = Vector3.Cross(proj, Vector3.up) * 1 / (Vector3.Distance(obstaclePos, origin) * Random.Range(0.1f,0.5f)); //goes to the left
        Agent_bis otherAgent = other.GetComponent<Agent_bis>();
        otherAgent.agentAvoidanceForce += -agentAvoidanceForce/2;

        // if the obstacle is to the left, we want to go right
        if (Vector3.Dot(dir.normalized, this.transform.right.normalized) < 0)
        {
            agentAvoidanceForce = -agentAvoidanceForce;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        if (pathManager != null)
        {
            Gizmos.DrawLine(transform.position, new Vector3(pathManager.goal.x, 0, pathManager.goal.y));
        }
    }
}
