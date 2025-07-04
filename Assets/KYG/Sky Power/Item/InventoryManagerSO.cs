using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/InventoryManagerSO")]
    public class InventoryManagerSO : ScriptableObject
    {
        [Header("���� ������ ����Ʈ")]
        public List<InventorySlot> inventory = new List<InventorySlot>();

        // ������ �߰� (SO ����)
        public void AddItem(ItemData itemData, int count = 1)
        {
            var slot = inventory.Find(x => x.itemData == itemData);
            if (slot != null)
                slot.count += count;
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count });
        }

        // ������ �߰� (ID ����, ������ �Ŵ��� �ʿ�)
        public void AddItemById(ItemManagerSO itemManager, int id, int count = 1)
        {
            var item = itemManager.GetItemById(id);
            if (item != null)
                AddItem(item, count);
            else
                Debug.LogError($"[InventoryManagerSO] ID {id}�� �������� ã�� �� �����ϴ�!");
        }

        // ������ ���� ��ȸ
        public int GetCount(ItemData itemData)
        {
            var slot = inventory.Find(x => x.itemData == itemData);
            return slot != null ? slot.count : 0;
        }

        // ������ �Ҹ�
        public bool UseItem(ItemData itemData, int count = 1)
        {
            var slot = inventory.Find(x => x.itemData == itemData);
            if (slot != null && slot.count >= count)
            {
                slot.count -= count;
                if (slot.count <= 0)
                    inventory.Remove(slot);
                return true;
            }
            return false;
        }

        // ��ü Ŭ����
        public void ClearInventory()
        {
            inventory.Clear();
        }
    }

    [System.Serializable]
    public class InventorySlot
    {
        public ItemData itemData;
        public int count;
    }
}