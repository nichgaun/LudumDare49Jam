using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    private Car _player;
    private float _hLast;
    private float _hLastWall;
    private float _hLastGrass;
    private float _hLastBuilding;
    private Vector3 _initialPosition;
    private Quaternion _quaternion;
    [SerializeField] GameObject _road;
    [SerializeField] GameObject _wall;
    [SerializeField] GameObject _grass;
    [SerializeField] GameObject _building;
    [SerializeField] float _lead;
    private Queue<GameObject> _createdRoads = new Queue<GameObject>();
    private Queue<GameObject> _createdWalls = new Queue<GameObject>();
    private Queue<GameObject> _createdGrass = new Queue<GameObject>();
    private Queue<GameObject> _createdBuildings = new Queue<GameObject>();

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag(TagName.Player).GetComponent<Car>();
        _initialPosition = _player.transform.position;
        _hLast = _initialPosition.x - _lead;
        _hLastWall = _initialPosition.x - _lead;
        _hLastGrass = _initialPosition.x - _lead;
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
        while (_hLastGrass - _player.transform.position.x < _lead)
        {
            _hLastGrass += _grass.transform.localScale.x;
            var createdGrass = Instantiate(_grass, _initialPosition + Vector3.right * _hLastGrass, Quaternion.identity);
            _createdGrass.Enqueue(createdGrass);
            while (_createdGrass.Count > Mathf.Max(1, (2 * _lead) / _grass.transform.localScale.x + 2))
            {
                Destroy(_createdGrass.Dequeue());
            }
        }
        while (_hLastBuilding - _player.transform.position.x < _lead * 5)
        {
            _hLastBuilding += 100f;
            var createdBuilding = Instantiate(_building, _initialPosition + Vector3.right * _hLastBuilding + Vector3.forward * Random.Range(100f, 2000f), Quaternion.identity);
            _createdBuildings.Enqueue(createdBuilding);
            while (_createdBuildings.Count > Mathf.Max(1, (2 * _lead * 5) / 100 + 2))
            {
                Destroy(_createdBuildings.Dequeue());
            }
        }

        while (_hLastWall - _player.transform.position.x < _lead)
        {
            _hLastWall += _wall.transform.localScale.x;
            var createdWall = Instantiate(_wall, _initialPosition + Vector3.right * _hLastWall, Quaternion.identity);
            _createdWalls.Enqueue(createdWall);
            while (_createdWalls.Count > Mathf.Max(1, (2 * _lead) / _wall.transform.localScale.x + 2))
            {
                Destroy(_createdWalls.Dequeue());
            }
        }
    }
}
