using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour
{
    [SerializeField] float spawnInterval; //set in editor

    [SerializeField] List<GameObject> thingsToSpawnInRoad; //set in editor
    [SerializeField] List<float> roadWeights; //set in editor
    [SerializeField] List<GameObject> thingsToSpawnOffRoad; //set in editor
    [SerializeField] List<float> offRoadWeights; //set in editor

    //spawn lanes
    [SerializeField] List<Vector3> forwardSpawnLocations;
    [SerializeField] List<Vector3> oncomingSpawnLocations;
    [SerializeField] List<Vector3> offRoadSpawnLocations;

    [SerializeField] GameObject followedObject; //set via tag to player
    [SerializeField] Vector3 offsetFromFollowed; //set in start

    float fixedZPos; //set in Start
    float fixedYPos; //set in start

    float distanceTraveled;
    Vector3 lastPosition;

    private void Start()
    {
        distanceTraveled = 0;

        if (followedObject == null) followedObject = GameObject.FindGameObjectWithTag(TagName.Player);
        
        offsetFromFollowed = transform.position - followedObject.transform.position;
        lastPosition = transform.position;

        fixedZPos = transform.position.z;
        fixedYPos = transform.position.y;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = followedObject.transform.position + offsetFromFollowed;

        Vector3 tmp = transform.position;
        tmp.z = fixedZPos;
        tmp.y = fixedYPos;
        transform.position = tmp;

        distanceTraveled += Mathf.Abs((transform.position - lastPosition).magnitude);
        if (distanceTraveled >= spawnInterval)
        {
            SpawnNew(thingsToSpawnOffRoad, offRoadWeights, offRoadSpawnLocations, false);
            SpawnNew(thingsToSpawnInRoad, roadWeights, forwardSpawnLocations, false);
            SpawnNew(thingsToSpawnInRoad, roadWeights, oncomingSpawnLocations, true);
            distanceTraveled = 0;
        }

        lastPosition = transform.position;
    }

    private void SpawnNew(List<GameObject> toSpawn, List<float> weights, List<Vector3> locations, bool oncoming)
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
            Instantiate(toSpawn[index], transform.position + locations[locationIndex], Quaternion.identity);
        }
    }
}
