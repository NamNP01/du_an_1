using UnityEngine;

public class Bow : MonoBehaviour
{
    public GameObject bulletPrefab; // Tham chiếu đến Prefab của viên đạn
    public Transform firePoint; // Vị trí bắn ra viên đạn
    public float fireRate = 0.5f; // Thời gian chờ giữa các lần bắn
    private float nextFireTime = 0f;
    private Animator ani; // Tham chiếu đến Animator

    void Start()
    {
        ani = GetComponentInParent<Character>().ani; // Lấy tham chiếu đến Animator từ Character
    }

    // Hàm này được gọi trong Update của Character để xử lý bắn cung
    public void UpdateBow()
    {
        // Cập nhật vị trí và hướng của Bow
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Đảm bảo vị trí chuột có z bằng 0
        Vector3 direction = (mousePosition - transform.position).normalized;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward); // Xoay Bow theo hướng chuột

        // Kiểm tra nếu nút chuột trái được nhấn và đã qua thời gian chờ
        if (Input.GetMouseButtonDown(0) && Time.time > nextFireTime)
        {
            ShootBullet(direction); // Bắn viên đạn theo hướng chuột
            nextFireTime = Time.time + fireRate; // Cập nhật thời gian cho lần bắn tiếp theo
            SetAttackAnimationTrigger(angle); // Thiết lập trigger animation tấn công
        }
    }

    void ShootBullet(Vector3 direction)
    {
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity); // Tạo viên đạn
        bullet.GetComponent<Arrow>().SetDirection(direction); // Thiết lập hướng di chuyển cho viên đạn
    }

    void SetAttackAnimationTrigger(float angle)
    {
        // Dựa vào góc để xác định trigger animation tấn công
        if (angle >= -45f && angle < 45f)
        {
            ani.SetTrigger("AttackRight");
        }
        else if (angle >= 45f && angle < 135f)
        {
            ani.SetTrigger("AttackUp");
        }
        else if (angle >= -135f && angle < -45f)
        {
            ani.SetTrigger("AttackDown");
        }
        else
        {
            ani.SetTrigger("AttackLeft");
        }
    }
}
