using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class InventoryUIController : MonoBehaviour 
    {
        public InventoryManagerSO inventoryManagerSO; // 할당
        public GameObject slotUIPrefab;              // 슬롯 UI 프리팹 할당
        public Transform slotParent;                 // 슬롯을 담을 부모 오브젝트(Vertical Layout Group 등)

        private List<GameObject> slotUIList = new List<GameObject>(); // 현재 UI 슬롯 목록

        void Start()
        {
            RefreshInventoryUI();

            // 첫 슬롯에서 자식들 이름 확인
            if (slotUIList.Count > 0)
            {
                var go = slotUIList[0];
                foreach (var tr in go.GetComponentsInChildren<Transform>(true))
                    Debug.Log("[프리팹 자식 오브젝트] " + tr.name);
            }
        }

        public void RefreshInventoryUI() // 인벤토리 UI 갱신 메서드
        {
            // 1. 기존 슬롯 UI 제거
            foreach (var go in slotUIList) 
                Destroy(go); // 기존 UI 슬롯 제거
            slotUIList.Clear();
            Debug.Log($"[UI] 인벤토리 슬롯 개수: {inventoryManagerSO.inventory.Count}"); 

            // 2. 인벤토리 슬롯 개수만큼 프리팹 생성 및 정보 바인딩
            foreach (var slot in inventoryManagerSO.inventory)
            {
                var go = Instantiate(slotUIPrefab, slotParent); // 슬롯 UI 프리팹 인스턴스화
                slotUIList.Add(go);
                Debug.Log($"[UI] 슬롯 생성: {slot.itemData.itemName} x {slot.count}");

                // 바인딩(예시, 실제 컴포넌트명/구조에 맞게 수정)
                go.transform.Find("NameText").GetComponent<Text>().text = slot.itemData.itemName;
                go.transform.Find("CountText").GetComponent<Text>().text = $"x{slot.count}"; 
                var iconImage = go.transform.Find("Icon").GetComponent<Image>(); 
                if (slot.itemData.itemPrefab != null) 
                {     
                     // 만약 itemData에 스프라이트 정보가 있다면 할당, 아니면 프리팹 프리뷰 등
                    // iconImage.sprite = ...; 
                }
            }
        }
    }
}
