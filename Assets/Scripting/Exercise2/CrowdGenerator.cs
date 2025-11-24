using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrowdGenerator : MonoBehaviour
{
    [SerializeField] private int MaxAgentNumber;
    [SerializeField] private Agent Agent;
    [SerializeField] private float XBound;
    [SerializeField] private float ZBound;
    public List<Agent> AgentList;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        InstantiateCrowd();
        StartCoroutine(SimulationCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
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
                if(Mathf.Abs(posX - a.GetPosition().x) <= a.GetRadius() || Mathf.Abs(posZ - a.GetPosition().z) <= a.GetRadius())
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

            if(!(this.transform.position.x < XBound && this.transform.position.x > -XBound))
            {
                vel.x = -vel.x;
            }
            if (!(this.transform.position.z < ZBound && this.transform.position.z > -ZBound))
            {
                vel.z = -vel.z;
            }

            a.SetVelocity(vel);
        }
    }

    IEnumerator SimulationCoroutine()
    {
        while (true)
        {
            UpdateSimulation();
            yield return new WaitForSeconds(1);
        }
    }
}
