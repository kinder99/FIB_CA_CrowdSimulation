using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Tracker : MonoBehaviour
{
    private Vector3 prevPosition;
    private Vector3 displacement;
    private Vector3 velocity;
    private Vector3 localVelocity;
    private Vector3 forward;
    private float angle;
    public float maxSpeed = 3.0f;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        displacement = gameObject.transform.position - prevPosition;
        velocity = displacement / Time.deltaTime;
        localVelocity = transform.worldToLocalMatrix.MultiplyVector(velocity);

        if (Mathf.Abs(velocity.magnitude) > 0.01f)
        {
            forward = velocity.normalized;
            float offset = velocity.z < 0 ? Mathf.PI : 0;
            angle = Mathf.Atan(velocity.x / velocity.z) + offset;
        }

        prevPosition = gameObject.transform.position;
    }

    public Vector2 GetVelocityXZ()
    {
        return new Vector2(localVelocity.x, localVelocity.z);
    }

    public float GetEulerAngle()
    {
        return angle * Mathf.Rad2Deg;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.purple;
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + forward * velocity.magnitude / 2f);
    }
}
