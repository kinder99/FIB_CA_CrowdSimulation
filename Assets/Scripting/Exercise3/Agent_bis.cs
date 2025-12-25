using UnityEditor.Rendering;
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

    public Vector3 avoidanceForce;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<CapsuleCollider>().radius = radius;
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

    // Update is called once per frame
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

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Obstacle"))
            Avoidance(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Obstacle"))
            avoidanceForce = Vector3.zero;
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
}
