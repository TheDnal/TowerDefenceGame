using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*
        This script controls the spawning of enemies, and spawns them in waves.
        Each wave has a budget that it needs to expend
    */
    public float spawnRate = 2;
    private List<GameObject> enemies = new List<GameObject>();
    public List<EnemyProfile> enemyProfiles = new List<EnemyProfile>();
    public SpinScript spawnerSpinner;
    private float timer = 0;
    public Transform spawnLocation;
    private bool active = true;
    public int enemiesPerWave = 10;
    private int currEnemySpawnCount = 0;
    public int increasePerWave = 5;
    private int waveCount = 0;
    private enum ScriptState
    {
        IDLE,
        SPAWNING
    }
    private ScriptState currState = ScriptState.IDLE;
    //Singleton Ref
    public static EnemySpawner instance;
    void Awake()
    {
        if(instance != null)
        {
            if(instance != this)
            {
                Destroy(this);
            }
        }
        instance = this;
    }
    void Update()
    {
        switch(currState)
        {
            case ScriptState.IDLE:
                if(enemies.Count == 0)
                {
                    timer = 0;
                    currState = ScriptState.SPAWNING;
                    spawnerSpinner.Play();
                    break;
                }
                break;
            case ScriptState.SPAWNING:
                timer += Time.deltaTime;
                if(timer < 1 / spawnRate){return;}
                SpawnRandomEnemy();
                currEnemySpawnCount ++;
                timer = 0;
                if(currEnemySpawnCount >= enemiesPerWave)
                {
                    spawnerSpinner.Pause();
                    currEnemySpawnCount = 0;
                    if(spawnRate > .2){spawnRate *= 0.75f;}
                    enemiesPerWave += increasePerWave;
                    waveCount ++;
                    currState = ScriptState.IDLE;
                    break;
                }
                break;
        }
        return;
    }

    private void SpawnRandomEnemy()
    {
        EnemyProfile currProfile = enemyProfiles[Random.Range(0,enemyProfiles.Count)];
        GameObject newEnemy = Instantiate(currProfile.prefab,spawnLocation.position,Quaternion.identity);
        SubscribeEnemy(newEnemy);
    }
    public void SetSpawnerEnabled(bool _enabled){active = _enabled;}
    #region SubscriptionPattern
    public void SubscribeEnemy(GameObject _enemy)
    {
        if(enemies.Contains(_enemy)){return;}
        enemies.Add(_enemy);
    }
    public void UnsubscribeEnemy(GameObject _enemy)
    {
        if(!enemies.Contains(_enemy)){return;}
        enemies.Remove(_enemy);
    }
    public List<GameObject> GetSubscribers(){return enemies;}
    #endregion
}
