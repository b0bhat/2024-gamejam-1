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
        // 0 top door
    // 90 left
    // 180 bottom
    // 270 right
    private TMP_Text doorText;
    public List<Room> rooms = new();

    private Vector2 newRoomPos = new Vector2(0,0);
    private AudioSource audioSource;

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
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    // Update is called once per frame
    void Update() {
        if (!doorOpened) {
            ShowDoorUI();
            if (Input.GetKeyDown("space")) {
                if (isPlayerNearby && !manager.upgradeLock) {
                    DoorPurchase();
                }
            }
        }
    }

    private void ShowDoorUI() {
        if(player != null)
        {
            float distance = Vector2.Distance(transform.position, player.gameObject.transform.position);
            if (player.money >= manager.doorCost)
            {
                doorText.color = affordTextColor;
            }
            else
            {
                doorText.color = poorTextColor;
            }
            if (distance <= popupDistance && !isPlayerNearby)
            {
                doorCanvas.SetActive(true);
                isPlayerNearby = true;
                doorText.text = GameManager.instance.doorCost.ToString();
            }
            else if (distance > popupDistance && isPlayerNearby)
            {
                doorCanvas.SetActive(false);
                isPlayerNearby = false;
            }
        }
    }

    public void AddRoom(GameObject room) {
        rooms.Add(room.GetComponent<Room>());
    }

    private void DoorPurchase() {
        if (player.money >= manager.doorCost) {
            manager.upgradeLock = true;
            player.MoneySpend(manager.doorCost);
            doorSprite.GetComponent<BoxCollider2D>().enabled=false;
            doorSprite.GetComponent<SpriteRenderer>().color=openedColor;
            player.Heal(30);
            doorOpened = true;
            foreach (Room room in rooms) {
                room.ShowRoom();
            }
            doorCanvas.SetActive(false);
            audioSource.Play();
            manager.ShowUpgradeMenu();
            manager.FinishDoorPurchase();
        }
    }

    public void SetDoorRotation(Quaternion rotation) {
        doorSprite.transform.rotation = rotation;
    }
    // void OnTriggerEnter2D(Collider2D other) {
    //     if (other.CompareTag("Player")) {
    //         // GenerateNewRoom();
    //     }
    // }

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