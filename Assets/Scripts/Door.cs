using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
    public Vector2 roomPosition_unit;
    public bool activated = false;
    public GameObject roomPrefab;
    public float doorDirection;
    // 0 top door
    // 90 left
    // 180 bottom
    // 270 right

    private Vector2 newRoomPos = new Vector2(0,0);

    // Start is called before the first frame update
    void Start() {
        if (doorDirection == 0) {
            newRoomPos = new Vector2(roomPosition_unit.x, roomPosition_unit.y + 1);
        } else if (doorDirection == 90) {
            newRoomPos = new Vector2(roomPosition_unit.x - 1, roomPosition_unit.y);
        } else if (doorDirection == 180) {
            newRoomPos = new Vector2(roomPosition_unit.x, roomPosition_unit.y - 1);
        } else if (doorDirection == 270) {
            newRoomPos = new Vector2(roomPosition_unit.x + 1, roomPosition_unit.y);
        }
    }

    // Update is called once per frame
    void Update() {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) {
            // GenerateNewRoom();
        }
    }

    public void GenerateNewRoom() {
        if (!activated) {
            activated = true;
            GameObject room = Instantiate(roomPrefab, Vector2.zero, Quaternion.identity);
            Room roomScript = room.GetComponent<Room>();
            roomScript.position_unit = newRoomPos;
            roomScript.width_unit = 1;
            roomScript.height_unit = 1;
            roomScript.unit_mult = 1;
            roomScript.enterDoorRotation = (doorDirection + 180) % 360;
        }
    }
}