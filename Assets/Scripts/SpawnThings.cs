using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour
{
    [SerializeField] List<GameObject> thingsToSpawn; //set in editor
    [SerializeField] float spawnInterval; //set in editor
    [SerializeField] List<Vector3> spawnLocations;

    [SerializeField] GameObject followedObject; //set via tag to player
    [SerializeField] Vector3 offsetFromFollowed; //set in editor

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
            SpawnNew();
            spawnTimer = 0;
        }
    }

    private void SpawnNew()
    {
        //randomly pick a point along the line given as the spawn location
        Vector3 location = spawnLocations[Random.Range(0, spawnLocations.Count)];
        location += transform.position;

        if (thingsToSpawn.Count != 0)
        {
            //randomly select a thing in our list to spawn
            int index = Random.Range(0, thingsToSpawn.Count);
            Instantiate(thingsToSpawn[index], location, Quaternion.identity);
        }
    }
}
