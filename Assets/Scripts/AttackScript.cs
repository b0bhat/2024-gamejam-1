using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackScript : MonoBehaviour
{
    [SerializeField] public float reloadTime = 2f;
    [SerializeField] public int burstCount = 1; // shots in a burst
    [SerializeField] public float burstTime = 1f;
    [SerializeField] public float timeBetweenBurst = 1f; // Time between shots in a burst
    [SerializeField] public float force = 1f; // knockback
    [SerializeField] public float spread = 1f; // Weapon accuracy
    [SerializeField] public int weaponType = 0;
    [SerializeField] public float damage = 0;
    [SerializeField] public float bulletSpeed = 50; 
    [SerializeField] public int penetration = 1; // how many enemies can bullet go through
    [SerializeField] public float angleBurst = 0f; // multi shot weapon angle
    [SerializeField] public int projInShot = 1;
    [SerializeField] GameObject Bullet;
    [SerializeField] Transform firePoint;
    AudioSource audioSource;
    [ColorUsageAttribute(true,true)] public Color bulletColor;

    public float cooldown;

    float tickRate = 0.5f;
    public int burstNum;
    public float burstTick;
    //public int ammoCount;

    //public bool noAmmo;

    public float firingtime;

    void Start()
    {
        //weapon = GetComponent<ParticleSystem>();
        //weapon.GetComponent<ParticleSystem>().Stop();
        //cooldown = reloadTime;
        //ammoCount = ammoCap;
        burstNum = 0;
        burstTick = 0;

        audioSource = GetComponent<AudioSource>();
    }

    public void Check(bool shooting) {
        if (shooting && cooldown >= timeBetweenBurst) {
            cooldown = 0f;
            Fire();
            shooting = false;
            if (burstCount > 1) burstNum = 1;
        } else if (burstNum >= 1) {
            if (burstTick >= burstNum * burstTime){
                Fire();
                burstNum += 1;
            } burstTick += tickRate;
            if (burstTick >= burstCount * burstTime) {
                burstNum = 0;
                burstTick = 0;
            }
        } else {
            if (cooldown < timeBetweenBurst) {
                cooldown += tickRate;
            }
        }
    }

    // public void Reload() {
    //     ammoCount = ammoCap;
    // }

    public void Fire() {
        //CameraController.instance.ShakeCamera(0.05f, force*0.01f);
        for (int i = 1; i <= projInShot; i++) {
            var inc = angleBurst / projInShot;
            var angle = -angleBurst / 2f + (inc / 2) + (inc * (i - 1));
            GameObject bullet = Instantiate(Bullet);
            bullet.GetComponent<BulletParticle>().SetWeapon(damage, force, penetration, bulletColor, bulletSpeed);

            Quaternion spreadRotation = Quaternion.Euler(0, 0, angle + Random.Range(-spread, spread));
            
            // Instead of setting rotation directly, rotate the bullet's direction vector by the spread angle
            Vector3 bulletDirection = Quaternion.Euler(0, 0, angle) * transform.up;
            
            // Apply spread rotation to the bullet's direction
            Vector3 spreadDirection = spreadRotation * bulletDirection;
            
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.AddForce(spreadDirection.normalized * bulletSpeed, ForceMode2D.Impulse); // Normalize the spreadDirection to ensure consistent speed
            bullet.transform.position = firePoint.transform.position;
        }
        audioSource.Play();
    }

}
