using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;


public class LevelSelection : MonoBehaviour
{    
    [SerializeField] private bool UnlockedLevels;
    public GameObject[] star;    
    public Image Unlock;
    public TMP_Text text;
    public Sprite starSprite;

    private void Start()
    {
        //PlayerPrefs.DeleteAll();
    }
    private void Update()
    {
        UpdateLevelImage();
        UpdateLevelStatus();
    }
        
    private void UpdateLevelStatus()
    {

        int preLevelNum = int.Parse(gameObject.name) - 1;
        if (PlayerPrefs.GetInt("lv" + preLevelNum) > 0)
        {
            UnlockedLevels = true;
        }
    }
    private void UpdateLevelImage()
    {
        if (!UnlockedLevels)
        {
            Unlock.gameObject.SetActive(true);
            for (int i = 0; i < star.Length; i++)
            {
                star[i].gameObject.SetActive(false);
            }
            text.gameObject.SetActive(false);
        }
        else
        {
            Unlock.gameObject.SetActive(false);
            for (int i = 0; i < star.Length; i++)
            {
                star[i].gameObject.SetActive(true);
            }
            text.gameObject.SetActive(true);
            for (int i = 0; i < PlayerPrefs.GetInt("lv" + gameObject.name); i++)
            {
                star[i].gameObject.GetComponent<Image>().sprite = starSprite;
            }
        }
    } 
    public void UnlockScene(string level)
    {
        if (UnlockedLevels)
        {
            SceneManager.LoadScene(level);
        }
    }
}
