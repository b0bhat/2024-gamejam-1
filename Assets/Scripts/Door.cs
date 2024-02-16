using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour {
    public Vector2 roomPosition_unit;
    public bool activated = false;
    public GameObject roomPrefab;
    public GameObject doorSprite;
    public GameObject doorCanvas;
    public float popupDistance = 0.5f;
    private bool isPlayerNearby = false;
    private Player player;
    public bool doorOpened = false;
    private GameManager manager;
    public Color closedColor;
    public Color openedColor;
    public Color affordTextColor;
    public Color poorTextColor;
    public float doorDirection;
    private TMP_Text doorText;
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
        player = Player.instance;
        manager = GameManager.instance;
        doorSprite.GetComponent<SpriteRenderer>().color=closedColor;
        doorText = doorCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update() {
        if (!doorOpened) {
            ShowDoorUI();
            if (Input.GetKeyDown("space")) {
                if (isPlayerNearby) {
                    DoorPurchase();
                }
            }
        }
    }

    private void ShowDoorUI() {

        float distance = Vector2.Distance(transform.position, player.gameObject.transform.position);
        if (player.money >= manager.doorCost) {
            doorText.color = affordTextColor;
        } else {
            doorText.color = poorTextColor;
        }
        if (distance <= popupDistance && !isPlayerNearby) {
            doorCanvas.SetActive(true);
            isPlayerNearby = true;
            doorText.text = GameManager.instance.doorCost.ToString();
        }
        else if (distance > popupDistance && isPlayerNearby) {
            doorCanvas.SetActive(false);
            isPlayerNearby = false;
        }
    }

    private void DoorPurchase() {
        if (player.money >= manager.doorCost) {
            player.MoneySpend(manager.doorCost);
            CameraController.instance.ShakeCamera(0.01f, 0.1f);
            doorSprite.GetComponent<BoxCollider2D>().enabled=false;
            doorSprite.GetComponent<SpriteRenderer>().color=openedColor;
            manager.FinishDoorPurchase();
            doorOpened = true;
            doorCanvas.SetActive(false);
        }
    }

    public void SetDoorRotation(Quaternion rotation) {
        doorSprite.transform.rotation = rotation;
    }
    void OnTriggerEnter2D(Collider2D other) {
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