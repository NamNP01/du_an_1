using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    public float attackRange = 0.5f;
    public float attackCooldown = 1.0f;
    public int attackDamage = 10;
    private float attackTimer = 0f;
    private Transform target;
    private EnemyController enemyController;

    private bool isAttacking = false;

    void Start()
    {
        enemyController = GetComponent<EnemyController>();
    }

    void Update()
    {
        FindNearestMelee();

        if (target != null && enemyController != null && enemyController.isBlockByMelee)
        {
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= attackRange)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(Attack());
                }
            }
            else
            {
                isAttacking = false;
                StopCoroutine(Attack());
            }
        }
        else
        {
            isAttacking = false;
            StopCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        while (isAttacking)
        {
            if (target != null)
            {
                // Gọi hàm gây sát thương cho melee unit
                target.GetComponent<MeleeHeal>().TakeDamage(attackDamage);
            }

            // Dừng lại cho đến khi hết cooldown
            yield return new WaitForSeconds(attackCooldown);
        }
    }

    void FindNearestMelee()
    {
        float nearestDistance = Mathf.Infinity;
        Transform nearestMelee = null;

        foreach (GameObject melee in GameObject.FindGameObjectsWithTag("Melee"))
        {
            float distance = Vector3.Distance(transform.position, melee.transform.position);
            if (distance < nearestDistance)
            {
                nearestDistance = distance;
                nearestMelee = melee.transform;
            }
        }

        target = nearestMelee;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
