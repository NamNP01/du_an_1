using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuiltMeleeTower : MonoBehaviour
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
    private TowerMeleeUpgra towerMeleeUpgra;

    // Thêm biến này để lưu trữ prefab của melee unit
    public GameObject meleeUnitPrefab;
    private BasepointSelector basepointSelector;

    void Start()
    {
        towerController = GetComponent<TowerController>();
        buildController = GameObject.FindWithTag("Manager").transform.GetChild(0).gameObject.GetComponent<BuildController>();
        buildCanvas = transform.GetChild(0).gameObject;
        upgradeTouchCanvas = transform.GetChild(1).gameObject;
        prizeText = buildCanvas.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();

        basepointSelector = GameObject.FindObjectOfType<BasepointSelector>(); // Tìm đối tượng BasepointSelector
        towerMeleeUpgra = GetComponent<TowerMeleeUpgra>(); // Tìm thành phần TowerMeleeUpgra

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

                // Tạo melee unit khi xây dựng tháp
                if (meleeUnitPrefab != null)
                {
                    GameObject meleeUnit = Instantiate(meleeUnitPrefab, transform.position, Quaternion.identity);

                    // Gọi ActivateBasepointSelection với melee unit được tạo ra
                    if (basepointSelector != null)
                    {
                        basepointSelector.ActivateBasepointSelection(meleeUnit.GetComponent<Melee>());
                    }

                    // Thêm melee unit vào danh sách spawnedUnits của TowerMeleeUpgra
                    if (towerMeleeUpgra != null)
                    {
                        towerMeleeUpgra.AddSpawnedUnit(meleeUnit);
                    }
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
