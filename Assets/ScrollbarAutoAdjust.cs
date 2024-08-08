using UnityEngine;
using UnityEngine.UI;

public class ScrollbarAutoAdjust : MonoBehaviour
{
    public ScrollRect scrollRect; // Gán ScrollRect từ Inspector
    public Scrollbar scrollbar; // Gán Scrollbar từ Inspector
    public RectTransform content; // Gán RectTransform của nội dung từ Inspector

    void Start()
    {
        // Gọi hàm để điều chỉnh scrollbar khi bắt đầu
        AdjustScrollbar();
    }

    void AdjustScrollbar()
    {
        // Lấy chiều cao của content và viewport
        float contentHeight = content.rect.height;
        float viewportHeight = scrollRect.viewport.rect.height;

        // Tính toán và đặt giá trị size cho scrollbar
        scrollbar.size = viewportHeight / contentHeight;

        // Đảm bảo giá trị không vượt quá 1
        if (scrollbar.size > 1)
        {
            scrollbar.size = 1;
        }
    }
}
