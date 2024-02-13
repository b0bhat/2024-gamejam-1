using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int unit_mult = 3;
    public Vector2 position_unit;
    private Vector2 position;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;
    public int width_unit = 10;
    private int width;
    public int height_unit = 10;
    private int height;
    public bool startRoom = false;
    private int numDoorsLimit;
    private int numDoors;
    public List<GameObject> Doors;
    public float enterDoorRotation = 10;
    public bool door_generated = false;

    public struct WallPair {
        public GameObject thisWall;
        public GameObject otherWall;
        public Vector2 position;

        public WallPair(GameObject thisWall, GameObject otherWall, Vector2 position) {
            this.thisWall = thisWall;
            this.otherWall = otherWall;
            this.position = position;
        }
    }

    void Start()
    {
        width = width_unit*unit_mult;
        height = height_unit*unit_mult;
        position = position_unit*unit_mult;
        // Either 1 or 2
        numDoors = UnityEngine.Random.Range(1, Mathf.Max(width_unit, height_unit)+1);
        GenerateRoom();
    }

    void GenerateRoom()
    {
        GameObject floor = Instantiate(floorPrefab, new Vector2(position.x+width/2f,position.y+height/2f), Quaternion.identity, transform);
        SpawnWall(new Vector3(position.x, position.y+height), width, 0f, true);
        SpawnWall(new Vector3(position.x, position.y), width, 180f, true);
        SpawnWall(new Vector3(position.x, position.y), height, 90f, false);
        SpawnWall(new Vector3(position.x+width, position.y), height, 270f, false);
        floor.transform.localScale = new Vector3(width, height, 1);
    }

    void SpawnWall(Vector3 pos, int length, float rotateDegree, bool isHorizontal) {
        List<GameObject> thisWalls = new List<GameObject>();
        List<GameObject> allWalls = MapGenerator.instance.walls;
        for (float i = 0; i < length; i++)
        {
            GameObject prefab;
            bool isDoor = false;
            // if ((i-1) % unit_mult == 0) {
            //     isDoor = true;
            //     prefab = doorPrefab;
            //     if (enterDoorRotation == rotateDegree) {
            //        continue;
            //     }
            // } else {
            //     prefab = wallPrefab;
            // }
            prefab = wallPrefab;
            Vector3 offset = isHorizontal ? new Vector3(i+0.5f, 0) : new Vector3(0, i+0.5f);
            GameObject wall = Instantiate(prefab, pos + offset, Quaternion.identity, transform);
            if (isDoor) {
                Door doorScript = wall.GetComponent<Door>();
                doorScript.roomPosition_unit = position_unit;
                doorScript.doorDirection = rotateDegree;
                Doors.Add(wall);
            }
            wall.transform.Rotate(Vector3.forward, rotateDegree);
            thisWalls.Add(wall);
            
        }
        List<WallPair> thisWallsOverlap = new List<WallPair>();
        foreach (var thisWall in thisWalls.ToArray()) {
            Collider2D thisCollider = thisWall.GetComponent<Collider2D>();
            if (thisCollider == null) continue;

            foreach (var otherWall in allWalls.ToArray()) {
                Collider2D otherCollider = otherWall.GetComponent<Collider2D>();
                if (otherCollider == null) continue;

                if (thisCollider.bounds.Intersects(otherCollider.bounds)) {
                    Vector2 overlapCenter = (thisCollider.bounds.center + otherCollider.bounds.center) / 2f;
                    thisWallsOverlap.Add(new WallPair(thisWall, otherWall, overlapCenter));
                    // Destroy(thisWall);
                    // Destroy(otherWall);
                    // allwalls.Remove(otherWall);
                    break;
                }
            }
        }
        if (door_generated == false && thisWallsOverlap.Count > 0) {
            int middleIndex = thisWallsOverlap.Count / 2;
            Debug.Log(middleIndex);
            if (thisWallsOverlap.Count % 2 == 0)
                middleIndex = UnityEngine.Random.Range(middleIndex - 1, middleIndex + 1);
            Instantiate(doorPrefab, thisWallsOverlap[middleIndex].position, thisWallsOverlap[middleIndex].thisWall.transform.rotation);
            Destroy(thisWallsOverlap[middleIndex].thisWall);
            Destroy(thisWallsOverlap[middleIndex].otherWall);
            door_generated = true;
        }
        allWalls.AddRange(thisWalls);
    }

}
