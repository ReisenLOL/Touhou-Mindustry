using TMPro;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] GameObject[] enemyTypes;
    [SerializeField] Transform spawnPoint;
    [SerializeField] TextMeshProUGUI waveText;
    private int waveNumber = 0;
    private GameObject enemyFolder;
    private float waveDuration = 60;
    private float _time;
    void Start()
    {
        _time = waveDuration;
        enemyFolder = GameObject.Find("EnemyFolder");
    }

    // Update is called once per frame
    void Update()
    {
        _time -= Time.deltaTime;
        if (_time <= 0)
        {
            _time = waveDuration;
            waveNumber++;
            SpawnEnemyWave(waveNumber);
        }
        waveText.text = ("Wave: " + waveNumber + "\nNext wave in: " + (int)_time);
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
