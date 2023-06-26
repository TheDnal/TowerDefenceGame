using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicTurret : Turret
{
    public float targetingRefreshRate = 0.5f;
    private float targetingTimer = 0;
    private Transform target;
    public Transform turretPivot;
    public Transform projectileSpawnPos;
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
        TurretUpdate(); //update rate of fire

        if(target != null)
        {
            turretPivot.right = target.position - transform.position;
        }
        //Update target
        targetingTimer += Time.deltaTime;
        if(targetingTimer < targetingRefreshRate){return;}
        targetingTimer = 0;
        target = GetTarget();
    }
    private Transform GetTarget()
    {
        Transform _target = null;
        switch(currentTargetType)
        {
            case TargetingType.FIRST:
                _target = GetFirstEnemy();
                break;
            case TargetingType.LAST:
                _target = GetLastEnemy();
                break;
            case TargetingType.CLOSE:
                _target = GetClosestEnemy();
                break;
        }
        return _target;
    }
    private Transform GetClosestEnemy()
    {
        float distance, closest = 999;
        Transform target = null;
        foreach(GameObject obj in enemiesWithinRadius)
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
        foreach(GameObject obj in enemiesWithinRadius)
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
        foreach(GameObject obj in enemiesWithinRadius)
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
    protected override void OnShoot()
    {
        target = GetTarget();
        if(target == null){return;}
        turretPivot.right = target.position - transform.position;
        GameObject projectile = Instantiate(projectilePrefab,projectileSpawnPos.position,projectileSpawnPos.rotation);
        projectile.GetComponent<Projectile>().Init(projectileSpeed,projectileDamage,projectileHealth);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
