using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] List<WaveConfig> waveConfigs;

    [SerializeField] private int _startingWave = 0;

    private int _waveCounter;

    private void Start()
    {
        StartCoroutine(SpawnWaves());
    }

    public int GetCountWaves()
    {
        return waveConfigs.Count;
    }

    public int GetWaveCounter()
    {
        return _waveCounter;
    }

    private IEnumerator SpawnWaves()
    {
        for (int waveIndex = _startingWave; waveIndex < waveConfigs.Count; waveIndex++)
        {
            var currentWave = waveConfigs[waveIndex];
            _waveCounter++;
            yield return StartCoroutine( SpawnEnemyWave(currentWave));
        }
    }

    private IEnumerator SpawnEnemyWave(WaveConfig waveConfig)
    {
        for (int numberOfEnemys = 0; numberOfEnemys < waveConfig.GetCountOfEnemys(); numberOfEnemys++)
        {
            var newEnemy = Instantiate(waveConfig.GetEnemyPrefab(), waveConfig.GetWayPoints()[0].transform.position, Quaternion.identity);
            newEnemy.GetComponent<Enemy>().SetWaveConfig(waveConfig);
            yield return new WaitForSeconds(waveConfig.GetTimeBtwSpawnEnemys());
        }
    }
}
