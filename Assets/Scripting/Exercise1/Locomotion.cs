using UnityEngine;

public class Locomotion : MonoBehaviour
{
    public Animator anim;
    public Tracker tracker;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetFloat("VelX",tracker.GetVelocity().x,0.2f,Time.deltaTime);
        anim.SetFloat("VelY",tracker.GetVelocity().z,0.2f,Time.deltaTime);
    }
}
