using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class InventoryUIController : MonoBehaviour 
    {
        public InventoryManagerSO inventoryManagerSO; // �Ҵ�
        public GameObject slotUIPrefab;              // ���� UI ������ �Ҵ�
        public Transform slotParent;                 // ������ ���� �θ� ������Ʈ(Vertical Layout Group ��)

        private List<GameObject> slotUIList = new List<GameObject>(); // ���� UI ���� ���

        void Start()
        {
            RefreshInventoryUI();

            // ù ���Կ��� �ڽĵ� �̸� Ȯ��
            if (slotUIList.Count > 0)
            {
                var go = slotUIList[0];
                foreach (var tr in go.GetComponentsInChildren<Transform>(true))
                    Debug.Log("[������ �ڽ� ������Ʈ] " + tr.name);
            }
        }

        public void RefreshInventoryUI() // �κ��丮 UI ���� �޼���
        {
            // 1. ���� ���� UI ����
            foreach (var go in slotUIList) 
                Destroy(go); // ���� UI ���� ����
            slotUIList.Clear();
            Debug.Log($"[UI] �κ��丮 ���� ����: {inventoryManagerSO.inventory.Count}"); 

            // 2. �κ��丮 ���� ������ŭ ������ ���� �� ���� ���ε�
            foreach (var slot in inventoryManagerSO.inventory)
            {
                var go = Instantiate(slotUIPrefab, slotParent); // ���� UI ������ �ν��Ͻ�ȭ
                slotUIList.Add(go);
                Debug.Log($"[UI] ���� ����: {slot.itemData.itemName} x {slot.count}");

                // ���ε�(����, ���� ������Ʈ��/������ �°� ����)
                go.transform.Find("NameText").GetComponent<Text>().text = slot.itemData.itemName;
                go.transform.Find("CountText").GetComponent<Text>().text = $"x{slot.count}"; 
                var iconImage = go.transform.Find("Icon").GetComponent<Image>(); 
                if (slot.itemData.itemPrefab != null) 
                {     
                     // ���� itemData�� ��������Ʈ ������ �ִٸ� �Ҵ�, �ƴϸ� ������ ������ ��
                    // iconImage.sprite = ...; 
                }
            }
        }
    }
}
