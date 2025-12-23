using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Tracker : MonoBehaviour
{
    /*public Vector3 prevPosition;

    public Vector3 forward;

    public Vector3 localDisplacement;
    public Vector3 worldDisplacement;

    public Vector3 localVelocity;
    private Vector3 worldVelocity;

    public GameObject character;

    public bool enableDebug;
    public bool fixOrientation;

    public void Start()
    {
        enableDebug = true;
        fixOrientation = false;
        prevPosition = transform.position;
    }

    public void Update()
    {
        forward = transform.forward;
        localDisplacement = character.transform.InverseTransformDirection(transform.position - prevPosition);
        worldDisplacement = transform.position - prevPosition;
        if(localDisplacement.magnitude != 0)
        {
            localVelocity = Vector3.zero;
            localVelocity = (localDisplacement / Time.deltaTime); //velocity = distance/time, use time.deltatime since we're in upgrade
            worldVelocity = character.transform.TransformDirection(localVelocity);
        }
        if (prevPosition == transform.position) { 
            localVelocity = worldVelocity = Vector3.zero;
        }
        if (!fixOrientation)
        {
            character.transform.forward = Vector3.Slerp(forward, worldVelocity, Time.deltaTime);
            forward = character.transform.forward;
        }
        prevPosition = transform.position;
    }

    public Vector3 GetVelocity()
    {
        return localVelocity;
    }

    public Vector3 GetLocalDisplacement()
    {
        return localDisplacement;
    }

    private void OnDrawGizmos()
    {
        if (enableDebug) {
            //draw local velocity vector
            Handles.color = Color.red;
            if (localVelocity != Vector3.zero) { Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(localVelocity), 1.0f, EventType.Repaint); }
            
            //draw world velocity vector
            Handles.color = Color.yellow;
            if (localVelocity != Vector3.zero) { Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(worldVelocity), 1.0f, EventType.Repaint); }

            //draw forward facing vector of character
            Handles.color = Color.blue;
            Handles.ArrowHandleCap(0, transform.position, Quaternion.LookRotation(forward), 1.0f, EventType.Repaint);
        }
        
    }*/

    private Vector3 prevPosition;
    private Vector3 displacement;
    private Vector3 speed;
    private Vector3 localSpeed;
    private Vector3 forward;
    private float eulerY;

    // Start is called before the first frame update
    void Start()
    {
        prevPosition = gameObject.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        displacement = gameObject.transform.position - prevPosition;
        speed = displacement / Time.deltaTime;
        localSpeed = transform.worldToLocalMatrix.MultiplyVector(speed);

        if (Mathf.Abs(speed.magnitude) > 0.01f)
        {
            forward = speed.normalized;
            float offset = speed.z < 0 ? Mathf.PI : 0;
            eulerY = Mathf.Atan(speed.x / speed.z) + offset;
        }

        prevPosition = gameObject.transform.position;
    }

    public float getSpeedX()
    {
        return localSpeed.x;
    }


    public float getSpeedZ()
    {
        return localSpeed.z;
    }

    public Vector2 getSpeedXZ()
    {
        return new Vector2(localSpeed.x, localSpeed.z);
    }

    public float getEulerY()
    {
        return eulerY * Mathf.Rad2Deg;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(gameObject.transform.position, gameObject.transform.position + forward * speed.magnitude / 2f);
    }
}
