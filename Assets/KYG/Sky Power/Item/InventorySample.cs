using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{


    public class InventorySample : MonoBehaviour
    {
        public ItemManagerSO itemManagerSO;         // �����Ϳ��� �Ҵ�
        public InventoryManagerSO inventoryManagerSO; // �����Ϳ��� �Ҵ�

        void Start()
        {
            // 1. ������ �߰� (ID��)
            inventoryManagerSO.AddItemById(itemManagerSO, 1001, 5);

            // 2. ������ �߰� (�̸�����)
            var item = itemManagerSO.GetItemByName("ü������");
            if (item != null)
                inventoryManagerSO.AddItem(item, 2);

            // 3. ������ ���
            bool used = inventoryManagerSO.UseItem(item, 1);
            Debug.Log($"ü������ ��� ����? {used}");

            // 4. �κ��丮 ��ü ���
            foreach (var slot in inventoryManagerSO.inventory)
            {
                Debug.Log($"����: {slot.itemData.itemName} x {slot.count}");
            }
        }
    }
}
