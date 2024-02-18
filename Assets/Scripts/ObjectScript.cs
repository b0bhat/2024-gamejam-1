using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScript : MonoBehaviour {
    public float health = 30;
    Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    public Color damagedColor = new Color(1,0,0,1);
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color; 
    }

    // Update is called once per frame
    void Update()
    {
        if (health < 0) {
            Die();
        }
    }

    public void Damage(float damage) {
        //rb.AddForce(force*3, ForceMode2D.Impulse);
        health -= damage;
        StartCoroutine(DamageFlash());
    }
    IEnumerator DamageFlash() {
        spriteRenderer.color = damagedColor;
        yield return new WaitForSeconds(0.05f);
        spriteRenderer.color = originalColor;
    }
    
    private void Die() {
        StartCoroutine(FadeOut());
        Destroy(gameObject, 2f);
    }
    IEnumerator FadeOut() {
        float elapsedTime = 0.0f;
        while (elapsedTime < 1.9f) {
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime/1.9f);
            spriteRenderer.color = new Color(damagedColor.r, damagedColor.g, damagedColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

}
