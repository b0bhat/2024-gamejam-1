using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.8f;
    public int damageAmount = 10;

    private Player player;
    private Rigidbody2D rb;

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();  // Getting the Rigidbody2D component
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 direction = (player.gameObject.transform.position - transform.position).normalized;
            rb.velocity = direction * speed;
        }
    }

    // void OnCollisionEnter2D(Collision2D collision)
    // {
    //     if (collision.gameObject.CompareTag("Player"))
    //     {
    //         PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
    //         if (playerHealth != null)
    //         {
    //             playerHealth.TakeDamage(damageAmount);
    //         }
    //     }
    // }
}
