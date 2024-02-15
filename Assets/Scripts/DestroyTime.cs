using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
    public float destructTime = 1f;
    public bool fade = true;
    private SpriteRenderer spriteRenderer;
    private Color originalColor;

    void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        originalColor = spriteRenderer.color;
        if (fade) {
            StartCoroutine(FadeOut());
        }
        Invoke("DestroySelf", destructTime);
    }

    IEnumerator FadeOut() {
        float elapsedTime = 0.0f;
        float destructTimeLess = destructTime*0.9f;
        while (elapsedTime < destructTimeLess) {
            float alpha = Mathf.Lerp(originalColor.a, 0, elapsedTime / destructTimeLess);
            spriteRenderer.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
    }

    void DestroySelf() {
        Destroy(gameObject);
    }
}
