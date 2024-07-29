using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasepointSelector : MonoBehaviour
{
    public GameObject selectionCircle; // Vòng tròn hiển thị khi chọn basepoint
    private Melee meleeUnitInstance; // Thể hiện của đơn vị melee
    public float selectionRadius = 5f; // Bán kính của vòng tròn chọn
    private TowerMeleeUpgra towerMeleeUpgra;

    void Start()
    {
        selectionCircle.SetActive(false); // Ẩn vòng tròn lúc đầu
        towerMeleeUpgra = GameObject.FindObjectOfType<TowerMeleeUpgra>();
    }

    void Update()
    {
        if (selectionCircle.activeSelf && meleeUnitInstance != null)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                if (Vector2.Distance(transform.position, mousePosition) <= selectionRadius)
                {
                    // Lưu lại basepoint gần nhất
                    meleeUnitInstance.basePoint = mousePosition;
                    selectionCircle.SetActive(false); // Ẩn vòng tròn sau khi chọn

                    if (towerMeleeUpgra != null)
                    {
                        towerMeleeUpgra.SetLastBasepoint(mousePosition); // Cập nhật basepoint gần nhất trong TowerMeleeUpgra
                    }
                }
            }
        }
    }

    public void ActivateBasepointSelection(Melee meleeUnit)
    {
        meleeUnitInstance = meleeUnit;
        selectionCircle.transform.position = transform.position; // Đặt vòng tròn chọn tại vị trí trụ
        selectionCircle.SetActive(true); // Hiển thị vòng tròn
    }
}
