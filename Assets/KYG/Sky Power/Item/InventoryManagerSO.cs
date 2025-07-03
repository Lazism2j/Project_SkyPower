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
        public List<InventorySlot> inventory = new List<InventorySlot>(); // 인벤토리 슬롯 리스트

        // 장비/아이템 모두 지원 (SO 기준)
        public void AddItem(IInventoryItemAdapter itemData, int count = 1) 
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 아이템 데이터로 슬롯 찾기
            if (slot != null) 
                slot.count += count; // 이미 존재하는 아이템이면 개수 증가
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count }); // 새로운 아이템이면 새 슬롯 추가
        }

        public int GetCount(IInventoryItemAdapter itemData) // 특정 아이템의 개수 반환
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 아이템 데이터로 슬롯 찾기
            return slot != null ? slot.count : 0; // 슬롯이 있으면 개수 반환, 없으면 0 반환
        }

        public bool UseItem(IInventoryItemAdapter itemData, int count = 1) // 아이템 사용 (개수 감소)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 아이템 데이터로 슬롯 찾기
            if (slot != null && slot.count >= count) // 슬롯이 있고, 사용하려는 개수가 충분하면
            {
                slot.count -= count; // 개수 감소
                if (slot.count <= 0) // 개수가 0 이하가 되면
                    inventory.Remove(slot); // 슬롯 제거
                return true; // 사용 성공
            }
            return false; // 사용 실패 (슬롯이 없거나 개수가 부족한 경우)
        }

        public void ClearInventory() => inventory.Clear(); // 인벤토리 초기화

        // 타입 필터 예시 (무기만, 방어구만 등)
        public List<IInventoryItemAdapter> GetItemsByType(string type) // 특정 타입의 아이템 리스트 반환
        {
            return inventory 
                .Select(x => x.itemData) // 인벤토리 슬롯에서 아이템 데이터만 추출
                .Where(x => x is EquipmentData ed && ed.Equip_Type == type) // 타입이 일치하는 아이템만 필터링
                .ToList();
        }
    }

    [System.Serializable]
    public class InventorySlot // 인벤토리 슬롯 클래스
    {
        public IInventoryItemAdapter itemData; // 아이템 데이터 (IInventoryItemAdapter 인터페이스 구현)
        public int count; // 아이템 개수
    }
}