using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Enemy : MonoBehaviour
{
    /*
        This script controls the enemy that moves along the game path.
    */
    private int currPathPoint = 1; //the point in the path this enemy is moving towards
    private Vector3 targetPoint = Vector3.zero;
    public float speed = 5;
    private float progress = 0;
    public float coinsOnKill = 1;
    public TextMeshProUGUI progressText;
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

        progress = CalculateProgress();
        float displayProgress = Mathf.Round(progress * 100);
        progressText.text = displayProgress.ToString() + "%";

        //Check if close enough to next node
        if(Vector3.Distance(transform.position,targetPoint) > 0.05f){return;}
        if(currPathPoint == Path.instance.GetPathLength() - 1){GameController.instance.DecrementHealth(); DestroyEnemy();}
        else{
            currPathPoint ++;
            transform.position = targetPoint;
            targetPoint = Path.instance.GetPointInPath(currPathPoint).position;
        }
    }
    public void DestroyEnemy(bool _defeated = false)
    {
        if(_defeated){GameController.instance.ChangeCoins(coinsOnKill);}
        EnemySpawner.instance.UnsubscribeEnemy(this.gameObject);
        Destroy(this.gameObject);
    }
    public float GetProgress()
    {
        return progress;
    }
    private float CalculateProgress()
    {
        //Get progress along nodes
        float lastNodeReached = currPathPoint - 1;
        float interModeProgress = lastNodeReached / (Path.instance.GetPathLength() - 1);
        //Get progress inside nodes
        float length = Vector2.Distance(Path.instance.GetPointInPath((int)lastNodeReached).position,Path.instance.GetPointInPath(currPathPoint).position); 
        float intraNodeProgress = 1 - (Vector2.Distance(transform.position, Path.instance.GetPointInPath(currPathPoint).position) / length);

        return interModeProgress +(intraNodeProgress / 10 );
    }
}
