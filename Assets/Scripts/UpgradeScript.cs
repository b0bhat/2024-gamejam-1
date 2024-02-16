using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeScript {
    [SerializeField] public float reloadTime = 2f;
    [SerializeField] public int burstCount = 1;
    [SerializeField] public float burstTime = 1f;
    [SerializeField] public float timeBetweenBurst = 1f;
    [SerializeField] public float force = 1f;
    [SerializeField] public float spread = 1f;
    [SerializeField] public int weaponType = 0;
    [SerializeField] public float damage = 0;
    [SerializeField] public float bulletSpeed = 50; 
    [SerializeField] public int penetration = 1; // how many enemies can bullet go through
    [SerializeField] public float angleBurst = 0f; // multi shot weapon angle
    [SerializeField] public int projInShot = 1;
    [ColorUsageAttribute(true,true)] public Color bulletColor;

    void Start() {

    }

}
