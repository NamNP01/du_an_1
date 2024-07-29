using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUnit : MonoBehaviour
{
    public GameObject unitPrefab;
    public Transform spawnPoint;
    public TowerMeleeUpgra tower; // Tham chiếu đến đối tượng TowerMeleeUpgra

    public void Spawn()
    {
        if (unitPrefab != null && spawnPoint != null && tower != null)
        {
            GameObject newUnit = Instantiate(unitPrefab, spawnPoint.position, Quaternion.identity);

            // Đối tượng TowerMeleeUpgra gọi phương thức AddSpawnedUnit
            tower.AddSpawnedUnit(newUnit);
        }
    }
}
