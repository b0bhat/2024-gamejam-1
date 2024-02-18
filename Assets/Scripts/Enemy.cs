using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float speed = 0.8f;
    public float drift = 0.3f;
    public float damageAmount = 10;
    public float health = 10;
    public float maxHealth = 10;
    public float force = 1f;
    public float attackCooldown = 0.1f; // shouldn't change as enemies get stronger tbh
    public int enemyValue = 10;
    public float fadeInDuration = 1.0f;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damagedColor = new Color(1,0,0,1);
    public Color dangerColor = new Color(1,0,0,1);

    private bool canDamage = false;
    private bool damageFlashing = false;
    public GameObject moneyPrefab;
    public GameObject deadPrefab;

    private Player player;
    private Rigidbody2D rb;
    private Coroutine attackCoroutine;

    private AudioSource audioSource;
    private bool dangerClose;
    float playerDist;
    public bool canDamageObject = true;

    void Start()
    {
        player = Player.instance;
        rb = GetComponent<Rigidbody2D>();
        audioSource = GetComponent<AudioSource>();
        audioSource.volume = 0.3f;
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0); 
        StartCoroutine(FadeIn());
        StartCoroutine(ApplyRandomForce());
        canDamageObject = true;
    }

    IEnumerator FadeIn()
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < fadeInDuration) {
            if (!damageFlashing) {
                float alpha = Mathf.Lerp(0, originalColor.a, elapsedTime / fadeInDuration);
                spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            }
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        spriteRenderer.color = originalColor;
        canDamage = true;
    }

    void FixedUpdate() {
        if (player != null) {
            Vector2 direction = (player.gameObject.transform.position - transform.position).normalized;
            rb.AddForce(direction * speed, ForceMode2D.Force);
        }
    }

    void Update() {
        if (health <= 0) {
            Die();
        }
        if (player != null) {
            playerDist = Vector3.Distance(transform.position, player.gameObject.transform.position);
            if (playerDist < 0.5f ) {
                dangerClose = true;
                spriteRenderer.color = dangerColor;
            } else if (dangerClose) {
                dangerClose = false;
                spriteRenderer.color = originalColor;
            } else if (playerDist > 7f) {
                //Debug.Log("deleted");
                Destroy(gameObject); // Cleanup
            }
        }
    }

    IEnumerator ApplyRandomForce() {
        while (true) {
            if (playerDist < 5f) {
                Vector2 randomForce = Random.insideUnitCircle.normalized * drift;
                rb.AddForce(randomForce, ForceMode2D.Impulse);
                yield return new WaitForSeconds(Random.Range(1.5f, 2.5f)); 
            }
            else {
                yield return new WaitForSeconds(1.0f);
            }
        }
    }

    public void TakeDamage(float damage, Vector3 force) {
        rb.AddForce(force, ForceMode2D.Impulse);
        Hurt(damage);
    }

    public void SetScaling(float scale) {
        maxHealth *= scale;
        health *= scale;
        speed += scale/10f;
        gameObject.transform.localScale += new Vector3(scale/50f,scale/50f,scale/50f);
        damageAmount *= scale/3;
    }

    IEnumerator DamageFlash() {
        damageFlashing = true;
        // Capsule.GetComponent<MeshRenderer>().material = flashMat;
        spriteRenderer.color = damagedColor;
        yield return new WaitForSeconds(0.05f);
        // Capsule.GetComponent<MeshRenderer>().material = origMat;
        spriteRenderer.color = originalColor;
        damageFlashing = false;
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
        audioSource.Play();
    }

    void Die() {
        GameObject money = Instantiate(moneyPrefab);
        money.GetComponent<Money>().value = enemyValue;
        GameObject corpse = Instantiate(deadPrefab);
        money.transform.position = transform.position;
        corpse.transform.position = transform.position;
        if (attackCoroutine != null) {
            StopCoroutine(attackCoroutine);
        }
        Destroy(gameObject);
    }

    void OnCollisionStay2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && canDamage) {
            attackCoroutine = StartCoroutine(AttackPlayerWithDelay());
        }
        if (collision.gameObject.CompareTag("Object") && canDamageObject) {
            attackCoroutine = StartCoroutine(AttackObjectWithDelay(collision.gameObject));
        }
    }

    IEnumerator AttackPlayerWithDelay() {
        if (player != null && gameObject != null) {
            canDamage = false;
            player.Damage(damageAmount, rb.velocity * 0.2f*force);
            yield return new WaitForSeconds(attackCooldown);
            canDamage = true;
        }
    }

    IEnumerator AttackObjectWithDelay(GameObject objectOfAttack) {
        if (objectOfAttack != null && gameObject != null) {
            canDamageObject = false;
            objectOfAttack.GetComponent<ObjectScript>().Damage(damageAmount);
            yield return new WaitForSeconds(attackCooldown);
            canDamageObject = true;
        }
    }
    
}
