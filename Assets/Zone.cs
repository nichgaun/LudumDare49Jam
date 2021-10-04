using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zone : MonoBehaviour
{
    [SerializeField] public float onRoadSpawnInterval; //set in editor
    [SerializeField] public float offRoadSpawnInterval; //set in editor

    [SerializeField] public List<GameObject> thingsToSpawnInRoad; //set in editor
    [SerializeField] public List<float> roadWeights; //set in editor
    [SerializeField] public List<GameObject> thingsToSpawnOffRoad; //set in editor
    [SerializeField] public List<float> offRoadWeights; //set in editor

    public void SpawnNew(List<GameObject> toSpawn, List<float> weights, List<Vector3> locations, bool oncoming)
    {
        if (toSpawn.Count != 0)
        {
            float sum = 0;
            foreach (var w in weights)
            {
                sum += w;
            }
            float choice = Random.Range(0, sum);
            int index = 0;
            foreach (var w in weights)
            {
                choice -= w;
                if (choice < 0)
                {
                    break;
                }
                index += 1;
            }
            int locationIndex = Random.Range(0, locations.Count);
            var o = Instantiate(toSpawn[index], transform.position + locations[locationIndex], Quaternion.identity);
            if (oncoming)
            {
                var car = o.GetComponent<Car>();
                if (car)
                {
                    car.ReverseDir();
                }
            }
        }
    }
}
