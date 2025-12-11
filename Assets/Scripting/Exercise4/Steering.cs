using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Steering : MonoBehaviour
{
    [SerializeField] private int MaxAgentNumber;
    [SerializeField] private Agent Agent;
    [SerializeField] private float XBound;
    [SerializeField] private float ZBound;
    [SerializeField] private float maxForce;

    public Vector3 temp;

    [SerializeField] private bool isSeeking;
    public List<Agent> AgentList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateCrowd();
        isSeeking = true;
        //StartCoroutine(SimulationCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Agent agent in AgentList)
        {
            Vector3 g = agent.GetPathManager().GetGoal();
            temp = Seek(agent, g, maxForce);
        }
    }

    public void InstantiateCrowd()
    {
        for (int i = 0; i < MaxAgentNumber; i++)
        {
            float posX = Random.Range(-XBound, XBound);
            float posZ = Random.Range(-ZBound, ZBound);

            //check if computed position conflicts with already instantiated agent
            foreach (Agent a in AgentList)
            {
                if (Mathf.Abs(posX - a.GetPosition().x) <= a.GetRadius() || Mathf.Abs(posZ - a.GetPosition().z) <= a.GetRadius())
                {
                    posX = Random.Range(-XBound, XBound);
                    posZ = Random.Range(-ZBound, ZBound);
                }
            }

            Agent agent = GameObject.Instantiate(Agent, new Vector3(posX, 1f, posZ), Quaternion.identity);
            AgentList.Add(agent);
        }
    }

    public void UpdateSimulation()
    {
        foreach (Agent a in AgentList)
        {
            Vector3 g = a.GetPathManager().GetGoal();
            //Debug.Log(g);
            float max = a.GetMaxSpeed();
            Vector3 vel = max * (g - a.transform.position).normalized; //agents can get stuck in place after a collision

            if (!(this.transform.position.x < XBound && this.transform.position.x > -XBound))
            {
                vel.x = -vel.x;
            }
            if (!(this.transform.position.z < ZBound && this.transform.position.z > -ZBound))
            {
                vel.z = -vel.z;
            }

            if (!IsSeeking())
            {
                a.SetVelocity(vel);
            }
        }
    }

    IEnumerator SimulationCoroutine()
    {
        while (true)
        {
            UpdateSimulation();
            yield return new WaitForSeconds(0.2f);
        }
    }

    public bool IsSeeking()
    {
        return isSeeking;
    }

    public Vector3 Truncate(Vector3 v, float max)
    {
        float size = Mathf.Min(v.magnitude, max);
        return v.normalized * size;
    }

    public Vector3 Seek(Agent a, Vector3 target, float maxForce)
    {
        Vector3 desiredVelocity = (target - a.GetPosition()).normalized * a.GetMaxSpeed();
        Vector3 steering = desiredVelocity - a.GetRigidbody().linearVelocity;

        Truncate(steering, maxForce);
        steering = steering / a.GetRigidbody().mass;

        Vector3 vel = Truncate(a.GetRigidbody().linearVelocity + steering, a.GetMaxSpeed());
        a.SetVelocity(vel);

        //a.GetRigidbody().position += a.GetVelocity() * Time.deltaTime;

        return vel;
    }

    private void OnDrawGizmos()
    {
        Handles.color = Color.green;
        foreach(Agent a in AgentList)
        {
            Handles.ArrowHandleCap(0, a.GetPosition(), Quaternion.Euler(temp), temp.magnitude, EventType.Repaint);
        }
    }
}
