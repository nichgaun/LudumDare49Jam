using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    private Car _player;
    private float _hLast;
    private Vector3 _initialPosition;
    private Quaternion _quaternion;
    [SerializeField] GameObject _road;
    [SerializeField] float _lead;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
        _initialPosition = _player.transform.position + Vector3.down * _player.transform.localScale.y / 2;
        _hLast = _initialPosition.x - _road.transform.localScale.x;
        _quaternion = Quaternion.Euler(90, 0, 0);
    }
    
    void LateUpdate()
    {
        if (_hLast - _player.transform.position.x < _lead)
        {
            _hLast += _road.transform.localScale.x;
            Instantiate(_road, _initialPosition + Vector3.right * _hLast, _quaternion);
        }
    }
}
