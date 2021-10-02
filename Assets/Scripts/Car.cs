using UnityEngine;

public class Car : MonoBehaviour
{
    private Collider _collider;
    [SerializeField] private Driver _driver;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _hSpeed;
    [SerializeField] private float _vSpeed;
    public float DefaultSpeed { get { return _defaultSpeed; } }

    private void Awake()
    {
        _collider = GetComponent<Collider>();
    }

    void Update()
    {
        _driver.ControlCar(out int hMove, out int vMove);
        transform.position = new Vector3(transform.position.x + (hMove * _hSpeed + DefaultSpeed) * Time.deltaTime, transform.position.y, transform.position.z + vMove * _vSpeed * Time.deltaTime);
    }

    void OnTriggerEnterOrStay(Collider other)
    {
        Physics.ComputePenetration(_collider, _collider.transform.position, _collider.transform.rotation, other, other.transform.position, other.transform.rotation, out Vector3 direction, out float distance);
        transform.position += new Vector3(direction.x * distance, 0, direction.y * distance);
    }

    void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterOrStay(other);
    }

    void OnTriggerStay(Collider other)
    {
        OnTriggerEnterOrStay(other);
    }
}
