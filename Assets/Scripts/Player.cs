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

    public int score = 0;
    public int money = 0; // currency
    public int maxHealth = 100;

    private int health;

    Vector2 movement;
    Vector2 mousePos;

    private UIManager _UIManager;

    void Start() {
        _UIManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_UIManager == null)
        {
            Debug.LogError("UI Manager is NULL!");
        }

        health = maxHealth;
        _UIManager.UpdateHealthSlider(health);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1) {
            movement.Normalize();
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        //test health system
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Damage(10);
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDirection = mousePos - rb.position;
        float angle = Mathf.Atan2(lookDirection.y, lookDirection.x) * Mathf.Rad2Deg - 90f;
        rb.rotation = angle;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            rb.velocity = Vector2.zero;
        }
    }

    public void Damage(int damage) 
    {
        health -= damage;
        _UIManager.UpdateHealthSlider(health);
        if (health <= 0)
        {
            _UIManager.GameOverSequence();
        }
    }

    public void Heal(int amount)
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
