using UnityEngine;

public class CollidableObstacle : MonoBehaviour
{
    [SerializeField] float _inertia; // set in editor
    public float Inertia { get { return _inertia; } }

    private float _VSpeed;
    private float _HSpeed;
    private float _groundHeight;

    private Vector3 _tumbleDirection;
    private float _tumbleAngularSpeed;

    /// param: positive is up
    public void SetVerticalSpeed(float speed) {
        _VSpeed = speed;
    }

    public void SetHorizontalSpeed(float speed) {
        _HSpeed = speed;
    }

    public void SetTumbleDirectionAndIntensity(Vector3 tumble, float intensity) {
        _tumbleAngularSpeed = intensity;
        _tumbleDirection = tumble;
    }

    void Start() {
        _VSpeed = 0;
        _HSpeed = 0;
        _tumbleAngularSpeed = 0;
        _groundHeight = 0;
    }

    void Update() {
        transform.position += (Vector3.up * _VSpeed + Vector3.back * _HSpeed )* Time.deltaTime;
        transform.Rotate(Time.deltaTime * _tumbleDirection * _tumbleAngularSpeed);

        if (transform.position.y > _groundHeight)
        {
            _VSpeed -= UniversalConstants.Gravity * Time.fixedDeltaTime;
        }
        else
        {
            transform.position = new Vector3(transform.position.x, _groundHeight, transform.position.z);
            _VSpeed = Mathf.Max(_VSpeed, 0);
        }

        _HSpeed *= (float) 1 - UniversalConstants.Friction * Time.deltaTime;
        _tumbleAngularSpeed *= (float) 1 - UniversalConstants.AirResistance * Time.deltaTime;
    }
}