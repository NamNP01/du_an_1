using UnityEngine;

public class Character : MonoBehaviour
{
    public Animator ani;
    private Vector3 lastMovement; // Biến lưu hướng di chuyển cuối cùng
    private Bow bow; // Tham chiếu đến Bow
    // Start is called before the first frame update
    void Start()
    {
        lastMovement = new Vector3(0, -1, 0); // Đặt hướng mặc định ban đầu
        bow = GetComponentInChildren<Bow>(); // Lấy tham chiếu đến Bow
    }

    // Update is called once per frame
    void Update()
    {
        // Xử lý di chuyển
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, vertical, 0f).normalized;
        transform.Translate(movement * 5f * Time.deltaTime);

        if (movement.sqrMagnitude > 0)
        {
            lastMovement = movement; // Cập nhật hướng di chuyển cuối cùng
        }

        ani.SetFloat("Horizontal", lastMovement.x);
        ani.SetFloat("Vertical", lastMovement.y);
        ani.SetFloat("Speed", movement.sqrMagnitude);

        // Xử lý bắn cung
        if (bow != null)
        {
            bow.UpdateBow();
        }
    }
}
