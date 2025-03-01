using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform spawnPoint;
    private int enemyCount;
    private int waveNumber = 1;
    void Start()
    {
        SpawnEnemyWave(enemyCount);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = FindObjectsByType<EnemyController>(FindObjectsSortMode.None).Length;
        if (enemyCount == 0)
        {
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
    }
    private void SpawnEnemyWave(int enemyCount)
    {
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyIndex = Random.Range(0, enemyTypes.Length);
            Instantiate(enemyTypes[enemyIndex], spawnPoint.position, enemyTypes[enemyIndex].transform.rotation);
        }
    }
}
