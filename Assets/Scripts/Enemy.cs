using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.8f;
    public int damageAmount = 10;

    public float fadeInDuration = 1.0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    private Player player;
    private Rigidbody2D rb;

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); // Set the sprite to be transparent initially
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeInDuration) {
            float alpha = Mathf.Lerp(0, originalColor.a, elapsedTime / fadeInDuration);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
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
