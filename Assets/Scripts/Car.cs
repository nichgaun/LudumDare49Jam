using UnityEngine;

public class Car : MonoBehaviour
{
    [SerializeField] private Driver _driver;
    [SerializeField] private float _defaultSpeed;
    [SerializeField] private float _hSpeed;
    [SerializeField] private float _vSpeed;
    public float DefaultSpeed { get { return _defaultSpeed; } }

    void Update()
    {
        int hMove, vMove;
        _driver.ControlCar(out hMove, out vMove);
        transform.position = new Vector3(transform.position.x + (hMove * _hSpeed + DefaultSpeed) * Time.deltaTime, transform.position.y, transform.position.z + vMove * _vSpeed * Time.deltaTime);
    }
}
