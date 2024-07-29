using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SingleLevel : MonoBehaviour
{
    private int curentStars = 0;
    public int levelIndex;
    public void Back()
    {
        SceneManager.LoadScene("MenuLogin");
    }
    public void Continue()
    {
        SceneManager.LoadScene("LevelMap");
    }
    public void Restart(string level)
    {
        SceneManager.LoadScene(level);
    }
    public void PressStarsButton(int num)
    {
        
        curentStars = num;
        if (curentStars > PlayerPrefs.GetInt("lv" + levelIndex))
        {
            PlayerPrefs.SetInt("lv" + levelIndex, num);
        }
    }
}
