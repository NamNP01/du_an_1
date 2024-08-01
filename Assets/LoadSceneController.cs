﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneController : MonoBehaviour
{
    public void LoadMenuLoginScene()
    {
        SceneManager.LoadScene("MenuLogin");
    }

    public void ExitGame()
    {
        // Thoát game khi chạy build
        Application.Quit();

        // Để kiểm tra khi chạy trong Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
