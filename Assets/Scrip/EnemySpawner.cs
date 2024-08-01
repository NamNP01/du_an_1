using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EnemyType
{
    public GameObject enemyPrefab;
    public int count;
    public float spawnInterval;
    public Transform wavePath; // Thêm thuộc tính wavePath
}

[System.Serializable]
public class Wave
{
    public List<EnemyType> enemies;
}

public class EnemySpawner : MonoBehaviour
{
    public List<Wave> waves;
    public List<Transform> spawnPoints;
    public float waveInterval = 20f;
    public TextMeshProUGUI WaveText;
    public GameObject win;

    private int currentWave = 0;
    public int maxWave;
    private float nextWaveTime;
    private bool isFirstWaveStarted = false;
    private int enemiesRemaining = 0;

    void Start()
    {
        nextWaveTime = Time.time + waveInterval;
    }

    void Update()
    {
        if (isFirstWaveStarted && Time.time >= nextWaveTime && enemiesRemaining <= 0)
        {
            StartNextWave();
        }
    }

    public void StartFirstWave()
    {
        if (currentWave == 0)
        {
            StartNextWave();
            isFirstWaveStarted = true;
        }
    }

    void StartNextWave()
    {
        if (currentWave >= waves.Count)
        {
            if (enemiesRemaining <= 0)
            {
                win.SetActive(true);
            }
            return;
        }

        StartSpecificWave(currentWave);

        // Đặt lại thời gian của đợt tiếp theo
        nextWaveTime = Time.time + waveInterval;
        currentWave++;
    }

    public void StartSpecificWave(int waveIndex)
    {
        if (waveIndex < 0 || waveIndex >= waves.Count)
        {
            Debug.LogError("Chỉ số wave không hợp lệ.");
            return;
        }

        // Dừng wave hiện tại nếu có
        StopAllCoroutines();

        // Bắt đầu wave mới
        Wave wave = waves[waveIndex];
        WaveText.text = string.Format("Wave {0}/{1}", waveIndex + 1, maxWave);

        foreach (EnemyType enemyType in wave.enemies)
        {
            enemiesRemaining += enemyType.count;
            StartCoroutine(SpawnEnemies(enemyType));
        }
    }

    IEnumerator SpawnEnemies(EnemyType enemyType)
    {
        for (int i = 0; i < enemyType.count; i++)
        {
            SpawnEnemy(enemyType.enemyPrefab, enemyType.wavePath);
            yield return new WaitForSeconds(enemyType.spawnInterval);
        }
    }

    void SpawnEnemy(GameObject enemyPrefab, Transform wavePath)
    {
        Transform spawnPoint = wavePath.Find("spawnPoint"); // Tìm điểm spawn có tên "spawnPoint" trong wavePath
        if (spawnPoint == null)
        {
            Debug.LogError("Không tìm thấy spawnPoint trong wavePath");
            return;
        }
        Vector2 spawnPosition = spawnPoint.position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
        EnemyController enemyController = enemy.GetComponent<EnemyController>();

        // Chuyển danh sách các điểm từ wavePath cho EnemyController
        List<Transform> waypoints = new List<Transform>();
        foreach (Transform child in wavePath)
        {
            if (child != spawnPoint) // Loại trừ điểm spawn
            {
                waypoints.Add(child);
            }
        }

        enemyController.SetWaypoints(waypoints);

        // Đăng ký sự kiện khi kẻ địch bị tiêu diệt
        enemyController.OnEnemyDestroyed += HandleEnemyDestroyed;
    }

    void HandleEnemyDestroyed()
    {
        enemiesRemaining--;

        if (enemiesRemaining <= 0)
        {
            // Nếu đã hoàn thành tất cả các đợt sóng, hiển thị chiến thắng
            if (currentWave >= waves.Count)
            {
                CastleHealth castleHealth = FindObjectOfType<CastleHealth>();
                if (castleHealth != null)
                {
                    castleHealth.UpdateStars();
                }

                win.SetActive(true);
                Debug.Log("Đã hoàn thành tất cả các đợt sóng!");
            }
            else
            {
                // Đặt lại thời gian của đợt tiếp theo sau khi kết thúc một đợt
                nextWaveTime = Time.time + waveInterval;
            }
        }
    }
}
