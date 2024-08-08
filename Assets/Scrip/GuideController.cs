using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GuideController : MonoBehaviour
{
    public Image guideImage;
    public TextMeshProUGUI pageText;
    public Sprite[] guideSprites;
    public Button nextButton;
    public Button previousButton;
    public GameObject guidePanel; // Panel chứa Guide

    private int currentPage = 0;
    public CastleHealth castleHealth; // Thêm tham chiếu đến CastleHealth

    void Start()
    {
        UpdateGuide();

        nextButton.onClick.AddListener(NextPage);
        previousButton.onClick.AddListener(PreviousPage);

        guidePanel.SetActive(false); // Bắt đầu không hiển thị Guide
    }

    void NextPage()
    {
        if (currentPage < guideSprites.Length - 1)
        {
            currentPage++;
            UpdateGuide();
        }
    }

    void PreviousPage()
    {
        if (currentPage > 0)
        {
            currentPage--;
            UpdateGuide();
        }
    }

    void UpdateGuide()
    {
        guideImage.sprite = guideSprites[currentPage];
        pageText.text = (currentPage + 1) + "/" + guideSprites.Length;

        previousButton.interactable = currentPage > 0;
        nextButton.interactable = currentPage < guideSprites.Length - 1;
    }

    public void OpenGuide()
    {
        if (!castleHealth.IsPaused) // Chỉ mở Guide khi MenuPause không hoạt động
        {
            guidePanel.SetActive(true);
            Time.timeScale = 0f; // Dừng game khi mở Guide
        }
    }

    public void CloseGuide()
    {
        guidePanel.SetActive(false);
        Time.timeScale = 1f; // Tiếp tục game khi đóng Guide
    }
}
