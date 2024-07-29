using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeTower : MonoBehaviour
{
    public float selectionRadius = 5f;
    private bool isSelected = false;
    private Transform meleeUnit;

    void Update()
    {
        if (isSelected)
        {
            // Hiển thị vùng tròn lựa chọn
            ShowSelectionRadius();

            // Kiểm tra click chuột trái để chọn vị trí mới
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0;

                // Kiểm tra nếu vị trí được chọn nằm trong vùng tròn
                if (Vector3.Distance(mousePosition, transform.position) <= selectionRadius)
                {
                    SetNewBasePoint(mousePosition);
                    isSelected = false; // Deselect sau khi chọn
                }
            }
        }
    }

    void OnMouseDown()
    {
        isSelected = !isSelected; // Chọn hoặc bỏ chọn trụ
    }

    void ShowSelectionRadius()
    {
        // Hiển thị vùng tròn lựa chọn trong chế độ Editor
        Debug.DrawLine(transform.position, transform.position + Vector3.up * selectionRadius, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.down * selectionRadius, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.left * selectionRadius, Color.green);
        Debug.DrawLine(transform.position, transform.position + Vector3.right * selectionRadius, Color.green);
    }

    void SetNewBasePoint(Vector3 newBasePoint)
    {
        if (meleeUnit != null)
        {
            meleeUnit.GetComponent<Melee>().basePoint = newBasePoint;
            Debug.Log("Base point updated to: " + newBasePoint);
        }
    }

    public void SetMeleeUnit(Transform melee)
    {
        meleeUnit = melee;
    }
}
