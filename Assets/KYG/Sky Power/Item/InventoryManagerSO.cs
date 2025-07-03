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
        public List<InventorySlot> inventory = new List<InventorySlot>(); // �κ��丮 ���� ����Ʈ

        // ���/������ ��� ���� (SO ����)
        public void AddItem(IInventoryItemAdapter itemData, int count = 1) 
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ������ �����ͷ� ���� ã��
            if (slot != null) 
                slot.count += count; // �̹� �����ϴ� �������̸� ���� ����
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count }); // ���ο� �������̸� �� ���� �߰�
        }

        public int GetCount(IInventoryItemAdapter itemData) // Ư�� �������� ���� ��ȯ
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ������ �����ͷ� ���� ã��
            return slot != null ? slot.count : 0; // ������ ������ ���� ��ȯ, ������ 0 ��ȯ
        }

        public bool UseItem(IInventoryItemAdapter itemData, int count = 1) // ������ ��� (���� ����)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ������ �����ͷ� ���� ã��
            if (slot != null && slot.count >= count) // ������ �ְ�, ����Ϸ��� ������ ����ϸ�
            {
                slot.count -= count; // ���� ����
                if (slot.count <= 0) // ������ 0 ���ϰ� �Ǹ�
                    inventory.Remove(slot); // ���� ����
                return true; // ��� ����
            }
            return false; // ��� ���� (������ ���ų� ������ ������ ���)
        }

        public void ClearInventory() => inventory.Clear(); // �κ��丮 �ʱ�ȭ

        // Ÿ�� ���� ���� (���⸸, ���� ��)
        public List<IInventoryItemAdapter> GetItemsByType(string type) // Ư�� Ÿ���� ������ ����Ʈ ��ȯ
        {
            return inventory 
                .Select(x => x.itemData) // �κ��丮 ���Կ��� ������ �����͸� ����
                .Where(x => x is EquipmentData ed && ed.Equip_Type == type) // Ÿ���� ��ġ�ϴ� �����۸� ���͸�
                .ToList();
        }
    }

    [System.Serializable]
    public class InventorySlot // �κ��丮 ���� Ŭ����
    {
        public IInventoryItemAdapter itemData; // ������ ������ (IInventoryItemAdapter �������̽� ����)
        public int count; // ������ ����
    }
}