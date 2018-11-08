using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerScript : MonoBehaviour {
    public float spawnTimer, spawnTimerMax;
    public Vector2 spawnDirection;
    public GameObject enemy;
	// Use this for initialization
	void Start () {
        spawnTimer = spawnTimerMax;
	}
	
	// Update is called once per frame
	void Update () {
        spawnTimer -= Time.deltaTime;
        if(spawnTimer <= 0){
            GameObject newEnemy = Instantiate(enemy, transform.position, Quaternion.identity);
            newEnemy.GetComponent<SentryScript>().startDirection = spawnDirection.normalized;
            spawnTimer = spawnTimerMax;
        }
	}
}
