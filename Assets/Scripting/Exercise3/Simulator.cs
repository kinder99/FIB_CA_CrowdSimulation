using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Android;

public class Simulator : MonoBehaviour
{
    // singleton
    public static Simulator _instance = null;
    public static Simulator GetInstance()
    {
        if (_instance == null)
        {
            GameObject _simulatorGameObject = new GameObject("Simulator");
            _instance = _simulatorGameObject.AddComponent<Simulator>();
        }
        return _instance;
    }

    public List<Agent_bis> agents = new List<Agent_bis>();

    public float velocity = 3f;
    public float maxForce = 5f;
    private float maxSpeed = 3f;

    public float[] lowestTs = new float[100];

    public Transform goalPos;

    private Vector3 steering;

    public bool doSeek;
    public bool doAvoidObstacles;
    public bool doAvoidOtherAgents;

    public float seekWeight;
    public float avoidObstaclesWeight;
    public float avoidAgentWeight;

    // Update is called once per frame
    void Update()
    {
        UpdateSimulation(Time.deltaTime);
        foreach (var agent in agents)
        {
            if (doSeek) agent.isSeeking = true;
            else agent.isSeeking = false;

            if (doAvoidObstacles) agent.isAvoiding = true;
            else agent.isAvoiding = false;

            if (doAvoidOtherAgents) agent.isAvoidingAgents = true;
            else agent.isAvoidingAgents = false;

            agent.seekWeight = seekWeight;
            agent.avoidWeight = avoidObstaclesWeight;
            agent.agentAvoidWeight = avoidAgentWeight;
        }
    }

    void UpdateSimulation(float timestep)
    {
        foreach (Agent_bis agent in agents)
        {
            Vector3 goal = new Vector3(agent.GetPathManager().goal.x, agent.transform.position.y, agent.GetPathManager().goal.y);
            if (goalPos !=  null)
                goal = goalPos.position;

            if (agent.isSteering)
            {
                steering = ComputeSteeringForces(agent, goal);

                Vector3 force = Truncate(steering, maxForce);
                Vector3 accel = force / agent.GetRigidbody().mass;
                agent.AddVelocity(accel * timestep);
                agent.SetVelocity(Truncate(agent.GetVelocity(), maxSpeed));
            }
            else
            {
                agent.SetVelocity((goal - agent.transform.position).normalized * velocity);
            }
        }
    }
    Vector3 Truncate(Vector3 v, float max)
    {
        float size = Mathf.Min(v.magnitude, max);
        return v.normalized * size;
    }

    Vector3 ComputeSteeringForces(Agent_bis a, Vector3 target)
    {
        Vector3 steeringForces = Vector3.zero;

        if(a.isSeeking) steeringForces += Seek(a, target) * a.seekWeight;
        if (a.isAvoiding) steeringForces += Avoid(a) * a.avoidWeight;
        if (a.isAvoidingAgents) steeringForces += AgentAvoid(a) * a.agentAvoidWeight;

        return steeringForces;
    }

    Vector3 Seek(Agent_bis a, Vector3 target)
    {
        Vector3 desiredVelocity = (target - a.transform.position).normalized * a.GetMaxSpeed();
        return desiredVelocity - a.velocity;
    }

    Vector3 Avoid(Agent_bis a)
    {
        return a.GetAvoidanceForce();
    }

    Vector3 AgentAvoid(Agent_bis a)
    {
        return a.GetAgentAvoidanceForce();
    }

    void OnDrawGizmos()
    {
        // draw lines to intersection points

        foreach(Agent_bis agent in agents)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.GetAvoidanceForce());
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.GetVelocity());
            Gizmos.color = Color.green;
            Gizmos.DrawLine(agent.transform.position, agent.transform.position + agent.GetAgentAvoidanceForce());
        }
    }

}