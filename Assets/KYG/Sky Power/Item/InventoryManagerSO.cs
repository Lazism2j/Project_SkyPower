using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/InventoryManagerSO")] 
    public class InventoryManagerSO : ScriptableObject 
    {
        [Header("보유 아이템 리스트")]
        public List<InventorySlot> inventory = new List<InventorySlot>(); // 아이템 데이터와 개수를 저장하는 리스트

        // 추가/검색/필터
        public void AddItem(IInventoryItemAdapter itemData, int count = 1) 
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 아이템이 이미 존재하는지 확인
            if (slot != null)
                slot.count += count; // 존재하면 개수만 증가
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count }); // 존재하지 않으면 새 슬롯 추가
        }

        public List<IInventoryItemAdapter> GetItemsByType(string type) // 아이템 타입으로 필터링하여 아이템 리스트 반환
        {
            return inventory 
                .Select(x => x.itemData) // InventorySlot에서 itemData만 추출
                .Where(x => x is EquipmentData ed && ed.Equip_Type == type) // EquipmentData 타입이면서 Equip_Type이 일치하는 아이템만 필터링
                .ToList();
        }

        public List<IInventoryItemAdapter> GetItemsBySlotType(EquipmentSlotType slotType) // 슬롯 타입으로 필터링하여 아이템 리스트 반환
        {
            return inventory
                .Select(x => x.itemData) // InventorySlot에서 itemData만 추출
                .Where(x => x is EquipmentData ed && ed.GetSlotType() == slotType) // EquipmentData 타입이면서 슬롯 타입이 일치하는 아이템만 필터링
                .ToList();
        }

        public int GetCount(IInventoryItemAdapter itemData) // 특정 아이템의 개수를 반환하는 메서드
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 아이템 데이터로 슬롯 검색
            return slot != null ? slot.count : 0; // 슬롯이 존재하면 개수 반환, 없으면 0 반환
        }

        public bool UseItem(IInventoryItemAdapter itemData, int count = 1)  // 아이템 사용 메서드
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 아이템 데이터로 슬롯 검색
            if (slot != null && slot.count >= count) // 슬롯이 존재하고 개수가 충분하면
            {
                slot.count -= count; // 개수 감소
                if (slot.count <= 0) 
                    inventory.Remove(slot);// 개수가 0 이하가 되면 슬롯 제거
                return true;
            }
            return false;
        }

        public void ClearInventory() => inventory.Clear(); // 인벤토리 초기화 메서드
    }

    [System.Serializable]
    public class InventorySlot
    {
        public IInventoryItemAdapter itemData; // 아이템 데이터 (IInventoryItemAdapter 인터페이스를 구현한 클래스)
        public int count;
    }
}