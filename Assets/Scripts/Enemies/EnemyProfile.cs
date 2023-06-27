using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class EnemyProfile : ScriptableObject
{
    public GameObject prefab;
    public int minWave = 0;
    public int cost = 1;
}
