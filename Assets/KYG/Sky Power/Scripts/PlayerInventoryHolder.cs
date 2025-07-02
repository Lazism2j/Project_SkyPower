using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{


public class PlayerInventoryHolder : MonoBehaviour // 플레이어가 InventoryManagerSO를 직접 할당받는 컴포넌트
    {
        public InventoryManagerSO inventoryManagerSO; // 플레이어가 소유한 인벤토리 매니저 SO (InventoryManagerSO 타입)

        public InventoryUIController inventoryUIController; // 인벤토리 UI 컨트롤러 (UI 업데이트용)

        // 인벤토리 디버그(테스트용)
        public void PrintInventory() // 인벤토리 내용을 콘솔에 출력하는 메소드 (디버그용)
        {
        foreach (var slot in inventoryManagerSO.inventory) // 인벤토리의 모든 슬롯을 순회
            {
            Debug.Log($"[인벤토리] {slot.itemData.itemName} x {slot.count}"); // 각 슬롯의 아이템 이름과 개수를 출력
            }
    }
    }
}