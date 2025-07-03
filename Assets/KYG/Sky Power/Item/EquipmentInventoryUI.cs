using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class EquipmentInventoryUI : MonoBehaviour
    {
        public EquipmentTableSO equipmentTableSO; // 에디터 등록
        public InventoryManagerSO inventoryManagerSO; // 에디터 등록
        public Transform slotParent; // VerticalLayoutGroup 등
        public GameObject slotPrefab; // 슬롯 프리팹 (Image+Text+Button 등)

        List<GameObject> slotUIList = new List<GameObject>();

        public string filterType = "weapon"; // inspector에서 "weapon"/"armor"/"accessory" 등 지정

        void Start()
        {
            // 플레이 할 때마다 인벤토리 비우고, TableSO의 모든 장비를 추가!
            inventoryManagerSO.ClearInventory();
            foreach (var equip in equipmentTableSO.equipmentList)
            {
                inventoryManagerSO.AddItem(equip, 1);
            }
            RefreshEquipmentInventoryUI();
            Debug.Log("장비 인벤토리 UI 초기화 완료");
        }

        public void RefreshEquipmentInventoryUI()
        {
            // 기존 슬롯 제거
            foreach (var go in slotUIList)
                Destroy(go);
            slotUIList.Clear();

            // 현재 인벤토리의 장비 아이템만 필터
            var equipmentList = inventoryManagerSO.GetItemsByType(filterType); // filterType에 따라 "weapon"/"armor"/"accessory" 등 필터링

            foreach (var item in equipmentList) 
            {
                // EquipmentData만 필터
                if (item is EquipmentData equipData)
                {
                    var go = Instantiate(slotPrefab, slotParent); // 슬롯 프리팹을 부모 아래에 인스턴스화
                    slotUIList.Add(go);

                    var icon = go.transform.Find("Icon").GetComponent<Image>(); // 슬롯 내 아이콘 이미지 컴포넌트
                    var nameText = go.transform.Find("NameText").GetComponent<TMP_Text>(); // 슬롯 내 이름 텍스트 컴포넌트
                    var levelText = go.transform.Find("LevelText").GetComponent<TMP_Text>(); // 슬롯 내 레벨 텍스트 컴포넌트
                    var equipBtn = go.transform.Find("EquipButton").GetComponent<Button>(); // 슬롯 내 장비 버튼 컴포넌트

                    icon.sprite = equipData.GetIcon(); // 아이콘 설정 (아이콘은 Resources 폴더에서 로드)
                    nameText.text = equipData.GetDisplayName(); // 아이템 이름 설정
                    levelText.text = $"Lv.{equipData.Equip_Level}"; // 레벨 텍스트 설정
                    equipBtn.onClick.RemoveAllListeners(); // 기존 리스너 제거
                    equipBtn.onClick.AddListener(() => OnClickEquip(equipData)); // 장비 버튼 클릭 시 장비 장착 함수 호출
                }
            }
        }

        // 장비 장착 함수 (실제로는 캐릭터/파티 등과 연동 필요)
        void OnClickEquip(EquipmentData data)
        {
            Debug.Log($"장비 장착: {data.GetDisplayName()}");
            // TODO: 캐릭터 컨트롤러에 장착 정보 변경, 세이브 반영 등
        }
    }
}
