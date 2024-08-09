using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CastleHealth : MonoBehaviour
{
    public int currenthealth;
    public int numOfHearts;
    public GameObject _GameOver;
    public bool IsPaused;
    private float startingTime = 0f;

    public GameObject PauseMenu;
    public GuideController guideController; // Tham chiếu đến GuideController để kiểm tra trạng thái Guide

    public Text timerText;
    public float currentTime = 0f;

    public TextMeshProUGUI HeartsText;
    public Image[] star;
    public Sprite starSprite;
    public GameObject[] Continue;

    public float previousSpeed;

    void Start()
    {
        currenthealth = numOfHearts;
        HeartsText.text = "" + currenthealth;

        startingTime = Time.time;

        previousSpeed = GameSpeedController.currentSpeed;
    }

    void Update()
    {
        currentTime = Time.time - startingTime;
        UpdateTimerDisplay();

        if (Input.GetKeyDown(KeyCode.Escape) && !guideController.guidePanel.activeSelf) // Không tạm dừng khi Guide đang mở
        {
            if (IsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    public void UpdateStars()
    {
        if (currenthealth < 10)
        {
            Continue[0].SetActive(true);
            Continue[1].SetActive(false);
            Continue[2].SetActive(false);
            star[0].gameObject.GetComponent<Image>().sprite = starSprite;
        }
        else if (currenthealth < 15)
        {
            Continue[1].SetActive(true);
            Continue[2].SetActive(false);
            Continue[0].SetActive(false);

            star[0].gameObject.GetComponent<Image>().sprite = starSprite;
            star[1].gameObject.GetComponent<Image>().sprite = starSprite;
        }
        else
        {
            Continue[2].SetActive(true);
            Continue[1].SetActive(false);
            Continue[0].SetActive(false);
            star[0].gameObject.GetComponent<Image>().sprite = starSprite;
            star[1].gameObject.GetComponent<Image>().sprite = starSprite;
            star[2].gameObject.GetComponent<Image>().sprite = starSprite;
        }
    }

    public void hurt()
    {
        currenthealth -= 1;
        HeartsText.text = "" + currenthealth;
        if (currenthealth <= 0)
        {
            StartCoroutine(GameOverAfterDelay(0f));
        }
    }

    public void gameOver()
    {
        StartCoroutine(GameOverAfterDelay(1f));
        _GameOver.SetActive(true);
        Time.timeScale = 0f;
        IsPaused = true;
    }

    IEnumerator GameOverAfterDelay(float delayTime)
    {
        yield return new WaitForSeconds(delayTime);
        gameOver();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            hurt();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Destroy(collision.gameObject);
            hurt();
        }
    }

    void UpdateTimerDisplay()
    {
        int minutes = Mathf.FloorToInt(currentTime / 60);
        int seconds = Mathf.FloorToInt(currentTime % 60);
        string timeString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = "" + timeString;
    }
    public void Pause()
    {
        if (!PauseMenu.activeSelf)
        {
            if (guideController.guidePanel.activeSelf)
            {
                guideController.CloseGuide(); // Đóng Guide nếu mở
            }

            previousSpeed = GameSpeedController.currentSpeed; // Cập nhật previousSpeed trước khi tạm dừng
            PauseMenu.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }
    }

    public void Resume()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = previousSpeed; // Khôi phục lại tốc độ trước đó
        GameSpeedController.currentSpeed = previousSpeed; // Cập nhật giá trị currentSpeed
        IsPaused = false;
    }

    public void PlayAgain()
    {
        // Đặt lại tốc độ game về mặc định trước khi restart
        GameSpeedController.currentSpeed = 1f;
        Time.timeScale = 1f;

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        IsPaused = false;
    }

    public void Exit()
    {
        SceneManager.LoadScene("LevelMap");
        Time.timeScale = 1f;
    }

    public void OpenGuide()
    {
        if (!guideController.guidePanel.activeSelf)
        {
            if (PauseMenu.activeSelf)
            {
                Resume(); // Đóng PauseMenu nếu mở
            }

            previousSpeed = GameSpeedController.currentSpeed; // Cập nhật previousSpeed trước khi mở guide
            guideController.OpenGuide();
            IsPaused = true;
        }
    }

    public void CloseGuide()
    {
        guideController.CloseGuide();
        Time.timeScale = previousSpeed; // Khôi phục lại tốc độ trước đó
        GameSpeedController.currentSpeed = previousSpeed; // Cập nhật giá trị currentSpeed
        IsPaused = false;
    }
}
