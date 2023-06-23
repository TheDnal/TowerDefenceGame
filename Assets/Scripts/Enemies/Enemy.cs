using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    /*
        This script controls the enemy that moves along the game path.
    */
    private int currPathPoint = 1; //the point in the path this enemy is moving towards
    private Vector3 targetPoint = Vector3.zero;
    public float speed = 5;
    void Start()
    {
        targetPoint = Path.instance.GetPointInPath(1).position;
    }
    void Update()
    {
        //Update position
        Vector3 direction = targetPoint - transform.position;
        direction.Normalize();
        transform.position += direction * speed * Time.deltaTime;
        //

        //Check if close enough to next node
        if(Vector3.Distance(transform.position,targetPoint) > 0.05f){return;}
        if(currPathPoint == Path.instance.GetPathLength() - 1){Debug.Log("end reached"); DestroyEnemy();}
        else{
            currPathPoint ++;
            transform.position = targetPoint;
            targetPoint = Path.instance.GetPointInPath(currPathPoint).position;
        }
    }
    public void DestroyEnemy()
    {
        EnemySpawner.instance.UnsubscribeEnemy(this.gameObject);
        Destroy(this.gameObject);
    }
}
