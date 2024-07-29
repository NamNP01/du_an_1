using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ArcherTower : MonoBehaviour
{
    public enum TowerType { Archer, Bomb, Fire, Magic }

    public TowerType towerType;
    public GameObject projectilePrefab;
    public Transform firePoint;
    public float attackRange = 5f;
    public float attackRate = 2f; // Attacks per second
    public int damage=4;
    public float circleColour;

    private float nextAttackTime = 0f;
    private GameObject target;
    private GameObject rangeCircle;
    public GameObject CirclePoint;

    [Header("Audio")]
    public AudioSource src;
    public AudioClip Attack;

    public Animator animator; // Add an Animator reference

    public bool isBuilt = false; // Cờ để kiểm tra trạng thái xây dựng

    // Variables specific to the Fire tower
    [System.Serializable]
    public struct FireVariables
    {
        public int fireDamage;
        public float damageOverTimeInterval;
        public bool isDamgeStarted;
    }
    public FireVariables fireVariables;

    // Variables specific to the Ice tower
    [System.Serializable]
    public struct MagicVariables
    {
        public float slowTime;
        public float iceSlowRate;
        public bool isSlowed;
    }
    public MagicVariables iceVariables;

    [System.Serializable]
    public struct StoneVariables
    {
        public int stoneDamage;
        public float damageOverTimeInterval;
        public bool isDamgeStarted;
    }
    public StoneVariables stoneVariables;

    void Start()
    {
        rangeCircle = CirclePoint.transform.GetChild(3).gameObject;
        RangeCircle();
    }

    void Update()
    {
        if (!isBuilt) return; // Không làm gì nếu trụ chưa được xây dựng

        if (Time.time >= nextAttackTime)
        {
            GameObject targetEnemy = GetNearestEnemy();
            if (targetEnemy != null)
            {
                target = targetEnemy;
                ChangeAnimationBasedOnEnemyPosition(targetEnemy); // Change animation based on enemy position
                Shoot(targetEnemy);
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        Ability();
    }

    void Shoot(GameObject target)
    {
        Vector3 directionToEnemy = target.transform.position - transform.position;
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

        if (angle >= -45 && angle <= 45)
        {
            // Right
            animator.SetTrigger("AttackRight");
        }
        else if (angle > 45 && angle <= 135)
        {
            // Up (Back)
            animator.SetTrigger("AttackBack");
        }
        else if (angle < -45 && angle >= -135)
        {
            // Down (Front)
            animator.SetTrigger("AttackFront");
        }
        else
        {
            // Left
            animator.SetTrigger("AttackLeft");
        }
        src.clip = Attack;
        src.Play();
        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
        if (projectileController != null)
        {
            float slowAmount = towerType == TowerType.Magic ? iceVariables.iceSlowRate : 0f;
            float slowTime = towerType == TowerType.Magic ? iceVariables.iceSlowRate : 0f;
            projectileController.Seek(target.transform, damage, towerType, slowAmount,slowTime); // Pass slowAmount to projectile
            projectileController.damage = damage;
        }
    }



    GameObject GetNearestEnemy()
    {
        GameObject nearestEnemy = null;
        float shortestDistance = Mathf.Infinity;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject enemy in enemies)
        {
            float distanceToEnemy = Vector2.Distance(CirclePoint.transform.position, enemy.transform.position);
            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
            {
                nearestEnemy = enemy;
                shortestDistance = distanceToEnemy;
            }
        }
        return nearestEnemy;
    }

    public void Ability()
    {
        if (towerType == TowerType.Magic && target != null)
        {
            //Ice();
        }
        else if (towerType == TowerType.Fire && target != null)
        {
            Fire();
        }
        else if (towerType == TowerType.Bomb && target != null)
        {
            Stone();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(CirclePoint.transform.position, attackRange);
    }

    //public void Ice()
    //{
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(CirclePoint.transform.position, attackRange);
    //    foreach (Collider2D collider in colliders)
    //    {
    //        if (collider.gameObject.CompareTag("Enemy"))
    //        {
    //            EnemyController enemyController = collider.GetComponent<EnemyController>();
    //            if (enemyController != null)
    //            {
    //                bool wasInsideRange = enemyController.isSlowed; // Check if the enemy was previously inside the range
    //                bool isInsideRange = Vector2.Distance(CirclePoint.transform.position, collider.transform.position) <= attackRange;

    //                if (isInsideRange)
    //                {
    //                    if (!enemyController.isSlowed)
    //                    {
    //                        // Enemy is within the attack range and not already slowed
    //                        enemyController.moveSpeed -= iceVariables.iceSlowRate;
    //                        enemyController.isSlowed = true;
    //                        Debug.Log("Slowed: " + enemyController.moveSpeed);
    //                    }
    //                }
    //                else if (wasInsideRange)
    //                {
    //                    // Enemy was inside the attack range but now outside, increase its speed
    //                    enemyController.moveSpeed += iceVariables.iceSlowRate;
    //                    enemyController.isSlowed = false; // Reset the slow status
    //                    Debug.Log("Speed increased: " + enemyController.moveSpeed);
    //                }
    //            }
    //        }
    //    }
    //}

    public void Fire()
    {
        if (!fireVariables.isDamgeStarted)
        {
            StartCoroutine(ApplyFireDamageOverTime());
            fireVariables.isDamgeStarted = true;
        }
    }

    public void Stone()
    {
        if (!stoneVariables.isDamgeStarted)
        {
            StartCoroutine(ApplyStoneDamageOverTime());
            stoneVariables.isDamgeStarted = true;
        }
    }

    IEnumerator ApplyFireDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(CirclePoint.transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(fireVariables.fireDamage);
                    Debug.Log("Damaged: " + enemyHealth.currentHealth);
                }
            }
        }

        yield return new WaitForSeconds(fireVariables.damageOverTimeInterval);

        if (towerType == TowerType.Fire && target != null)
        {
            StartCoroutine(ApplyFireDamageOverTime());
        }
    }

    IEnumerator ApplyStoneDamageOverTime()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(CirclePoint.transform.position, attackRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.CompareTag("Enemy"))
            {
                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(stoneVariables.stoneDamage);
                    Debug.Log("Damaged: " + enemyHealth.currentHealth);
                }
            }
        }

        yield return new WaitForSeconds(stoneVariables.damageOverTimeInterval);

        if (towerType == TowerType.Bomb && target != null)
        {
            StartCoroutine(ApplyStoneDamageOverTime());
        }
    }

    public void RangeCircle()
    {
        if (rangeCircle != null)
        {
            rangeCircle.transform.localScale = new Vector2(attackRange + attackRange, attackRange + attackRange);
            SpriteRenderer spriteRenderer = rangeCircle.GetComponent<SpriteRenderer>();
            Color spriteColor = spriteRenderer.color;

            spriteColor.a = circleColour;

            spriteRenderer.color = spriteColor;
        }
        else
        {
            rangeCircle = transform.GetChild(3).gameObject;
            return;
        }
    }

    void ChangeAnimationBasedOnEnemyPosition(GameObject enemy)
    {
        Vector3 directionToEnemy = enemy.transform.position - transform.position;
        float angle = Mathf.Atan2(directionToEnemy.y, directionToEnemy.x) * Mathf.Rad2Deg;

        if (angle >= -45 && angle <= 45)
        {
            // Right
            animator.SetTrigger("Right");
        }
        else if (angle > 45 && angle <= 135)
        {
            // Up (Back)
            animator.SetTrigger("Back");
        }
        else if (angle < -45 && angle >= -135)
        {
            // Down (Front)
            animator.SetTrigger("Front");
        }
        else
        {
            // Left
            animator.SetTrigger("Left");
        }
    }
}

