using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class EquipmentInventoryUI : MonoBehaviour
    {
        public EquipmentTableSO equipmentTableSO; // ������ ���
        public InventoryManagerSO inventoryManagerSO; // ������ ���
        public Transform slotParent; // VerticalLayoutGroup ��
        public GameObject slotPrefab; // ���� ������ (Image+Text+Button ��)

        List<GameObject> slotUIList = new List<GameObject>();

        public string filterType = "weapon"; // inspector���� "weapon"/"armor"/"accessory" �� ����

        void Start()
        {
            // �÷��� �� ������ �κ��丮 ����, TableSO�� ��� ��� �߰�!
            inventoryManagerSO.ClearInventory();
            foreach (var equip in equipmentTableSO.equipmentList)
            {
                inventoryManagerSO.AddItem(equip, 1);
            }
            RefreshEquipmentInventoryUI();
            Debug.Log("��� �κ��丮 UI �ʱ�ȭ �Ϸ�");
        }

        public void RefreshEquipmentInventoryUI()
        {
            // ���� ���� ����
            foreach (var go in slotUIList)
                Destroy(go);
            slotUIList.Clear();

            // ���� �κ��丮�� ��� �����۸� ����
            var equipmentList = inventoryManagerSO.GetItemsByType(filterType);

            foreach (var item in equipmentList)
            {
                // EquipmentData�� ����
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

        // ��� ���� �Լ� (�����δ� ĳ����/��Ƽ ��� ���� �ʿ�)
        void OnClickEquip(EquipmentData data)
        {
            Debug.Log($"��� ����: {data.GetDisplayName()}");
            // TODO: ĳ���� ��Ʈ�ѷ��� ���� ���� ����, ���̺� �ݿ� ��
        }
    }
}
