using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TowerController : MonoBehaviour
{
//    public enum TowerType { Archer, Stone, Fire, Ice }

//    public TowerType towerType;
//    public GameObject projectilePrefab;
//    public Transform firePoint;
//    public float attackRange = 5f;
//    public float attackRate = 2f; // Attacks per second
//    public int damage;
//    public float circleColour;

//    private float nextAttackTime = 0f;
//    private GameObject target;
//    private GameObject rangeCircle;

//    // Variables specific to the Fire tower
//    [System.Serializable]
//    public struct FireVariables
//    {
//        public int fireDamage;
//        public float damageOverTimeInterval;
//        public bool isDamgeStarted;
//    }
//    public FireVariables fireVariables;

//    // Variables specific to the Ice tower
//    [System.Serializable]
//    public struct IceVariables
//    {
//        public float iceSlowRate;
//        public bool isSlowed;
//    }
//    public IceVariables iceVariables;

//    [System.Serializable]
//    public struct StoneVariables
//    {
//        public int stoneDamage;
//        public float damageOverTimeInterval;
//        public bool isDamgeStarted;
//    }
//    public StoneVariables stoneVariables;

//    public void Start()
//    {
//        rangeCircle = transform.GetChild(3).gameObject;
//        RangeCircle();
//    }
//    void Update()
//    {
//        //if (Time.time >= nextAttackTime)
//        //{
//        //    GameObject targetEnemy = GetNearestEnemy();
//        //    if (targetEnemy != null)
//        //    {
//        //        target = targetEnemy;
//        //        Shoot(targetEnemy);
//        //        nextAttackTime = Time.time + 1f / attackRate;
//        //    }
//        //}
//        //Ability();
//    }

//    void Shoot(GameObject target)
//    {
//        GameObject projectile = Instantiate(projectilePrefab, firePoint.position, Quaternion.identity);
//        ProjectileController projectileController = projectile.GetComponent<ProjectileController>();
//        if (projectileController != null)
//        {
//            projectileController.Seek(target.transform,damage);
//            projectileController.damage = damage;
//        }
//    }

//    GameObject GetNearestEnemy()
//    {
//        GameObject nearestEnemy = null;
//        float shortestDistance = Mathf.Infinity;
//        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
//        foreach (GameObject enemy in enemies)
//        {
//            float distanceToEnemy = Vector2.Distance(transform.position, enemy.transform.position);
//            if (distanceToEnemy < shortestDistance && distanceToEnemy <= attackRange)
//            {
//                nearestEnemy = enemy;
//                shortestDistance = distanceToEnemy;
//            }
//        }
//        return nearestEnemy;
//    }

//    public void Ability()
//    {
//        if (towerType == TowerType.Ice && target != null)
//        {
//            Ice();
//        }
//        else if (towerType == TowerType.Fire && target != null)
//        {
//            Fire();
//        }
//        else if (towerType == TowerType.Stone && target != null)
//        {
//            Stone();
//        }
//    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.color = Color.red;
//        Gizmos.DrawWireSphere(transform.position, attackRange);
//    }

//    public void Ice()
//    {
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
//        foreach (Collider2D collider in colliders)
//        {
//            if (collider.gameObject.CompareTag("Enemy"))
//            {
//                EnemyController enemyController = collider.GetComponent<EnemyController>();
//                if (enemyController != null)
//                {
//                    bool wasInsideRange = enemyController.isSlowed; // Check if the enemy was previously inside the range
//                    bool isInsideRange = Vector2.Distance(transform.position, collider.transform.position) <= attackRange;

//                    if (isInsideRange)
//                    {
//                        if (!enemyController.isSlowed)
//                        {
//                            // Enemy is within the attack range and not already slowed
//                            enemyController.moveSpeed -= iceVariables.iceSlowRate;
//                            enemyController.isSlowed = true;
//                            Debug.Log("Slowed: " + enemyController.moveSpeed);
//                        }
//                    }
//                    else if (wasInsideRange)
//                    {
//                        // Enemy was inside the attack range but now outside, increase its speed
//                        enemyController.moveSpeed += iceVariables.iceSlowRate;
//                        enemyController.isSlowed = false; // Reset the slow status
//                        Debug.Log("Speed increased: " + enemyController.moveSpeed);
//                    }
//                }
//            }
//        }
//    }

//    public void Fire()
//    {
//        if (!fireVariables.isDamgeStarted)
//        {
//            StartCoroutine(ApplyFireDamageOverTime());
//            fireVariables.isDamgeStarted = true;
//        }
//    }
//    public void Stone()
//    {
//        if (!stoneVariables.isDamgeStarted)
//        {
//            StartCoroutine(ApplyStoneDamageOverTime());
//            stoneVariables.isDamgeStarted = true;
//        }
//    }
//    IEnumerator ApplyFireDamageOverTime()
//    {
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
//        foreach (Collider2D collider in colliders)
//        {
//            if (collider.gameObject.CompareTag("Enemy"))
//            {
//                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
//                if (enemyHealth != null)
//                {
//                    enemyHealth.TakeDamage(fireVariables.fireDamage);
//                    Debug.Log("Damaged: " + enemyHealth.currentHealth);
//                }
//            }
//        }

//        yield return new WaitForSeconds(fireVariables.damageOverTimeInterval);

//        if (towerType == TowerType.Fire && target != null)
//        {
//            StartCoroutine(ApplyFireDamageOverTime());
//        }
//    }
//    IEnumerator ApplyStoneDamageOverTime()
//    {
//        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, attackRange);
//        foreach (Collider2D collider in colliders)
//        {
//            if (collider.gameObject.CompareTag("Enemy"))
//            {
//                EnemyHealth enemyHealth = collider.GetComponent<EnemyHealth>();
//                if (enemyHealth != null)
//                {
//                    enemyHealth.TakeDamage(stoneVariables.stoneDamage);
//                    Debug.Log("Damaged: " + enemyHealth.currentHealth);
//                }
//            }
//        }

//        yield return new WaitForSeconds(stoneVariables.damageOverTimeInterval);

//        if (towerType == TowerType.Stone && target != null)
//        {
//            StartCoroutine(ApplyStoneDamageOverTime());
//        }
//    }
//    public void RangeCircle()
//    {
//        if(rangeCircle != null)
//        {
//            rangeCircle.transform.localScale = new Vector2(attackRange + attackRange, attackRange + attackRange);
//            SpriteRenderer spriteRenderer = rangeCircle.GetComponent<SpriteRenderer>();
//            Color spriteColor = spriteRenderer.color;

//            spriteColor.a = circleColour;

//            spriteRenderer.color = spriteColor;
//        }
//        else
//        {
//            rangeCircle = transform.GetChild(3).gameObject;
//            return;
//        }
//    }
}
