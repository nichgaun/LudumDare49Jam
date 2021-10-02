using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    private Car _player;
    private Vector3 _initialDiff;
    [SerializeField] private float _lerpAmount;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
        _initialDiff = transform.position - _player.transform.position;
    }

    void LateUpdate()
    {
        transform.position = Vector3.Lerp(_player.transform.position + _initialDiff, transform.position + _player.DefaultSpeed * Time.deltaTime * Vector3.right, Mathf.Pow(1 - _lerpAmount, Time.deltaTime));
        transform.LookAt(_player.transform.position, Vector3.up);
    }
}
