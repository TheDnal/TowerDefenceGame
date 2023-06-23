using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    /*
        This script controls the spawning of enemies
    */
    public float spawnRate = 2;
    public GameObject enemyPrefab;
    private List<GameObject> enemies = new List<GameObject>();
    private float timer = 0;
    public Transform spawnLocation;
    
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
        //Update timer
        timer += Time.deltaTime;
        if(timer < 1 / spawnRate){return;}
        timer = 0;
        //

        //Spawn enemy
        GameObject newEnemy = Instantiate(enemyPrefab,spawnLocation.position,Quaternion.identity);
        SubscribeEnemy(newEnemy);
        return;
    }



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
