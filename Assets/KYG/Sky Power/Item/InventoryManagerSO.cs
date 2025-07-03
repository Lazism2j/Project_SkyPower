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
        [Header("���� ������ ����Ʈ")]
        public List<InventorySlot> inventory = new List<InventorySlot>(); // ������ �����Ϳ� ������ �����ϴ� ����Ʈ

        // �߰�/�˻�/����
        public void AddItem(IInventoryItemAdapter itemData, int count = 1) 
        {
            var slot = inventory.Find(x => x.itemData == itemData); // �������� �̹� �����ϴ��� Ȯ��
            if (slot != null)
                slot.count += count; // �����ϸ� ������ ����
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count }); // �������� ������ �� ���� �߰�
        }

        public List<IInventoryItemAdapter> GetItemsByType(string type) // ������ Ÿ������ ���͸��Ͽ� ������ ����Ʈ ��ȯ
        {
            return inventory 
                .Select(x => x.itemData) // InventorySlot���� itemData�� ����
                .Where(x => x is EquipmentData ed && ed.Equip_Type == type) // EquipmentData Ÿ���̸鼭 Equip_Type�� ��ġ�ϴ� �����۸� ���͸�
                .ToList();
        }

        public List<IInventoryItemAdapter> GetItemsBySlotType(EquipmentSlotType slotType) // ���� Ÿ������ ���͸��Ͽ� ������ ����Ʈ ��ȯ
        {
            return inventory
                .Select(x => x.itemData) // InventorySlot���� itemData�� ����
                .Where(x => x is EquipmentData ed && ed.GetSlotType() == slotType) // EquipmentData Ÿ���̸鼭 ���� Ÿ���� ��ġ�ϴ� �����۸� ���͸�
                .ToList();
        }

        public int GetCount(IInventoryItemAdapter itemData) // Ư�� �������� ������ ��ȯ�ϴ� �޼���
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ������ �����ͷ� ���� �˻�
            return slot != null ? slot.count : 0; // ������ �����ϸ� ���� ��ȯ, ������ 0 ��ȯ
        }

        public bool UseItem(IInventoryItemAdapter itemData, int count = 1)  // ������ ��� �޼���
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ������ �����ͷ� ���� �˻�
            if (slot != null && slot.count >= count) // ������ �����ϰ� ������ ����ϸ�
            {
                slot.count -= count; // ���� ����
                if (slot.count <= 0) 
                    inventory.Remove(slot);// ������ 0 ���ϰ� �Ǹ� ���� ����
                return true;
            }
            return false;
        }

        public void ClearInventory() => inventory.Clear(); // �κ��丮 �ʱ�ȭ �޼���
    }

    [System.Serializable]
    public class InventorySlot
    {
        public IInventoryItemAdapter itemData; // ������ ������ (IInventoryItemAdapter �������̽��� ������ Ŭ����)
        public int count;
    }
}