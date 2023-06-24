using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private float health = 0, damage = 0, speed = 0;
    private float maxLifetime = 5;
    private float lifeTimer = 0;
    public void Init(float _speed, float _damage, float _health)
    {
        speed = _speed;
        damage = _damage;
        health = _health;
    }
    void Update()
    {
        lifeTimer += Time.deltaTime;
        if(lifeTimer >= maxLifetime){Destroy(this.gameObject);}
        transform.position += transform.right * speed * Time.deltaTime;
    }
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.TryGetComponent<Enemy>(out Enemy _enemy))
        {
            _enemy.DestroyEnemy(true);
            health --;
            if(health <= 0){Destroy(this.gameObject);}
        }
    }
}
