using UnityEngine;

public class Locomotion : MonoBehaviour
{
    /*public Animator anim;
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
    }*/

    public Animator _animator;
    public Tracker _tracker;
    private Vector2 _curVelocity;
    public float _interpolationVelocityFactor = 0.05f;
    public float _interpolationOrientationFactor = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _curVelocity = Vector2.Lerp(_curVelocity, _tracker.GetVelocityXZ(), _interpolationVelocityFactor);
        _animator.SetFloat("VelX", _curVelocity.x);
        _animator.SetFloat("VelY", _curVelocity.y);
        transform.rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.eulerAngles.y, _tracker.GetEulerAngle(), _interpolationOrientationFactor), 0f);
    }
}
