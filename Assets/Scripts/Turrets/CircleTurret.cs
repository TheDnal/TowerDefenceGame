using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleTurret : Turret
{
    public List<Transform> projectilePositions = new List<Transform>();
    public float projectileHealth, projectileDamage, projectileSpeed, projectileLifeTime = 1;
    void Update()
    {
        TurretUpdate();
    }
    protected override void OnShoot()
    {
        foreach(Transform projectilePos in projectilePositions)
        {
            GameObject projectile = Instantiate(projectilePrefab,projectilePos.position,projectilePos.rotation);
            projectile.GetComponent<Projectile>().Init(projectileSpeed,projectileDamage,projectileHealth,projectileLifeTime);
        }
    }
}
