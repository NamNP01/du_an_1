using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildTower : MonoBehaviour
{
    [Space(30)]
    [Header("Prizes")]
    public int prize;

    private TextMeshProUGUI prizeText;

    private GameObject buildCanvas;
    private GameObject upgradeTouchCanvas;
    private TowerController towerController;

    private Button buildButton;
    private Button destroyBuildButton;

    private BuildController buildController;
    void Start()
    {
        towerController = GetComponent<TowerController>();
        buildController = GameObject.FindWithTag("Manager").transform.GetChild(0).gameObject.GetComponent<BuildController>();
        buildCanvas = transform.GetChild(0).gameObject;
        upgradeTouchCanvas = transform.GetChild(1).gameObject;
        prizeText = buildCanvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        buildCanvas.SetActive(true);
        upgradeTouchCanvas.SetActive(false);
        AssignButtonFunction();
        if (towerController != null)
        {
            towerController.enabled = false;
        }
    }

    void Update()
    {
        if (prizeText != null)
        {
            prizeText.text = "Prize: " + prize;

            if (prize < CoinManager.instance.currentCoins)
            {
                prizeText.color = Color.white;
            }
            else if (prize > CoinManager.instance.currentCoins)
            {
                prizeText.color = Color.red;
            }
        }
    }

    public void Build()
    {
        if (CoinManager.instance.SpendCoins(prize))
        {
            if (towerController != null)
            {
                towerController.enabled = true;
                upgradeTouchCanvas.SetActive(true);
                buildCanvas.SetActive(false);
                gameObject.layer = LayerMask.NameToLayer("Tower");
                ArcherTower archerMelee = GetComponent<ArcherTower>();
                if (archerMelee != null)
                {
                    archerMelee.isBuilt = true; // Đặt isBuilt thành true sau khi trụ được xây dựng
                }
                if (buildController != null)
                {
                    buildController.spawnedPrefab = null;
                }
            }
        }
    }

    public void DestroyBuild()
    {
        Destroy(gameObject);
        Debug.Log(gameObject);
    }

    public void AssignButtonFunction()
    {
        if (buildCanvas != null)
        {
            buildButton = buildCanvas.transform.GetChild(0).gameObject.GetComponent<Button>();
            destroyBuildButton = buildCanvas.transform.GetChild(1).gameObject.GetComponent<Button>();

            buildButton.onClick.RemoveAllListeners();
            destroyBuildButton.onClick.RemoveAllListeners();

            buildButton.onClick.AddListener(Build);
            destroyBuildButton.onClick.AddListener(DestroyBuild);
        }
    }
}
