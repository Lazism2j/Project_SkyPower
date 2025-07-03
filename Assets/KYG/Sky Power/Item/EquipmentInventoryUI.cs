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
            var equipmentList = inventoryManagerSO.GetItemsByType(filterType); // filterType�� ���� "weapon"/"armor"/"accessory" �� ���͸�

            foreach (var item in equipmentList) 
            {
                // EquipmentData�� ����
                if (item is EquipmentData equipData)
                {
                    var go = Instantiate(slotPrefab, slotParent); // ���� �������� �θ� �Ʒ��� �ν��Ͻ�ȭ
                    slotUIList.Add(go);

                    var icon = go.transform.Find("Icon").GetComponent<Image>(); // ���� �� ������ �̹��� ������Ʈ
                    var nameText = go.transform.Find("NameText").GetComponent<TMP_Text>(); // ���� �� �̸� �ؽ�Ʈ ������Ʈ
                    var levelText = go.transform.Find("LevelText").GetComponent<TMP_Text>(); // ���� �� ���� �ؽ�Ʈ ������Ʈ
                    var equipBtn = go.transform.Find("EquipButton").GetComponent<Button>(); // ���� �� ��� ��ư ������Ʈ

                    icon.sprite = equipData.GetIcon(); // ������ ���� (�������� Resources �������� �ε�)
                    nameText.text = equipData.GetDisplayName(); // ������ �̸� ����
                    levelText.text = $"Lv.{equipData.Equip_Level}"; // ���� �ؽ�Ʈ ����
                    equipBtn.onClick.RemoveAllListeners(); // ���� ������ ����
                    equipBtn.onClick.AddListener(() => OnClickEquip(equipData)); // ��� ��ư Ŭ�� �� ��� ���� �Լ� ȣ��
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
