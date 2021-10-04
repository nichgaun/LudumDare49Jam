using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour
{
    [SerializeField] float spawnInterval; //set in editor

    [SerializeField] List<GameObject> thingsToSpawnInRoad; //set in editor
    [SerializeField] List<GameObject> thingsToSpawnOffRoad; //set in editor

    //spawn lanes
    [SerializeField] List<Vector3> forwardSpawnLocations;
    [SerializeField] List<Vector3> oncomingSpawnLocations;
    [SerializeField] List<Vector3> offRoadSpawnLocations;

    [SerializeField] GameObject followedObject; //set via tag to player
    [SerializeField] Vector3 offsetFromFollowed; //set in start

    float fixedZPos; //set in Start
    float fixedYPos; //set in start

    float spawnTimer;

    private void Start()
    {
        spawnTimer = 0;

        if (followedObject == null) followedObject = GameObject.FindGameObjectWithTag(TagName.Player);
        
        offsetFromFollowed = transform.position - followedObject.transform.position;

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

        spawnTimer += Time.deltaTime;
        if (spawnTimer >= spawnInterval)
        {
            SpawnNew(thingsToSpawnOffRoad, offRoadSpawnLocations);
            SpawnNew(thingsToSpawnInRoad, forwardSpawnLocations);
            SpawnNew(thingsToSpawnInRoad, oncomingSpawnLocations);
            spawnTimer = 0;
        }
    }

    private void SpawnNew(List<GameObject> toSpawn, List<Vector3> locations)
    {
        if (toSpawn.Count != 0)
        {
            int index = Random.Range(0, toSpawn.Count);
            int locationIndex = Random.Range(0, locations.Count);
            Instantiate(toSpawn[index], transform.position + locations[locationIndex], Quaternion.identity);
        }
    }
}
