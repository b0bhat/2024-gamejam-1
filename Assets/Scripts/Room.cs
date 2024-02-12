using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrefab : MonoBehaviour
{
    public Vector2Int position;
    public GameObject wallPrefab;
    public GameObject floorPrefab;
    public GameObject doorPrefab;
    public int width_unit = 10;
    private int width;
    public int height_unit = 10;
    private int height;
    public bool startRoom;
    private int numDoorsLimit;
    private int numDoors;

    void Start()
    {
        width = width_unit*3;
        height = height_unit*3;
        // Either 1 or 2
        numDoors = Random.Range(1, Mathf.Max(width_unit, height_unit)+1);
        GenerateRoom();
    }

    void GenerateRoom()
    {
        GameObject floor = Instantiate(floorPrefab, new Vector2(position.x+width/2f,position.y+height/2f), Quaternion.identity, transform);
        SpawnWall(new Vector3(0, height), width, true);
        SpawnWall(new Vector3(0, 0), width, true);
        SpawnWall(new Vector3(0, 0), height, false);
        SpawnWall(new Vector3(width, 0), height, false);
        floor.transform.localScale = new Vector3(width, height, 1);
    }

    void SpawnWall(Vector3 position, int length, bool isHorizontal)
    {
        for (float i = 0; i < length; i++)
        {
            GameObject prefab;
            if ((i-1) % 3 == 0) {
                prefab = doorPrefab;
            } else {
                prefab = wallPrefab;
            }
            Vector3 offset = isHorizontal ? new Vector3(i+0.5f, 0) : new Vector3(0, i+0.5f);
            GameObject wall = Instantiate(prefab, position + offset, Quaternion.identity, transform);
            if (!isHorizontal)
                wall.transform.Rotate(Vector3.forward, 90f);
        }
    }
}
