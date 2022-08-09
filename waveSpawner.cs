using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Author: Christopher Cruz
// Spawners that can be used to set at locations or waves
public class waveSpawner : MonoBehaviour
{
    public int wave = 0;
    public int endWave;
    public int enemySpawnAmount = 0;
    public int enemiesDestroyed = 0;
    public GameObject[] spawners;
    public GameObject[] enemy;
    public bool isWave;
    public bool isTrigger;
    private int randomEnemy;
    // Start is called before the first frame update
    void Start()
    {
        //spawners = new GameObject[5];
        for(int i = 0; i < spawners.Length; i++)
        {
            spawners[i] = transform.GetChild(i).gameObject;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isWave && isTrigger)
        {
            if(wave != endWave)
            {
                if(wave == 0)
                {
                    NextWave();
                }
                else if(enemiesDestroyed >= enemySpawnAmount && isWave)
                {
                    NextWave();
                }
            }
        }

    }
    public void SpawnEnemy()
    {
        GameObject newEnemy;
        int spawnersID = Random.Range(0, spawners.Length);
        int randomEnemy = Random.Range(0, enemy.Length);
        //Instantiate(enemy, spawners[spawnersID].transform.position, spawners[spawnersID].transform.rotation);
        newEnemy = Instantiate(enemy[randomEnemy], spawners[spawnersID].transform.position, spawners[spawnersID].transform.rotation);
        newEnemy.GetComponentInChildren<EnemyStats>().spawner = this;
    }
    public void NextWave()
    {
        wave++;
        //enemySpawnAmount += 2;
        enemiesDestroyed = 0;
        for(int i = 0; i < enemySpawnAmount; i++)
        {
            SpawnEnemy();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && !isTrigger && !isWave)
        {
            for(int i = 0; i < enemySpawnAmount; i++)
            {
                SpawnEnemy();
            }
        }
        isTrigger = true;
    }
}
