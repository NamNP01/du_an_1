using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float bulletSpeed = 10f;
    public int damage = 2; // Sát thương của mũi tên

    void Start()
    {
        Destroy(gameObject, 2f); // Hủy viên đạn sau 2 giây
    }

    public void SetDirection(Vector2 direction)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.velocity = direction.normalized * bulletSpeed; // Thiết lập vận tốc cho viên đạn

        // Tính toán góc quay và áp dụng nó cho viên đạn
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = rotation;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            EnemyHealth enemyHealth = collision.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage); // Gây sát thương cho kẻ địch
            }
            Destroy(gameObject); // Hủy viên đạn sau khi va chạm
        }
        if (collision.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject); // Hủy viên đạn sau khi va chạm
        }
    }
}
