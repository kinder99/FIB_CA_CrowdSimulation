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
    private Vector2 _prevSpeedXZ;
    private Vector2 _currSpeedXZ;
    public float _interpolationSpeedFactor = 0.05f;
    public float _interpolationOrientationFactor = 0.2f;

    // Start is called before the first frame update
    void Start()
    {
        _prevSpeedXZ = _tracker.getSpeedXZ();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _currSpeedXZ = Vector2.Lerp(_currSpeedXZ, _tracker.getSpeedXZ(), _interpolationSpeedFactor);
        _currSpeedXZ = _tracker.getSpeedXZ();
        _animator.SetFloat("VelX", _currSpeedXZ.x);
        _animator.SetFloat("VelY", _currSpeedXZ.y);
        transform.rotation = Quaternion.Euler(0f, Mathf.LerpAngle(transform.eulerAngles.y, _tracker.getEulerY(), _interpolationOrientationFactor), 0f);
    }
}
