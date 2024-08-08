using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossController : MonoBehaviour
{
    public Transform[] teleportPositions; // Các vị trí dịch chuyển
    public float effectInterval = 3f; // Thời gian giữa các hiệu ứng
    public AudioClip newAudioClip; // Âm thanh mới
    public AudioClip Summon; // Âm thanh mới
    public GameObject effPrefab; // Prefab hiệu ứng
    public AudioSource src;

    private float effectTimer;
    private Animator animator;
    private int lastEffectIndex = -1; // Lưu trữ chỉ số hiệu ứng đã sử dụng lần cuối cùng

    private EnemySpawner enemySpawner; // Tham chiếu đến EnemySpawner
    private AudioSource audioSource; // Tham chiếu đến AudioSource
    public GameObject win; // Tham chiếu đến GameObject win

    private GameObject targetTower; // Lưu trữ tham chiếu đến trụ được chọn để phá hủy

    void Start()
    {
        effectTimer = effectInterval;
        animator = GetComponent<Animator>();

        // Tìm GameManager và lấy EnemySpawner
        GameObject gameManager = GameObject.Find("GameManager");
        if (gameManager != null)
        {
            enemySpawner = gameManager.GetComponent<EnemySpawner>();
        }
        else
        {
            Debug.LogError("GameManager không được tìm thấy.");
        }

        // Tìm SoundWorld và lấy AudioSource
        GameObject soundWorld = GameObject.Find("SoudWorld");
        if (soundWorld != null)
        {
            audioSource = soundWorld.GetComponent<AudioSource>();
        }
        else
        {
            Debug.LogError("SoundWorld không được tìm thấy.");
        }

        // Đổi âm thanh của AudioSource thành âm thanh mới
        if (audioSource != null && newAudioClip != null)
        {
            audioSource.clip = newAudioClip;
            audioSource.Play(); // Bắt đầu phát âm thanh mới
        }
        else
        {
            Debug.LogWarning("AudioSource hoặc AudioClip chưa được gán.");
        }

        // Tìm GameObject "Win" ngay cả khi nó không hoạt động
        GameObject[] allGameObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        foreach (GameObject obj in allGameObjects)
        {
            if (obj.name == "Win")
            {
                win = obj;
                break;
            }
        }

        if (win == null)
        {
            Debug.LogError("GameObject 'Win' không được tìm thấy.");
        }
    }

    void Update()
    {
        effectTimer -= Time.deltaTime;

        if (effectTimer <= 0)
        {
            UseRandomEffect();
            effectTimer = effectInterval;
        }
    }

    void UseRandomEffect()
    {
        int effectIndex;
        do
        {
            effectIndex = Random.Range(0, 3); // Chọn ngẫu nhiên một trong ba hiệu ứng
        } while (effectIndex == lastEffectIndex); // Đảm bảo không chọn hiệu ứng giống nhau liên tiếp

        lastEffectIndex = effectIndex; // Cập nhật chỉ số hiệu ứng đã sử dụng

        switch (effectIndex)
        {
            case 0:
                animator.SetTrigger("Teleport");
                StartCoroutine(ExecuteAfterDelay(0.4f, Teleport));
                break;
            case 1:
                animator.SetTrigger("DestroyTower");

                // Xác định trụ được chọn để phá hủy
                DetermineTargetTower();

                // Tạo hiệu ứng tại vị trí trụ được chọn
                if (targetTower != null && effPrefab != null)
                {
                    Instantiate(effPrefab, targetTower.transform.position, Quaternion.identity);
                }
                else
                {
                    Debug.LogWarning("targetTower hoặc effPrefab chưa được gán.");
                }

                // Bắt đầu Coroutine để phá hủy trụ sau một khoảng thời gian
                StartCoroutine(ExecuteAfterDelay(3f, DestroyTargetTower));
                break;
            case 2:
                animator.SetTrigger("Summon");
                StartCoroutine(ExecuteAfterDelay(0.4f, SummonEnemies));
                break;
        }
    }

    void DetermineTargetTower()
    {
        // Tìm tất cả các GameObject có tag "Tower"
        GameObject[] towers = GameObject.FindGameObjectsWithTag("Tower");
        if (towers.Length > 0)
        {
            // Chọn ngẫu nhiên một trụ
            int towerIndex = Random.Range(0, towers.Length);
            targetTower = towers[towerIndex]; // Lưu trữ tham chiếu đến trụ
        }
    }

    void DestroyTargetTower()
    {
        if (targetTower != null)
        {
            // Phá hủy trụ được chọn
            Destroy(targetTower);
        }
    }

    void SummonEnemies()
    {
        src.clip = Summon;
        src.Play();
        if (enemySpawner == null)
        {
            Debug.LogError("EnemySpawner không được gán.");
            return;
        }

        int randomWaveIndex = Random.Range(0, enemySpawner.waves.Count - 1);
        enemySpawner.StartSpecificWave(randomWaveIndex);
    }

    void Teleport()
    {
        // Dịch chuyển tới một vị trí ngẫu nhiên
        int teleportIndex = Random.Range(0, teleportPositions.Length);
        transform.position = teleportPositions[teleportIndex].position;
    }

    void OnDestroy()
    {
        // Dừng game
        Time.timeScale = 0;

        // Hiển thị màn hình chiến thắng
        if (win != null)
        {
            win.SetActive(true);
            Debug.Log("Boss đã bị phá hủy! Bạn đã chiến thắng!");
        }
        else
        {
            Debug.LogError("GameObject 'Win' chưa được gán.");
        }
    }

    IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay);
        action();
    }
}
