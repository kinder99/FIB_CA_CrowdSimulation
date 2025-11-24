using UnityEngine;

public class Agent : MonoBehaviour
{
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float radius;
    [SerializeField]
    private Vector3 velocity;
    [SerializeField]
    private Vector3 position;
    [SerializeField]
    private PathManager pathManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.GetComponent<CapsuleCollider>().radius = radius;
    }

    public Vector3 GetVelocity()
    {
        return velocity;
    }

    public void SetVelocity(Vector3 vel)
    {
        this.velocity = vel;
    }

    public float GetMaxSpeed()
    {
        return maxSpeed;
    }

    public Vector3 GetPosition()
    {
        return position;
    }

    public float GetRadius()
    {
        return radius;
    }

    public PathManager GetPathManager()
    {
        return pathManager;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += velocity * Time.deltaTime;
        position = transform.position;
    }
}
