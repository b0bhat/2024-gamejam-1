using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    #region Singleton
    public static MapGenerator instance;
    void Awake() {
        instance = this;
    }
    #endregion
    public GameObject roomPrefab;
    public GameObject doorPrefab;
    public GameObject chestPrefab;
    public int numRooms;
    public List<Vector2> roomPositions = new List<Vector2>();
    public List<Vector2> occupiedPositions = new List<Vector2>();
    public List<Vector2> doorPositions = new List<Vector2>(); // Tracks all door positions
    private int[] roomSizes = {2, 3, 4, 5, 6, 7};
    [SerializeField] private int maxChests = 3;
    public Dictionary<Vector2, List<Vector2>> adjacencyMap = new Dictionary<Vector2, List<Vector2>>();
    public List<GameObject> walls = new List<GameObject>();
    
    void Start()
    {
        GenerateRoom(Vector2.zero, 4, true); // Generate a 1x1 room at the spawn position
        roomPositions.Add(Vector2.zero);
        MarkOccupied(Vector2.zero, 4);
        GenerateMap(numRooms);
    }

    // void Update() {
    //     if (wait1 == 1) {
    //         ReplaceOverlappingWithDoor();
    //     } wait1++;
    // }
    void GenerateMap(int roomNum)
    {

        Vector2[] directions = { Vector2.up, Vector2.left, Vector2.right, Vector2.down };

        for (int i = 0; i < roomNum*5; i++) {
            Vector2 randomPosition = roomPositions[Random.Range(0, roomPositions.Count)];
            Vector2 direction = directions[Random.Range(0, directions.Length)];
            int randomSize = roomSizes[Random.Range(0, roomSizes.Length)];
            Vector2 newPosition = randomPosition + direction * randomSize;
            if (!IsOverlap(newPosition, randomSize)) {
                if (IsAdjacentToRoom(newPosition, randomSize)) {
                    GenerateRoom(newPosition, randomSize, false);
                    MarkOccupied(newPosition, randomSize);
                    roomPositions.Add(newPosition);
                }
            }
            if (roomPositions.Count >= roomNum) {
                break;
            }
        }
    }

    bool IsOverlap(Vector2 position, int size)
    {
        // Check if any part of the new room overlaps with existing rooms or occupied positions
        for (int x = (int)position.x; x < position.x + size; x++)
        {
            for (int y = (int)position.y; y < position.y + size; y++)
            {
                Vector2 checkPosition = new Vector2(x, y);
                if (roomPositions.Contains(checkPosition) || occupiedPositions.Contains(checkPosition))
                {
                    return true; // Overlaps
                }
            }
        }
        return false; // No overlap
    }

    bool IsAdjacentToRoom(Vector2 position, int size)
    {
        // Check if any part of the new room is adjacent to an existing room
        for (int x = (int)position.x - 1; x < position.x + size + 1; x++)
        {
            for (int y = (int)position.y - 1; y < position.y + size + 1; y++)
            {
                Vector2 checkPosition = new Vector2(x, y);
                if (roomPositions.Contains(checkPosition))
                {
                    // // Update adjacency map
                    // if (!adjacencyMap.ContainsKey(checkPosition))
                    // {
                    //     adjacencyMap[checkPosition] = new List<Vector2>();
                    // }
                    // adjacencyMap[checkPosition].Add(position);
                    // if (!adjacencyMap.ContainsKey(position))
                    // {
                    //     adjacencyMap[position] = new List<Vector2>();
                    // }
                    // adjacencyMap[position].Add(checkPosition);
                    return true; // Adjacent
                }
            }
        }
        return false; // Not adjacent
    }

    void MarkOccupied(Vector2 position, int size)
    {
        // Mark all squares occupied by the new room
        for (int x = (int)position.x; x < position.x + size; x++)
        {
            for (int y = (int)position.y; y < position.y + size; y++)
            {
                occupiedPositions.Add(new Vector2(x, y));
            }
        }
    }

    void GenerateRoom(Vector2 position, int size, bool start=false) {
        GameObject room = Instantiate(roomPrefab, position, Quaternion.identity);
        Room roomScript = room.GetComponent<Room>();
        int chestAmount = Random.Range(0, maxChests+1);
        roomScript.position_unit = position;
        roomScript.width_unit = size;
        roomScript.height_unit = size;
        roomScript.unit_mult = 1;
        if (start == true) {
            roomScript.hidden = false;
            chestAmount = 0;
        }
        for (int i = 0; i < chestAmount; i++)
        {
            float x = position.x + Random.Range(1.0f, size-1.0f);
            float y = position.y + Random.Range(1.0f, size-1.0f);
            Vector2 chestPos = new Vector2(x, y);
            Instantiate(chestPrefab, chestPos, Quaternion.identity);
        }
    }

    // Instantiate(doorPrefab, checkPosition, Quaternion.identity);
}
