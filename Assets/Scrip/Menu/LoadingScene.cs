using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    public GameObject loadingScene;
    public Slider slider;
    public Text progressText;

    public void LoadLevel (int sceneIndex)
    {
        loadingScene.SetActive(true);
        StartCoroutine(LoadAsynchronously(sceneIndex));
    }
    IEnumerator LoadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);
        
        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress/ .9f);
            slider.value = progress;
            yield return null;
            progressText.text = progress * 100f + "%";
        }
    }
}
