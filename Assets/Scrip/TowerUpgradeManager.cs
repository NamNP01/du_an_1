using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerUpgradeManager : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public Button touchButton;
    public Button upgradeButton;
    public Button sellButton;
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradePrizeText;
    public TextMeshProUGUI sellPrizeText;
    public List<GameObject> towerPrefabs; // Danh sách các prefab cho các cấp độ trụ
    private TowerController towerController;
    private int currentLevel;
    private int currentUpgradePrize;
    private int currentSellPrize;

    void Start()
    {
        upgradeCanvas.SetActive(false);
        currentLevel = 1;
        currentUpgradePrize = CoinManager.instance.upgradeStartPrize;
        currentSellPrize = CoinManager.instance.sellStartPrize;
        levelText.text = "Level: " + currentLevel.ToString();
        upgradePrizeText.text = "Prize: " + currentUpgradePrize.ToString();
        sellPrizeText.text = "Prize: " + currentSellPrize.ToString();
        towerController = gameObject.GetComponent<TowerController>();
        if (touchButton != null)
        {
            touchButton.onClick.RemoveAllListeners();
            touchButton.onClick.AddListener(Touch);
        }
        if (upgradeButton != null)
        {
            upgradeButton.onClick.RemoveAllListeners();
            upgradeButton.onClick.AddListener(Upgrade);
        }
        if (sellButton != null)
        {
            sellButton.onClick.RemoveAllListeners();
            sellButton.onClick.AddListener(Sell);
        }
    }

    private void Update()
    {
        if (upgradePrizeText != null)
        {
            upgradePrizeText.text = "Prize: " + currentUpgradePrize.ToString();

            if (currentUpgradePrize <= CoinManager.instance.currentCoins)
            {
                upgradePrizeText.color = Color.white;
            }
            else
            {
                upgradePrizeText.color = Color.red;
            }
        }
    }

    public void Upgrade()
    {
        if (currentLevel < towerPrefabs.Count)
        {
            if (CoinManager.instance.SpendCoins(currentUpgradePrize))
            {
                Vector3 position = transform.position;
                Quaternion rotation = transform.rotation;

                Destroy(gameObject);

                GameObject newTower = Instantiate(towerPrefabs[currentLevel], position, rotation);
                TowerUpgradeManager newTowerUpgradeManager = newTower.GetComponent<TowerUpgradeManager>();

                newTowerUpgradeManager.currentLevel = currentLevel + 1;
                newTowerUpgradeManager.currentUpgradePrize = currentUpgradePrize + CoinManager.instance.upgradePlusPrize;
                newTowerUpgradeManager.currentSellPrize = currentSellPrize + CoinManager.instance.sellPlusPrize;

                newTowerUpgradeManager.levelText.text = "Level: " + newTowerUpgradeManager.currentLevel.ToString();
                newTowerUpgradeManager.upgradePrizeText.text = "Prize: " + newTowerUpgradeManager.currentUpgradePrize.ToString();
                newTowerUpgradeManager.sellPrizeText.text = "Prize: " + newTowerUpgradeManager.currentSellPrize.ToString();

                if (newTowerUpgradeManager.currentLevel >= towerPrefabs.Count)
                {
                    newTowerUpgradeManager.levelText.text = "Max: " + newTowerUpgradeManager.currentLevel.ToString();
                }
            }
        }
    }

    public void Sell()
    {
        CoinManager.instance.AddCoins(currentSellPrize);
        sellPrizeText.text = "Prize: " + currentSellPrize.ToString();
        Destroy(gameObject);
    }

    public void Touch()
    {
        upgradeCanvas.SetActive(true);

        if (touchButton != null)
        {
            touchButton.onClick.RemoveAllListeners();
            touchButton.onClick.AddListener(Close);
        }
    }

    public void Close()
    {
        upgradeCanvas.SetActive(false);

        if (touchButton != null)
        {
            touchButton.onClick.RemoveAllListeners();
            touchButton.onClick.AddListener(Touch);
        }
    }
}
