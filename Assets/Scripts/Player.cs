using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
    public int health = 0;

    Vector2 movement;
    Vector2 mousePos;

    GameObject score_text;
    GameObject money_text;
    GameObject health_text;

    void Start() {
        score_text = GameObject.FindWithTag("Score");
        money_text = GameObject.FindWithTag("Money");
        health_text = GameObject.FindWithTag("Health");
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        if (movement.magnitude > 1) {
            movement.Normalize();
        }
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        score_text.GetComponent<TextMeshProUGUI>().text = "SCORE: " + score.ToString();
        money_text.GetComponent<TextMeshProUGUI>().text = money.ToString() + " Lingons";
        health_text.GetComponent<TextMeshProUGUI>().text = "HEALTH: " + health.ToString();
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

}
