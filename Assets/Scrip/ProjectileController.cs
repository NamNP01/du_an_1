using System.Collections;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    private Transform target;
    public float speed = 10f;
    public int damage = 6;
    public ArcherTower.TowerType towerType;
    private float slowAmount;
    private float slowTime;

    public GameObject explosionPrefab; // Biến này để chứa prefab nổ

    public void Seek(Transform target, int damage, ArcherTower.TowerType towerType, float slowAmount, float slowTime)
    {
        this.target = target;
        this.damage = damage;
        this.towerType = towerType;
        this.slowAmount = slowAmount;
        this.slowTime = slowTime;
    }

    void Update()
    {
        if (target == null)
        {
            Destroy(gameObject); // Nếu mất mục tiêu, phá hủy đạn
            return;
        }

        Vector2 direction = (Vector2)target.position - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;

        float distanceThisFrame = speed * Time.deltaTime;
        transform.Translate(direction.normalized * distanceThisFrame, Space.World);

        // Kiểm tra xem đạn đã đủ gần mục tiêu chưa
        if (direction.magnitude <= distanceThisFrame)
        {
            HitTarget();
        }
    }

    void HitTarget()
    {
        if (towerType == ArcherTower.TowerType.Bomb)
        {
            Instantiate(explosionPrefab, transform.position, transform.rotation);
            DealDamage(target.gameObject);
        }
        else
        {
            EnemyHealth enemyHealth = target.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            if (towerType == ArcherTower.TowerType.Magic)
            {
                EnemyController enemyController = target.GetComponent<EnemyController>();
                if (enemyController != null)
                {
                    StartCoroutine(SlowEnemy(enemyController, slowAmount, slowTime));
                }
            }
        }
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (towerType == ArcherTower.TowerType.Bomb && collision.gameObject.CompareTag("Enemy") && collision.gameObject != target.gameObject)
        {
            DealDamage(collision.gameObject);
        }
    }

    void DealDamage(GameObject enemy)
    {
        EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(damage);
        }
    }

    IEnumerator SlowEnemy(EnemyController enemy, float slowAmount, float duration)
    {
        enemy.SlowDown(slowAmount);
        yield return new WaitForSeconds(duration);
        enemy.ResetSpeed(slowAmount);
    }
}
