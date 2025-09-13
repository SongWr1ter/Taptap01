using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public float spawnInterval = 2f;
    public GameObject enemySpawnPoint;
    private float timer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= spawnInterval)
        {
            timer = 0f;
         
            Vector3 spawnPos = enemySpawnPoint.transform.position;

            ObjectPoolRegister.Instance._objectPool.Spawn("Monster", spawnPos, Quaternion.identity);
            Debug.Log("Éú³É");
        }
    }
}
