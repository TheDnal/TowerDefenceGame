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
    public float radius = 1;
    void Update()
    {

        if(target != null)
        {
            turretPivot.right = target.position - transform.position;
        }

        //Update shoot
        rofTimer+= Time.deltaTime;
        if(rofTimer >= 1 /rateOfFire)
        {
            rofTimer = 0;
            if(target != null){target.gameObject.GetComponent<Enemy>().DestroyEnemy();}
        }
        //Update target
        targetingTimer += Time.deltaTime;
        if(targetingTimer < targetingRefreshRate){return;}
        targetingTimer = 0;
        target = GetClosestEnemy();
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
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position,radius);
    }
}
