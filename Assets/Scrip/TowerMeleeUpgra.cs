using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerMeleeUpgra : MonoBehaviour
{
    public GameObject upgradeCanvas;
    public Button touchButton;
    public Button upgradeButton;
    public Button sellButton;
    public Button selectBasepointButton; // Nút mới để chọn basepoint
    public TextMeshProUGUI levelText;
    public TextMeshProUGUI upgradePrizeText;
    public TextMeshProUGUI sellPrizeText;
    public List<GameObject> towerPrefabs; // Danh sách các prefab cho các cấp độ trụ
    public GameObject meleeUnitPrefab; // Prefab của melee unit
    private TowerController towerController;
    private int currentLevel;
    private int currentUpgradePrize;
    private int currentSellPrize;
    private BasepointSelector basepointSelector;
    public List<GameObject> spawnedUnits; // Danh sách các đơn vị được tạo ra
    private Vector3 lastBasepoint; // Lưu basepoint gần nhất

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
        basepointSelector = GameObject.FindObjectOfType<BasepointSelector>(); // Lấy đối tượng BasepointSelector
        spawnedUnits = new List<GameObject>(); // Khởi tạo danh sách đơn vị được tạo ra
        lastBasepoint = transform.position; // Khởi tạo basepoint gần nhất

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
        if (selectBasepointButton != null && basepointSelector != null)
        {
            selectBasepointButton.onClick.RemoveAllListeners();
            selectBasepointButton.onClick.AddListener(() => basepointSelector.ActivateBasepointSelection(spawnedUnits.Count > 0 ? spawnedUnits[0].GetComponent<Melee>() : null));
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
                foreach (GameObject unit in spawnedUnits)
                {
                    Destroy(unit);
                }
                Destroy(gameObject);
                // Hủy các đơn vị đã tạo ra trước khi bán trụ
                

                GameObject newTower = Instantiate(towerPrefabs[currentLevel], position, rotation);
                TowerMeleeUpgra newTowerUpgradeManager = newTower.GetComponent<TowerMeleeUpgra>();

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
        // Hủy các đơn vị đã tạo ra trước khi bán trụ
        foreach (GameObject unit in spawnedUnits)
        {
            Destroy(unit);
        }

        CoinManager.instance.AddCoins(currentSellPrize);
        sellPrizeText.text = "Prize: " + currentSellPrize.ToString();
        Destroy(gameObject);
    }

    public void Touch()
    {
        upgradeCanvas.SetActive(true);

        if (touchButton != null)
        {
            touchButton.onClick.AddListener(Close);
        }
    }

    public void Close()
    {
        upgradeCanvas.SetActive(false);

        if (touchButton != null)
        {
            touchButton.onClick.AddListener(Touch);
        }
    }

    public void AddSpawnedUnit(GameObject unit)
    {
        spawnedUnits.Add(unit);
        Debug.Log("Melee unit added to spawnedUnits: " + unit.name);
    }

    public void RemoveSpawnedUnit(GameObject unit)
    {
        spawnedUnits.Remove(unit);
        Debug.Log("Melee unit removed from spawnedUnits: " + unit.name);
    }

    public void SetLastBasepoint(Vector3 newBasepoint)
    {
        lastBasepoint = newBasepoint;
    }

    public void OnMeleeUnitDestroyed()
    {
        // Gọi hàm tạo mới sau 5 giây
        StartCoroutine(RespawnMeleeUnitAfterDelay(5f));
    }

    private IEnumerator RespawnMeleeUnitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Tạo melee unit mới tại vị trí trụ
        if (meleeUnitPrefab != null)
        {
            GameObject newMeleeUnit = Instantiate(meleeUnitPrefab, transform.position, Quaternion.identity); // Tạo tại vị trí trụ
            AddSpawnedUnit(newMeleeUnit);
            if (basepointSelector != null)
            {
                basepointSelector.ActivateBasepointSelection(newMeleeUnit.GetComponent<Melee>());
            }
            newMeleeUnit.GetComponent<Melee>().basePoint = lastBasepoint; // Đặt basepoint là vị trí gần nhất
        }
    }
}
