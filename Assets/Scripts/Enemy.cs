using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.8f;
    public int damageAmount = 10;
    public float health = 10;
    public float maxHealth = 10;
    public float attackCooldown = 0.5f; // shouldn't change as enemies get stronger tbh

    public float fadeInDuration = 1.0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damagedColor = new Color(1,0,0,1);

    private bool canDamage = true;

    private Player player;
    private Rigidbody2D rb;
    private Coroutine attackCoroutine;

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.transform.GetChild(0).GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); 
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
        if (player != null) {
            Vector2 direction = (player.gameObject.transform.position - transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
        if (health <= 0) {
            Die();
        }
    }

    public void TakeDamage(float damage, Vector3 force) {
        rb.AddForce(force, ForceMode2D.Impulse);
        Hurt(damage);
    }

    IEnumerator DamageFlash() {
        // Capsule.GetComponent<MeshRenderer>().material = flashMat;
        spriteRenderer.color = damagedColor;
        yield return new WaitForSeconds(0.1f);
        // Capsule.GetComponent<MeshRenderer>().material = origMat;
        spriteRenderer.color = originalColor;
    }

    public void Hurt(float damage) {
        StartCoroutine(DamageFlash());
        // if (bloodLow + damage <= 10) {
        //     bloodLow += damage;
        // } else {
        //     var Blood = Instantiate(BloodPrefab, new Vector3(transform.position.x,0.2f,transform.position.z), Quaternion.Euler(0,0,0), bloodHolder.transform);
        //     Blood.GetComponent<BloodScript>().SetBlood(damage+bloodLow);
        //     bloodLow = 0f;
        // }
        health -= damage;
    }

    void Die() {
        // var death = Instantiate(DeathPrefab);
        // death.transform.position = transform.position;
        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
        }
        Destroy(gameObject);
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && canDamage) {
            attackCoroutine = StartCoroutine(AttackPlayerWithDelay());
        }
    }

    IEnumerator AttackPlayerWithDelay() {
        if (player != null && gameObject != null) {
            canDamage = false;
            player.Damage(damageAmount);
            yield return new WaitForSeconds(attackCooldown);
            canDamage = true;
        }
    }
}
