using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletParticle : MonoBehaviour
{
    private float damage = 0f;
    private float force = 0f;
    private int penetration;
    private int bounceTimes;
    private int curBounce;
    private float bulletSpeed;
    bool isBounce = false;

    public GameObject hit;
    Rigidbody2D rb;
    public Vector3 fireDir = Vector3.zero;
    public int weaponType;
    public int alreadyPen;
    Color bulletColor;
    [SerializeField] GameObject capsule;
    Vector3 lastVel;
    Vector2 direction;

    List<GameObject> alreadyHit = new List<GameObject>();

    void Awake() {
        rb = this.GetComponent<Rigidbody2D>();
        alreadyPen = 0;
    }

    public void SetWeaponType(int type) {
        weaponType = type;
    }

    void StopBounce() {
        isBounce = false;
    }

    public void SetWeapon(float dmg, float bulletForce, int pen, Color color, int bounce, float speed) {
        damage = dmg;
        force = bulletForce;
        penetration = pen;
        bulletColor = color;
        bulletSpeed = speed;

        bounceTimes = bounce;
        curBounce = bounce;

        // var originalMat =  capsule.gameObject.GetComponent<MeshRenderer>().material;
        // Material mat = Instantiate(originalMat as Material);
        // mat.EnableKeyword("_EMISSION");
        // mat.SetColor("_EmissionColor", bulletColor);
        // capsule.gameObject.GetComponent<MeshRenderer>().material = mat;

    // }
    }
    void OnCollisionEnter2D(Collision2D collision) {
        if (curBounce >= 1 || isBounce) {
            Vector2 refPos = Vector2.Reflect(transform.right, collision.GetContact(0).normal);
            var dir = ((refPos.normalized) * bulletSpeed);
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            this.gameObject.transform.eulerAngles = new Vector3(0, 0, angle);

            isBounce = true;
            Invoke("StopBounce", 0.0f);
            curBounce--;
        } else {
            Instantiate(hit, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    void FixedUpdate() {
    }

    void Update() {
        // lastVel = rb.velocity;
        // direction = rb.velocity.normalized;
        // transform.Translate(direction * bulletSpeed * Time.deltaTime);
        // float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        // Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        // transform.rotation = rotation;
    }

    void OnTriggerEnter2D(Collider2D collider) {
        GameObject other = collider.gameObject;
        if (other.gameObject.CompareTag("Wall")) {
            Destroy(gameObject);
        }
        if(other.TryGetComponent(out Enemy enemy)) {
            Instantiate(hit, transform.position, transform.rotation);
            if (!alreadyHit.Contains(other)) {
                alreadyPen++;
                alreadyHit.Add(other);
                other.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
                enemy.TakeDamage(damage*Random.Range(0.8f,1.2f), rb.velocity * 0.2f*force);
                if (alreadyPen >= penetration) {
                    Destroy(gameObject);
                }
            }
        }
    }

    // private void Bounce() {
    //     if (curBounce < bounceTimes) {
    //         curBounce++;
    //         direction = Vector2.Reflect(direction, transform.right);
    //     }
    //     else {
    //         Destroy(gameObject);
    //     }
    // }

}
