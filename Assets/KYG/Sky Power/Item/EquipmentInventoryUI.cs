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
            var equipmentList = inventoryManagerSO.GetItemsByType(filterType);

            foreach (var item in equipmentList)
            {
                // EquipmentData만 필터
                if (item is EquipmentData equipData)
                {
                    var go = Instantiate(slotPrefab, slotParent);
                    slotUIList.Add(go);

                    var icon = go.transform.Find("Icon").GetComponent<Image>();
                    var nameText = go.transform.Find("NameText").GetComponent<TMP_Text>();
                    var levelText = go.transform.Find("LevelText").GetComponent<TMP_Text>();
                    var equipBtn = go.transform.Find("EquipButton").GetComponent<Button>();

                    icon.sprite = equipData.GetIcon();
                    nameText.text = equipData.GetDisplayName();
                    levelText.text = $"Lv.{equipData.Equip_Level}";
                    equipBtn.onClick.RemoveAllListeners();
                    equipBtn.onClick.AddListener(() => OnClickEquip(equipData));
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
