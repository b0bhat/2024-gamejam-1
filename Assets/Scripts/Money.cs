using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Money : MonoBehaviour
{
    public int value = 10;
    private bool pickupStarted = false;
    private Transform playerTransform;
    //public AudioClip pickupSound;
    private Rigidbody2D rb;
    public float pickupDistance = 0.2f;

    private void Start() {
        playerTransform = Player.instance.gameObject.transform;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update() {
        if (pickupStarted) {
            Vector3 direction = (playerTransform.position - transform.position).normalized;
            rb.AddForce(direction * 1.5f, ForceMode2D.Force);
            if (Vector3.Distance(transform.position, playerTransform.position) < pickupDistance) {
                Pickup();
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            pickupStarted = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Player")) {
            pickupStarted = false;
        }
    }

    private void Pickup() {
        Player.instance.MoneyAdd(value);
        // if (pickupSound != null) {
        //     AudioSource.PlayClipAtPoint(pickupSound, transform.position);
        // }
        Destroy(gameObject);
    }
}