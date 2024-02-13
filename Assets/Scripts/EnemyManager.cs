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

    public GameObject enemyPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SpawnEnemy(Vector2 position) {
        Instantiate(enemyPrefab, position, Quaternion.identity, transform);
    }
}
