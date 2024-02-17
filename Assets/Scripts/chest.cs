using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class chest : MonoBehaviour
{
    private bool isPlayerNearby = false;
    private TMP_Text chestText;
    public GameObject chestCanvas;
    public float popupDistance = 0.5f;
    private AudioSource audioSource;
    public Color affordTextColor;
    public Color poorTextColor;
    private Player player;
    private GameManager manager;
    // Start is called before the first frame update
    void Start()
    {
        player = Player.instance;
        manager = GameManager.instance;
        chestText = chestCanvas.transform.GetChild(0).GetComponent<TMP_Text>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.5f;
    }

    // Update is called once per frame
    void Update()
    {
        ShowChestUI();
        if (Input.GetKeyDown("space"))
        {
            if (isPlayerNearby)
            {
                ChestPurchase();
            }
        }
    }

    private void ShowChestUI()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.gameObject.transform.position);
            if (player.money >= manager.unlockCost)
            {
                chestText.color = affordTextColor;
            }
            else
            {
                chestText.color = poorTextColor;
            }
            if (distance <= popupDistance && !isPlayerNearby)
            {
                chestCanvas.SetActive(true);
                isPlayerNearby = true;
                chestText.text = GameManager.instance.unlockCost.ToString();
            }
            else if (distance > popupDistance && isPlayerNearby)
            {
                chestCanvas.SetActive(false);
                isPlayerNearby = false;
            }
        }
    }

    private void ChestPurchase()
    {
        if (player.money >= manager.unlockCost)
        {
            player.MoneySpend(manager.unlockCost);
            //player.Heal(player.maxHealth - player.health);
            audioSource.Play();
            manager.ShowUpgradeMenu();
            manager.FinishChestPurchase();
            Destroy(this.gameObject);
        }
    }
}
