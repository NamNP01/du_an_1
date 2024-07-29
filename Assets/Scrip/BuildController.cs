using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class BuildController : MonoBehaviour
{
    public float gridMovementSize; // Kích thước ô lưới

    public GameObject spawnedPrefab; // Prefab được sinh ra
    private bool isDragging; // Cờ để kiểm tra trạng thái kéo

    // Danh sách các vị trí hợp lệ
    public List<GameObject> validPositionObjects;

    private SpriteRenderer spriteRenderer; // Tham chiếu đến SpriteRenderer của trụ
    private Color originalColor; // Màu gốc của trụ
    public Color invalidColor = Color.red; // Màu đỏ khi vị trí không hợp lệ
    public Color validColor = Color.green; // Màu xanh khi vị trí hợp lệ

    void Update()
    {
        if (isDragging && spawnedPrefab != null)
        {
            HandleDrag(spawnedPrefab);
        }
    }

    public void DragStart(GameObject prefab)
    {
        if (spawnedPrefab != null)
        {
            Destroy(spawnedPrefab); // Hủy prefab hiện tại nếu đã tồn tại
        }
        if (prefab != null)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = SnapToGrid(mousePos); // Căn chỉnh vị trí theo lưới
            spawnedPrefab = Instantiate(prefab, mousePos, prefab.transform.rotation); // Sinh ra prefab tại vị trí căn chỉnh
            spriteRenderer = spawnedPrefab.GetComponent<SpriteRenderer>(); // Khởi tạo SpriteRenderer
            originalColor = spriteRenderer.color; // Lưu trữ màu gốc của trụ
            isDragging = true;
        }
    }

    private void HandleDrag(GameObject prefab)
    {
        if (!isDragging || prefab == null) return; // Kiểm tra trạng thái kéo và prefab

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePos = SnapToGrid(mousePos); // Căn chỉnh vị trí theo lưới
            prefab.transform.position = mousePos;

            // Kiểm tra xem vị trí hiện tại có hợp lệ hay không
            if (IsValidPosition(mousePos) && IsAllowedPosition(mousePos))
            {
                spriteRenderer.color = validColor; // Đặt màu xanh khi vị trí hợp lệ
            }
            else
            {
                spriteRenderer.color = invalidColor; // Đặt màu đỏ khi vị trí không hợp lệ
            }

            if (Input.GetMouseButtonUp(0))
            {
                DragEnd();
            }
        }
    }

    public void DragEnd()
    {
        if (spawnedPrefab != null)
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePos = SnapToGrid(mousePos); // Căn chỉnh vị trí theo lưới
                if (IsValidPosition(mousePos) && IsAllowedPosition(mousePos))
                {
                    spawnedPrefab.layer = LayerMask.NameToLayer("Tower"); // Đặt layer cho prefab
                    spriteRenderer.color = originalColor; // Khôi phục màu gốc
                }
                else
                {
                    Destroy(spawnedPrefab); // Hủy prefab nếu vị trí không hợp lệ
                }
                isDragging = false;
            }
            else
            {
                Destroy(spawnedPrefab); // Hủy prefab nếu con trỏ ở trên UI
                isDragging = false;
            }
        }
    }

    private Vector2 SnapToGrid(Vector2 position)
    {
        // Định nghĩa kích thước mỗi ô lưới
        float gridSize = gridMovementSize;

        // Tính toán vị trí căn chỉnh bằng cách làm tròn tọa độ đến điểm lưới gần nhất
        float snappedX = Mathf.Round(position.x / gridSize) * gridSize;
        float snappedY = Mathf.Round(position.y / gridSize) * gridSize;

        // Tạo một Vector2 mới với tọa độ đã căn chỉnh
        Vector2 snappedPosition = new Vector2(snappedX, snappedY);

        return snappedPosition;
    }

    private bool IsValidPosition(Vector2 position)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(position, 0.1f); // Thay đổi 0.1f thành bán kính phù hợp
        foreach (Collider2D collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Tower"))
            {
                return false; // Tower tồn tại tại vị trí, vị trí không hợp lệ
            }
        }
        return true; // Không có tower tại vị trí, vị trí hợp lệ
    }

    private bool IsAllowedPosition(Vector2 position)
    {
        // Kiểm tra xem vị trí hiện tại có nằm trong danh sách vị trí hợp lệ
        foreach (GameObject validPosObj in validPositionObjects)
        {
            Vector2 validPos = validPosObj.transform.position;
            if (Vector2.Distance(position, validPos) < 0.1f) // Kiểm tra khoảng cách với độ chính xác phù hợp
            {
                return true;
            }
        }
        return false;
    }
}
