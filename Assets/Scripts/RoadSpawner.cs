using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    private Car _player;
    private float _hLast;
    private Vector3 _initialPosition;
    private Quaternion _quaternion;
    [SerializeField] GameObject _road;
    [SerializeField] float _lead;
    private Queue<GameObject> _createdRoads = new Queue<GameObject>();

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Car>();
        _initialPosition = _player.transform.position + Vector3.down * _player.transform.localScale.y / 2;
        _hLast = _initialPosition.x - _lead;
        _quaternion = Quaternion.Euler(90, 0, 0);
    }
    
    void LateUpdate()
    {
        while (_hLast - _player.transform.position.x < _lead)
        {
            _hLast += _road.transform.localScale.x;
            var createdRoad = Instantiate(_road, _initialPosition + Vector3.right * _hLast, _quaternion);
            _createdRoads.Enqueue(createdRoad);
            while (_createdRoads.Count > Mathf.Max(1, (2 * _lead) / _road.transform.localScale.x + 2))
            {
                Destroy(_createdRoads.Dequeue());
            }
        }
    }
}
