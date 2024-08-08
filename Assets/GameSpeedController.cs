using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSpeedController : MonoBehaviour
{
    private int speedMode = 0;
    private float[] speedLevels = { 1f, 1.5f, 2f };
    private string[] speedLabels = { ">", ">>", ">>>" };

    public Text speedText; // Thêm biến này để tham chiếu tới Text UI

    public void ToggleSpeed()
    {
        speedMode = (speedMode + 1) % speedLevels.Length;
        Time.timeScale = speedLevels[speedMode];
        UpdateSpeedText();
    }

    private void UpdateSpeedText()
    {
        speedText.text = speedLabels[speedMode];
    }
}
