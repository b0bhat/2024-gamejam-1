using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour {

    #region Singleton
    public static EnemyManager instance;
    void Awake() {
        instance = this;
    }
    #endregion

    List<GameObject> Enemies = new List<GameObject>();

    public GameObject enemyStandardPrefab;
    public GameObject enemyTankPrefab;
    public GameObject enemySwarmPrefab;
    //public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(Vector2 position) {
        GameObject enemyPrefab;
        float randomValue = Random.value;
        if (randomValue <= 0.7f) {
            enemyPrefab = enemyStandardPrefab;
        }
        else if (randomValue <= 0.95f) {
            enemyPrefab = enemySwarmPrefab;
        } else {
            enemyPrefab = enemyTankPrefab;
        }
        GameObject enemy = Instantiate(enemyPrefab, position, Quaternion.identity, transform);
        enemy.GetComponent<Enemy>().SetScaling(GameManager.instance.scaling);
        Enemies.Add(enemy);
    }
}
