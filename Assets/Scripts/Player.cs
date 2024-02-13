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

    Vector2 movement;
    Vector2 mousePos;

    GameObject score_text;
    GameObject money_text;

    void Start() {
        score_text = GameObject.FindWithTag("Score");
        money_text = GameObject.FindWithTag("Money");
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        score_text.GetComponent<TextMeshProUGUI>().text = "SCORE: " + score.ToString();
        money_text.GetComponent<TextMeshProUGUI>().text = score.ToString() + " Lingons";
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
