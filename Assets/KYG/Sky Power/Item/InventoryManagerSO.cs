using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/InventoryManagerSO")]
    public class InventoryManagerSO : ScriptableObject // 아이템 인벤토리 매니저 SO (아이템을 관리하는 스크립트 오브젝트)
    {
        [Header("보유 아이템 리스트")]
        public List<InventorySlot> inventory = new List<InventorySlot>(); // 보유 아이템을 저장하는 리스트 (InventorySlot 타입의 리스트)

        // 아이템 추가 (SO 기준)
        public void AddItem(ItemData itemData, int count = 1) // 아이템을 추가하는 메서드 (아이템 데이터 SO 기준)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 현재 인벤토리에서 해당 아이템을 찾음
            if (slot != null) // 아이템이 이미 존재하는지 확인
                slot.count += count; // 이미 존재하는 아이템이면 수량을 증가시킴
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count }); // 아이템이 없으면 새로운 슬롯을 추가
        }

        // 아이템 추가 (ID 기준, 아이템 매니저 필요)
        public void AddItemById(ItemManagerSO itemManager, int id, int count = 1) // 아이템 ID로 아이템을 추가하는 메서드 (아이템 매니저 SO 필요)
        {
            var item = itemManager.GetItemById(id); // 아이템 매니저에서 ID로 아이템을 검색
            if (item != null) // 아이템이 존재하는지 확인
                AddItem(item, count);   // 아이템이 존재하면 AddItem 메서드를 호출하여 추가
            else
                Debug.LogError($"[InventoryManagerSO] ID {id}번 아이템을 찾을 수 없습니다!");  
        }

        // 아이템 수량 조회
        public int GetCount(ItemData itemData) // 아이템의 수량을 조회하는 메서드 (아이템 데이터 SO 기준)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 현재 인벤토리에서 해당 아이템을 찾음
            return slot != null ? slot.count : 0; // 아이템이 존재하면 수량을 반환, 없으면 0 반환
        }

        // 아이템 소모
        public bool UseItem(ItemData itemData, int count = 1) // 아이템을 소모하는 메서드 (아이템 데이터 SO 기준)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // 현재 인벤토리에서 해당 아이템을 찾음
            if (slot != null && slot.count >= count) // 아이템이 존재하고 소모할 수량이 충분한지 확인
            {
                slot.count -= count; // 소모할 수량만큼 감소
                if (slot.count <= 0) // 수량이 0 이하가 되면
                    inventory.Remove(slot); // 해당 슬롯을 인벤토리에서 제거
                return true;
            }
            return false;
        }

        // 전체 클리어
        public void ClearInventory() // 인벤토리를 전체 클리어하는 메서드
        {
            inventory.Clear(); // 인벤토리 리스트를 초기화하여 모든 아이템을 제거
        }
    }

    [System.Serializable] // 인벤토리 슬롯을 나타내는 클래스 (아이템 데이터와 수량을 저장)
    public class InventorySlot : System.Object // 인벤토리 슬롯 클래스 (아이템 데이터와 수량을 저장하는 클래스)
    {
        public ItemData itemData; // 아이템 데이터 SO (아이템의 정보와 속성을 담고 있는 스크립트 오브젝트)
        public int count; // 아이템 수량 (해당 아이템의 보유 수량을 나타내는 정수형 변수)
    }
}