using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField] private Vector3 goal;
    [SerializeField] private float XBound;
    [SerializeField] private float ZBound;
    [SerializeField] private GameObject goalObj;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        goalObj = GameObject.FindGameObjectWithTag("Goal");
        SetGoal();
    }

    public bool IsGoalReached(Vector3 agentPos)
    {
        return Vector3.Magnitude((agentPos - goal)) <= 1;
    }

    public Vector3 GetGoal()
    {
        return goal;
    }

    public void SetGoal()
    {
        //set y = 1 to have the position of agents and goal aligned to the xz plane
        goalObj.transform.position = new Vector3(Random.Range(-XBound,XBound),1f,Random.Range(-ZBound,ZBound));
    }

    public void Update()
    {
        goal = goalObj.transform.position;
        if (IsGoalReached(this.transform.position)) {
            //Debug.Log("reached goal");
            SetGoal();
        }
    }
}
