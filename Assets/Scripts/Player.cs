using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    public SpriteRenderer spriteRenderer;
    Vector2 movement;
    Vector2 mousePos;
    public bool invincible = false;

    public UIManager _UIManager;

    public List<AttackScript> attacks = new List<AttackScript>();

    void Start() {
        _UIManager = GameObject.FindWithTag("UI").GetComponent<UIManager>();
        if (_UIManager == null){
            Debug.LogError("UI Manager is NULL!");
        }
        originalColor = spriteRenderer.color;
        health = maxHealth;
        _UIManager.UpdateHealthSlider(health);
        StartCoroutine(IncrementScore());
    }

    void Update() {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1) {
            movement.Normalize();
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetKeyDown("up")) {
            MoneyAdd(100);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        player.transform.rotation = Quaternion.Euler(0, 0, angle);
        foreach (AttackScript attack in attacks) {
            attack.Check(true);
        }
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
        if (!invincible) {
            health -= damage;
        }
        CameraController.instance.ShakeCamera(0.15f, 0.05f);
        StartCoroutine(DamageFlash());
        if (_UIManager != null) {
            _UIManager.UpdateHealthSlider(health);
        } else {
            Debug.LogWarning("_UIManager is null in Damage method!");
        }
        if (health <= 0) {
            _UIManager.GameOverSequence();
            this.gameObject.SetActive(false);
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
        _UIManager.UpdateMoneyText(money);
    }

    public void MoneySpend(int points)
    {
        money -= points;
        _UIManager.UpdateMoneyText(money);
    }
}
