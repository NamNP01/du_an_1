using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    private int speedMode = 0;
    private float[] speedLevels = { 1f, 1.5f, 2f };
    private string[] speedLabels = { ">", ">>", ">>>" };

    public Text speedText;

    public static float currentSpeed = 1f; // Biến tĩnh lưu trữ tốc độ hiện tại

    void Start()
    {
        Time.timeScale = currentSpeed;
        UpdateSpeedText();
    }

    public void ToggleSpeed()
    {
        speedMode = (speedMode + 1) % speedLevels.Length;
        currentSpeed = speedLevels[speedMode];
        Time.timeScale = currentSpeed;
        UpdateSpeedText();
    }

    private void UpdateSpeedText()
    {
        speedText.text = speedLabels[speedMode];
    }
}
