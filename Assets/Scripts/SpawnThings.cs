using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnThings : MonoBehaviour
{

    [SerializeField] List<Zone> zones;
    [SerializeField] int zoneIndex;

    [SerializeField] List<Vector3> offRoadSpawnLocations;
    [SerializeField] List<Vector3> forwardSpawnLocations;
    [SerializeField] List<Vector3> oncomingSpawnLocations;
    [SerializeField] GameObject followedObject; //set via tag to player
    [SerializeField] Vector3 offsetFromFollowed; //set in start

    float fixedZPos; //set in Start
    float fixedYPos; //set in start

    float onRoadDistanceTraveled;
    float offRoadDistanceTraveled;
    Vector3 lastPosition;

    private void Start()
    {
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

        onRoadDistanceTraveled += Mathf.Abs((transform.position - lastPosition).magnitude);
        offRoadDistanceTraveled += Mathf.Abs((transform.position - lastPosition).magnitude);
        var zone = zones[zoneIndex];
        if (onRoadDistanceTraveled >= zone.onRoadSpawnInterval)
        {
            zone.SpawnNew(zone.thingsToSpawnInRoad, zone.roadWeights, forwardSpawnLocations, false);
            zone.SpawnNew(zone.thingsToSpawnInRoad, zone.roadWeights, oncomingSpawnLocations, true);
            onRoadDistanceTraveled = 0;
        }
        if (offRoadDistanceTraveled >= zone.offRoadSpawnInterval)
        {
            zone.SpawnNew(zone.thingsToSpawnOffRoad, zone.offRoadWeights, offRoadSpawnLocations, false);
            offRoadDistanceTraveled = 0;
        }

        lastPosition = transform.position;
    }
}
