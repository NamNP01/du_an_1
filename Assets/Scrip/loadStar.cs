using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class loadStar : MonoBehaviour
{
    // Start is called before the first frame update
    public Text star;
    // Update is called once per frame
    void Update()
    {
        UpdateStar();
    }
    private void UpdateStar()
    {
        int sum = 0;
        for (int i = 1; i < 5; i++)
        {
            sum += PlayerPrefs.GetInt("lv" + i.ToString());
        }
        star.text = sum + "/" + 12;
    }
}
