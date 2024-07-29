using UnityEngine;

public class Melee : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float lineOfSight; // Phạm vi nhìn thấy enemy

    [Header("Attack")]
    public int attackDamage; // Sát thương mỗi lần tấn công
    public float attackRange;
    public float speedAttack; // Tốc độ tấn công (thời gian giữa mỗi lần tấn công)
    private float nextAttackTime; // Thời gian tiếp theo có thể tấn công

    private Transform enemy;
    public Vector3 basePoint; // Điểm cơ sở để quay về
    public bool isAttacking = false;
    public Animator ani;
    private MeleeHeal meleeHeal;

    private void Start()
    {
        basePoint = transform.position; // Gán vị trí hiện tại của người chơi
        meleeHeal = GetComponent<MeleeHeal>(); // Lấy tham chiếu đến MeleeHeal
    }

    void Update()
    {
        if (enemy != null && enemy.GetComponent<EnemyHealth>().currentHealth <= 0)
        {
            enemy = null;
        }

        FindClosestEnemy();

        if (enemy != null)
        {
            float distanceFromEnemy = Vector2.Distance(enemy.position, transform.position);
            EnemyController enemyController = enemy.GetComponent<EnemyController>();
            if (enemy.GetComponent<EnemyHealth>().currentHealth <= 0)
            {
                enemy = null;
                isAttacking = false;
                ReturnToBase();
                return; // Thoát khỏi phương thức Update để không tiếp tục xử lý với enemy đã chết
            }
            // Nếu enemy có thể bị chặn (canBlock là true)
            if (enemyController != null && enemyController.canBlock)
            {
                // Nếu khoảng cách trong phạm vi nhìn thấy nhưng ngoài phạm vi tấn công
                if (distanceFromEnemy < lineOfSight && distanceFromEnemy > attackRange)
                {
                    // Chặn enemy lại
                    enemyController.stopWalking();
                    meleeHeal.SetEnemy(enemy); // Gán enemy cho MeleeHeal script

                    isAttacking = true;
                    // Di chuyển về phía enemy
                    ani.SetBool("Move", true);
                    transform.position = Vector2.MoveTowards(transform.position, enemy.position, speed * Time.deltaTime);

                    // Điều chỉnh hướng của kẻ địch về phía enemy
                    if (transform.position.x < enemy.position.x)
                    {
                        transform.localScale = new Vector2(-1, 1);
                    }
                    else
                    {
                        transform.localScale = new Vector2(1, 1);
                    }
                }
                // Nếu khoảng cách trong phạm vi tấn công và có thể tấn công
                else if (distanceFromEnemy <= attackRange && nextAttackTime < Time.time)
                {
                    isAttacking = true;
                    nextAttackTime = Time.time + speedAttack; // Cập nhật thời gian tiếp theo có thể tấn công
                    Attack();
                }
            }
        }

        // Di chuyển về điểm cơ sở nếu không có kẻ địch hoặc không tấn công
        if (!isAttacking && transform.position != basePoint)
        {
            ReturnToBase();
            if (meleeHeal.currentHealth < meleeHeal.maxHealth)
            {
                StartCoroutine(meleeHeal.RegenerateHealth());
            }
        }
    }

    void ReturnToBase()
    {
        // Logic để quay về điểm cơ sở (basePoint)
        transform.position = Vector2.MoveTowards(transform.position, basePoint, speed * Time.deltaTime);
        // Điều chỉnh hướng khi quay trở lại basePoint
        if (transform.position.x < basePoint.x)
        {
            transform.localScale = new Vector2(-1, 1);
            ani.SetBool("Move", true);
        }
        else if (transform.position.x > basePoint.x)
        {
            transform.localScale = new Vector2(1, 1);
            ani.SetBool("Move", true);
        }
        else if (transform.position.x == basePoint.x)
        {
            ani.SetBool("Move", false);
        }
    }

    void FindClosestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (GameObject go in enemies)
        {
            float distance = Vector2.Distance(transform.position, go.transform.position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestEnemy = go.transform;
            }
        }

        enemy = closestEnemy;
    }

    void Attack()
    {
        if (enemy != null)
        {
            ani.SetTrigger("Attack");
            EnemyHealth enemyHealth = enemy.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(attackDamage); // Gây sát thương cho kẻ địch

                if (enemyHealth.currentHealth <= 0)
                {
                    // Enemy đã chết, quay lại vị trí ban đầu
                    enemy = null;
                    isAttacking = false;
                }
            }
        }
        Debug.Log("Tấn công kẻ địch!");
    }

    // Vẽ phạm vi nhìn thấy và phạm vi tấn công trong chế độ Editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, lineOfSight);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}
