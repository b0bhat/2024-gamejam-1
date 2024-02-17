using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region Singleton
    public static Player instance;
    void Awake() {
        instance = this;
    }
    #endregion

    public float moveSpeed = 0.5f;
    public Rigidbody2D rb;
    public GameObject player;
    public int score = 0;
    public int money = 0; // currency
    public int maxHealth = 100;
    private float health;

    private Color originalColor;
    public Color damagedColor = new Color(1,0,0,1);
    public Color normalCharColor = new Color(1,0,0,1);
    public Color moneyCharColor = new Color(1,0,0,1);
    public Color hurtCharColor = new Color(1,0,0,1);
    public Color deadCharColor = new Color(1,0,0,1);
    public SpriteRenderer spriteRenderer;
    Vector2 movement;
    Vector2 mousePos;
    public bool invincible = false;

    [SerializeField]
    private AudioClip hitAudio;
    [SerializeField]
    private AudioClip deathAudio;
    private AudioSource audioSource;

    public UIManager _UIManager;

    public List<GameObject> attacks = new List<GameObject>();
    [SerializeField] GameObject startAttackPrefab;

    private GameObject _UIchar;
    private GameObject _UIface;
    [SerializeField] List<Sprite> faces = new();

    void Start() {
        _UIManager = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        if (_UIManager == null){
            Debug.LogError("UI Manager is NULL!");
        }
        audioSource = GetComponent<AudioSource>();
        originalColor = spriteRenderer.color;
        health = maxHealth;
        _UIManager.UpdateHealthSlider(health);
        StartCoroutine(IncrementScore());
        _UIchar = GameObject.Find("UIcharacter");
        _UIface = GameObject.Find("UIface");
        AddNewAttack(startAttackPrefab);
    }

    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1) {
            movement.Normalize();
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown("f")) {
            MoneyAdd(100);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        player.transform.rotation = Quaternion.Euler(0, 0, angle);
        foreach (GameObject attack in attacks) {
            attack.GetComponent<AttackScript>().Check(true);
        }
    }

    public void AddNewAttack(GameObject attackPrefab) {
        GameObject newAttack = Instantiate(attackPrefab, Vector3.zero, Quaternion.identity, transform.GetChild(0));
        newAttack.transform.localRotation = Quaternion.Euler(Vector3.zero);
        newAttack.name = attackPrefab.name;
        attacks.Add(newAttack);
    }

    public bool CheckAttack(GameObject upgradeAttack) {
        foreach (GameObject attack in attacks) {
            //Debug.Log(attack.name);
            //Debug.Log(upgradeAttack.name);
            if (upgradeAttack.name == attack.name) {
                return true;
            }
        } return false;
    }

    public bool CheckAttackUpgrade(GameObject upgradeAttack, String upgradeName) {
        foreach (GameObject attack in attacks) {
            if (attack.GetComponent<AttackScript>().checkUpgrade(upgradeAttack, upgradeName)) {
                return true;
            }
        } return false;
    }

    public AttackScript GetAttack(GameObject upgradeAttack) {
        foreach (GameObject attack in attacks) {
            if (attack.GetComponent<AttackScript>().name.Equals(upgradeAttack.name)) {
                return attack.GetComponent<AttackScript>();
            }
        } return null;
    }

    private IEnumerator IncrementScore() {
        while (true) {
            yield return new WaitForSeconds(1f);
            ScoreAdd(10);
            GameManager.instance.Scale();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // if (collision.gameObject.CompareTag("Wall")){
        //     rb.velocity = Vector2.zero;
        // }
    }

    public void Damage(float damage) {
        if (_UIManager != null) {
            _UIManager.UpdateHealthSlider(health);
        } else {
            Debug.LogWarning("_UIManager is null in Damage method!");
        }
        if (!invincible) {
            health -= damage;
            if(audioSource.clip != hitAudio) {
                audioSource.clip = hitAudio;
            }
            audioSource.Play();
            CameraController.instance.ShakeCamera(0.15f, 0.05f);
            StartCoroutine(DamageFlash());
        }
        if (health <= 0) {
            if (audioSource.clip != deathAudio){
                audioSource.clip = deathAudio;
            }
            UIChar(4);
            audioSource.Play();
            _UIManager.GameOverSequence();
            this.GetComponent<Collider2D>().enabled = false;
            player.SetActive(false);
            Destroy(this.gameObject, 0.5f);
        } else {
            UIChar(3);
        }
    }

    IEnumerator DamageFlash() {
        spriteRenderer.color = damagedColor;
        yield return new WaitForSeconds(0.1f);
        spriteRenderer.color = originalColor;
    }

    public void Heal(float amount)
    {
        health += amount;
        if(health > maxHealth)
        {
            health = maxHealth;
        }
        _UIManager.UpdateHealthSlider(health);
    }

    public void ScoreAdd(int points)
    {
        score += points;
        _UIManager.UpdateScoreText(score);
    }

    public void MoneyAdd(int points)
    {
        money += points;
        UIChar(2);
        _UIManager.UpdateMoneyText(money);
    }

    public void MoneySpend(int points)
    {
        money -= points;
        _UIManager.UpdateMoneyText(money);
    }
    private void UIChar(int state) {
        if (state == 4) {
            _UIface.GetComponent<Image>().sprite = faces[3];
            _UIchar.GetComponent<Image>().color = deadCharColor;
        } else if (state == 3) {
            StartCoroutine(UICharChange(hurtCharColor, faces[2], 1f));
        } else if (state == 2) {
            StartCoroutine(UICharChange(moneyCharColor, faces[1], 0.8f));
        }      
    }

    IEnumerator UICharChange(Color color, Sprite sprite, float wait) {
        _UIface.GetComponent<Image>().sprite = sprite;
        _UIchar.GetComponent<Image>().color = color;
        yield return new WaitForSeconds(wait);
        if (player.activeSelf) {
            _UIface.GetComponent<Image>().sprite = faces[0];
            _UIchar.GetComponent<Image>().color = normalCharColor;
        }
    }

}
