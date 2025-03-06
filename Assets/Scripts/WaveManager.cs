using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform spawnPoint;
    private int enemyCount;
    private int waveNumber = 1;
    private GameObject enemyFolder;
    void Start()
    {
        enemyFolder = GameObject.Find("EnemyFolder");
        SpawnEnemyWave(enemyCount);
    }

    // Update is called once per frame
    void Update()
    {
        enemyCount = enemyFolder.transform.childCount;
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
            GameObject newEnemy = Instantiate(enemyTypes[enemyIndex], spawnPoint.position, enemyTypes[enemyIndex].transform.rotation);
            newEnemy.transform.SetParent(enemyFolder.transform);
            newEnemy.GetComponent<UnitStats>().isEnemy = true;
        }
    }
}
