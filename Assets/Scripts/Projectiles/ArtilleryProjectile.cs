using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtilleryProjectile : Projectile
{
    public SpriteRenderer explosionSprite, mainSprite;
    public float impactRadius = 2, killRadius = 4;
    private List<GameObject> nearbyEnemies = new List<GameObject>();
    private bool dying = false;
    private float dieDuration = 1, dieTimer = 0;
    void Start()
    {
        explosionSprite.enabled = false;
        nearbyEnemies = new List<GameObject>();
    }
    void Update()
    {
        if(dying)
        {
            dieTimer += Time.deltaTime;
            Color col = explosionSprite.color;
            float alpha = Mathf.Lerp(1,0,dieTimer / dieDuration);
            col.a = alpha;
            explosionSprite.color = col;
            if(dieTimer > dieDuration){Destroy(this.gameObject);}
            return;
        }
        ProjectileUpdate();
        nearbyEnemies.Clear();
        nearbyEnemies = GetEnemiesInRange(impactRadius);
        if(nearbyEnemies.Count > 0){OnExplode();}
    }
    protected override void OnImpact(Collider2D col)
    {
        if(col.gameObject.layer != 7){return;}
        OnExplode();
    }
    private void OnExplode()
    {
        nearbyEnemies = GetEnemiesInRange(killRadius);
        foreach(GameObject enemy in nearbyEnemies)
        {
            enemy.GetComponent<Enemy>().Damage(damage);
        }
        explosionSprite.enabled = true;
        mainSprite.enabled = false;
        dying = true;
        dieTimer = 0;
    }
    private List<GameObject> GetEnemiesInRange(float _range)
    {
        List<GameObject> enemies = new List<GameObject>();
        foreach(GameObject enemy in EnemySpawner.instance.GetSubscribers())
        {
            if(Vector3.Distance(enemy.transform.position,transform.position) <= _range)
            {
                enemies.Add(enemy);
            }
        }
        return enemies;
    }
}
