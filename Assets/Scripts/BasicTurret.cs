using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : MonoBehaviour
{
    public float targetingRefreshRate = 0.5f;
    private float targetingTimer = 0;
    public float rateOfFire = 1;
    private float rofTimer = 0;
    private Transform target;
    public Transform turretPivot;
    public GameObject projectilePrefab;
    public Transform projectileSpawnPos;
    public float radius = 1;
    public float projectileSpeed;
    public float projectileDamage;
    public float projectileHealth;
    public enum TargetingType
    {
        FIRST,
        LAST,
        CLOSE
    }
    public TargetingType currentTargetType = TargetingType.FIRST;
    void Update()
    {

        if(target != null)
        {
            turretPivot.right = target.position - transform.position;
        }

        //Update shoot
        rofTimer+= Time.deltaTime;
        if(rofTimer >= 1 /rateOfFire && target != null)
        {
            rofTimer = 0;
            ShootProjectile();
        }
        //Update target
        targetingTimer += Time.deltaTime;
        if(targetingTimer < targetingRefreshRate){return;}
        targetingTimer = 0;
        switch(currentTargetType)
        {
            case TargetingType.FIRST:
                target = GetFirstEnemy();
                break;
            case TargetingType.LAST:
                target = GetLastEnemy();
                break;
            case TargetingType.CLOSE:
                target = GetClosestEnemy();
                break;
        }
    }
    private Transform GetClosestEnemy()
    {
        float distance, closest = 999;
        Transform target = null;
        foreach(GameObject obj in EnemySpawner.instance.GetSubscribers())
        {
            distance = Vector2.Distance(transform.position,obj.transform.position);
            if(distance > radius){continue;}
            if(distance < closest)
            {
                closest = distance;
                target = obj.transform;
            }
        }
        return target;  
    }
    private Transform GetFirstEnemy()
    {
        float progress = 0, leastProgress = float.MaxValue;
        Transform target = null;
        float distance  = 0;
        foreach(GameObject obj in EnemySpawner.instance.GetSubscribers())
        {
            distance = Vector2.Distance(transform.position,obj.transform.position);
            if(distance > radius){continue;}
            progress = obj.GetComponent<Enemy>().GetProgress();
            if(progress < leastProgress)
            {
                leastProgress = progress;
                target = obj.transform;
            }
        }
        return target;
    }
    private Transform GetLastEnemy()
    {
        float progress = 0, mostProgress = 0;
        Transform target = null;
        float distance  = 0;
        foreach(GameObject obj in EnemySpawner.instance.GetSubscribers())
        {
            distance = Vector2.Distance(transform.position,obj.transform.position);
            if(distance > radius){continue;}
            progress = obj.GetComponent<Enemy>().GetProgress();
            if(progress > mostProgress)
            {
                mostProgress = progress;
                target = obj.transform;
            }
        }
        return target;
    }
    private void ShootProjectile()
    {
        GameObject projectile = Instantiate(projectilePrefab,projectileSpawnPos.position,projectileSpawnPos.rotation);
        projectile.GetComponent<Projectile>().Init(projectileSpeed,projectileDamage,projectileHealth);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
