using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealing : MonoBehaviour
{
    public int healAmount = 10; // Lượng máu hồi mỗi lần
    public float healRange = 5f; // Phạm vi hồi máu
    public float healInterval = 5f; // Thời gian giữa các lần hồi máu
    public GameObject healEffect; // Hiệu ứng hồi máu

    private float nextHealTime;
    public Animator ani;

    void Update()
    {
        if (Time.time >= nextHealTime)
        {
            HealAllies();
            nextHealTime = Time.time + healInterval;
        }
    }

    void HealAllies()
    {
        ani.SetTrigger("Healing");
        Collider2D[] alliesInRange = Physics2D.OverlapCircleAll(transform.position, healRange);
        foreach (Collider2D ally in alliesInRange)
        {
            EnemyHealth enemyHealth = ally.GetComponent<EnemyHealth>();
            if (enemyHealth != null && enemyHealth != this)
            {
                enemyHealth.Heal(healAmount);
                if (healEffect != null)
                {
                    Debug.Log("Healing effect instantiated at: " + ally.transform.position);
                    ShowHealEffect(ally.transform.position);
                }
                else
                {
                    Debug.LogError("Heal effect is not assigned.");
                }
            }
        }
    }
    void ShowHealEffect(Vector3 position)
    {
        if (healEffect != null)
        {
            GameObject effect = Instantiate(healEffect, position, Quaternion.identity);

            // Lấy ParticleSystem từ GameObject
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                // Kích hoạt particlesystem
                particleSystem.Play();

                // Sau 1 giây, dừng particlesystem và hủy bỏ GameObject
                StartCoroutine(StopAndDestroyParticleSystemAfterDelay(effect, 1f));
            }
        }
    }

    IEnumerator StopAndDestroyParticleSystemAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        // Lấy ParticleSystem từ GameObject
        ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
        if (particleSystem != null)
        {
            // Dừng particlesystem từ việc phát ra các particle mới
            particleSystem.Stop();
        }

        // Chờ 1 giây để cho particlesystem hoàn tất mờ dần (nếu có)
        yield return new WaitForSeconds(1f);

        // Hủy bỏ GameObject chứa particlesystem
        Destroy(effect);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, healRange);
    }
}
