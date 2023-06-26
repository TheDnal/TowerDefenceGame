using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public float rateOfFire;
    private float rofTimer = 0;
    public float radius;
    public GameObject projectilePrefab;
    protected List<GameObject> enemiesWithinRadius = new List<GameObject>();
    public int cost, refundAmount;
    public bool alwaysShoot = false;
    protected void TurretUpdate()
    {
        enemiesWithinRadius = CalculateNearbyEnemies();
        rofTimer += Time.deltaTime;
        if(rofTimer < 1 / rateOfFire){return;}
        if(enemiesWithinRadius.Count == 0 && !alwaysShoot){return;}
        rofTimer = 0;
        OnShoot();
    }
    protected virtual void OnShoot()
    {

    }
    private List<GameObject> CalculateNearbyEnemies()
    {
        List<GameObject> temp = new List<GameObject>();
        foreach(GameObject enemy in EnemySpawner.instance.GetSubscribers())
        {
            if(Vector3.Distance(transform.position,enemy.transform.position) > radius)
                continue;
            temp.Add(enemy);
        }
        return temp;
    }
}
