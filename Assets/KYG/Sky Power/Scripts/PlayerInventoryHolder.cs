using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

public class PlayerInventoryHolder : MonoBehaviour // 플레이어가 InventoryManagerSO를 직접 할당받는 컴포넌트
{
    public InventoryManagerSO inventoryManagerSO;

    // 인벤토리 디버그(테스트용)
    public void PrintInventory()
    {
        foreach (var slot in inventoryManagerSO.inventory)
        {
            Debug.Log($"[인벤토리] {slot.itemData.itemName} x {slot.count}");
        }
    }
    }
