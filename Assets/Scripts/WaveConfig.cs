using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config")]

public class WaveConfig : ScriptableObject
{
    [SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyPathPrefab;

    [SerializeField] int countOfEnemys = 5;
    [SerializeField] float timeBtwSpawnEnemys = 0.75f;
    [SerializeField] float enemySpeed = 2f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }
    public GameObject GetEnemyPathPrefab() { return enemyPathPrefab; }
    public int GetCountOfEnemys() { return countOfEnemys; }
    public float GetTimeBtwSpawnEnemys() { return timeBtwSpawnEnemys; }
    public float GetEnemySpeed() { return enemySpeed; }

    public List<Transform> GetWayPoints()
    {
        var wayPoints = new List<Transform>();
        foreach (Transform child in enemyPathPrefab.transform)
        {
            wayPoints.Add(child);
        }
        return wayPoints;
    }
}
    
