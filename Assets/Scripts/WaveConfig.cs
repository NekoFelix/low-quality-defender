using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Wave Config")]

public class WaveConfig : ScriptableObject
{
    /// <summary>
    /// if you need randomaize enemy, enemy path, enemy speed and enemy spawn uncomment and replace comments below
    /// </summary>

    [SerializeField] GameObject enemyPrefab;                                //[SerializeField] GameObject enemyPrefab;
    [SerializeField] GameObject enemyPathPrefab;                            //[SerializeField] GameObject enemyPathPrefab;

    [SerializeField] int countOfEnemys = 5;
    [SerializeField] float timeBtwSpawnEnemys = 0.75f;
    [SerializeField] float enemySpeed = 2f;

    public GameObject GetEnemyPrefab() { return enemyPrefab; }              //public GameObject GetEnemyPrefab() { return enemyPrefab[Random.Range(0, enemyPrefab.Length)]; }  
    public GameObject GetEnemyPathPrefab() { return enemyPathPrefab; }      //public GameObject GetEnemyPathPrefab() { return enemyPathPrefab[Random.Range(0, enemyPathPrefab.Length)]; }
    public int GetCountOfEnemys() { return countOfEnemys; }                 //public int GetCountOfEnemys() { return Random.Range(1, 10); }
    public float GetTimeBtwSpawnEnemys() { return timeBtwSpawnEnemys; }     //public float GetTimeBtwSpawnEnemys() { return Random.Range(0.25f, 1f); }
    public float GetEnemySpeed() { return enemySpeed; }                     //public float GetEnemySpeed() { return Random.Range(1, 4); }

    public List<Transform> GetWayPoints()
    {
        var wayPoints = new List<Transform>();
        foreach(Transform child in enemyPathPrefab.transform)               //foreach (Transform child in enemyPathPrefab[Random.Range(0, enemyPathPrefab.Length)].transform)
        {
            wayPoints.Add(child);
        }
        return wayPoints;
    }
}
    
