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
        public List<InventorySlot> inventory = new List<InventorySlot>();

        public void AddItem(IInventoryItemAdapter itemData, int count = 1)
        {
            var slot = inventory.Find(x => x.itemData == itemData);
            if (slot != null)
                slot.count += count;
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count });
        }

        public List<IInventoryItemAdapter> GetItemsByType(string type)
        {
            return inventory
                .Select(x => x.itemData)
                .Where(x => x is EquipmentDataSO ed && ed.Equip_Type == type)
                .ToList();
        }

        public List<IInventoryItemAdapter> GetItemsBySlotType(EquipmentType slotType)
        {
            return inventory
                .Select(x => x.itemData)
                .Where(x => x is EquipmentDataSO ed && ed.GetSlotType() == slotType)
                .ToList();
        }

        public int GetCount(IInventoryItemAdapter itemData)
        {
            var slot = inventory.Find(x => x.itemData == itemData);
            return slot != null ? slot.count : 0;
        }

        public bool UseItem(IInventoryItemAdapter itemData, int count = 1)
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

        public void ClearInventory() => inventory.Clear();
    }

    [System.Serializable]
    public class InventorySlot
    {
        public IInventoryItemAdapter itemData;
        public int count;
    }
}