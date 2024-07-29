using System.Collections;
using UnityEngine;

public class MeleeHeal : MonoBehaviour
{
    public int maxHealth = 100;
    public int currentHealth;
    public Animator ani;
    private Transform enemy;
    private TowerMeleeUpgra towerMeleeUpgra;
    private bool isRegenerating = false;
    public int Heal = 2;

    [Header("Healing Effects")]
    public GameObject healingEffect; // Hiệu ứng hồi máu

    void Start()
    {
        currentHealth = maxHealth;
        towerMeleeUpgra = GameObject.FindObjectOfType<TowerMeleeUpgra>();

        // Nếu không có GameObject hiệu ứng hồi máu, bạn có thể tạo một đối tượng hiệu ứng tại đây hoặc gán từ Inspector
        if (healingEffect == null)
        {
            Debug.LogWarning("Healing Effect GameObject not assigned!");
        }
    }

    public void TakeDamage(int damage)
    {
        ani.SetTrigger("Hurt");
        if (currentHealth <= 0)
            return;

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            ani.SetTrigger("Hurt");
        }
    }

    private void Die()
    {
        if (enemy != null)
        {
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemyController != null)
            {
                enemyController.ResetBlockStatus();
            }
        }

        if (towerMeleeUpgra != null)
        {
            towerMeleeUpgra.RemoveSpawnedUnit(gameObject); // Xóa khỏi danh sách spawnedUnits
            towerMeleeUpgra.OnMeleeUnitDestroyed(); // Báo cho TowerMeleeUpgra khi Melee bị tiêu diệt
        }

        Destroy(gameObject, 0.3f);
    }

    public void SetEnemy(Transform enemyTransform)
    {
        enemy = enemyTransform;
    }

    public IEnumerator RegenerateHealth()
    {
        if (isRegenerating) yield break; // Ngăn chặn chạy nhiều coroutine cùng lúc
        isRegenerating = true;

        // Bật hiệu ứng hồi máu
        if (healingEffect != null)
        {
            healingEffect.SetActive(true); // Bật GameObject hiệu ứng hồi máu
        }

        yield return new WaitForSeconds(2f); // Chờ 2 giây trước khi bắt đầu hồi máu

        while (currentHealth < maxHealth)
        {
            currentHealth += Heal; // Hồi 2 máu mỗi giây
            currentHealth = Mathf.Min(currentHealth, maxHealth); // Đảm bảo không vượt quá maxHealth
            yield return new WaitForSeconds(1f); // Chờ 1 giây trước lần hồi máu tiếp theo
        }

        // Tắt hiệu ứng hồi máu
        if (healingEffect != null)
        {
            healingEffect.SetActive(false); // Tắt GameObject hiệu ứng hồi máu
        }

        isRegenerating = false;
    }
}
