using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrafficConeSet : MonoBehaviour
{

    [SerializeField] GameObject trafficCone; // set in editor
    [SerializeField] int minimumCones; // set in editor
    [SerializeField] int maximumCones; // set in editor
    [SerializeField] float coneSeparation; // set in editor

    List<GameObject> _ourCones = new List<GameObject>();

    void Start()
    {
        int numCones =  Random.Range(minimumCones, maximumCones);
        for (int index = 0; index < numCones; index++) {
            var instancedCone = Instantiate(
                trafficCone,
                transform.position + Vector3.right * index * coneSeparation,
                Quaternion.identity
            );
            _ourCones.Add(instancedCone);
        }
    }

    void Update()
    {

    }
}
