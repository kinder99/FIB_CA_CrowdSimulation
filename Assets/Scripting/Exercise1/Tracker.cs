using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

public class Tracker : MonoBehaviour
{
    public Vector3 prevPosition;

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

    private void OnDrawGizmosSelected()
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
        
    }
}
