using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/InventoryManagerSO")]
    public class InventoryManagerSO : ScriptableObject // ������ �κ��丮 �Ŵ��� SO (�������� �����ϴ� ��ũ��Ʈ ������Ʈ)
    {
        [Header("���� ������ ����Ʈ")]
        public List<InventorySlot> inventory = new List<InventorySlot>(); // ���� �������� �����ϴ� ����Ʈ (InventorySlot Ÿ���� ����Ʈ)

        // ������ �߰� (SO ����)
        public void AddItem(ItemData itemData, int count = 1) // �������� �߰��ϴ� �޼��� (������ ������ SO ����)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ���� �κ��丮���� �ش� �������� ã��
            if (slot != null) // �������� �̹� �����ϴ��� Ȯ��
                slot.count += count; // �̹� �����ϴ� �������̸� ������ ������Ŵ
            else
                inventory.Add(new InventorySlot { itemData = itemData, count = count }); // �������� ������ ���ο� ������ �߰�
        }

        // ������ �߰� (ID ����, ������ �Ŵ��� �ʿ�)
        public void AddItemById(ItemManagerSO itemManager, int id, int count = 1) // ������ ID�� �������� �߰��ϴ� �޼��� (������ �Ŵ��� SO �ʿ�)
        {
            var item = itemManager.GetItemById(id); // ������ �Ŵ������� ID�� �������� �˻�
            if (item != null) // �������� �����ϴ��� Ȯ��
                AddItem(item, count);   // �������� �����ϸ� AddItem �޼��带 ȣ���Ͽ� �߰�
            else
                Debug.LogError($"[InventoryManagerSO] ID {id}�� �������� ã�� �� �����ϴ�!");  
        }

        // ������ ���� ��ȸ
        public int GetCount(ItemData itemData) // �������� ������ ��ȸ�ϴ� �޼��� (������ ������ SO ����)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ���� �κ��丮���� �ش� �������� ã��
            return slot != null ? slot.count : 0; // �������� �����ϸ� ������ ��ȯ, ������ 0 ��ȯ
        }

        // ������ �Ҹ�
        public bool UseItem(ItemData itemData, int count = 1) // �������� �Ҹ��ϴ� �޼��� (������ ������ SO ����)
        {
            var slot = inventory.Find(x => x.itemData == itemData); // ���� �κ��丮���� �ش� �������� ã��
            if (slot != null && slot.count >= count) // �������� �����ϰ� �Ҹ��� ������ ������� Ȯ��
            {
                slot.count -= count; // �Ҹ��� ������ŭ ����
                if (slot.count <= 0) // ������ 0 ���ϰ� �Ǹ�
                    inventory.Remove(slot); // �ش� ������ �κ��丮���� ����
                return true;
            }
            return false;
        }

        // ��ü Ŭ����
        public void ClearInventory() // �κ��丮�� ��ü Ŭ�����ϴ� �޼���
        {
            inventory.Clear(); // �κ��丮 ����Ʈ�� �ʱ�ȭ�Ͽ� ��� �������� ����
        }
    }

    [System.Serializable] // �κ��丮 ������ ��Ÿ���� Ŭ���� (������ �����Ϳ� ������ ����)
    public class InventorySlot : System.Object // �κ��丮 ���� Ŭ���� (������ �����Ϳ� ������ �����ϴ� Ŭ����)
    {
        public ItemData itemData; // ������ ������ SO (�������� ������ �Ӽ��� ��� �ִ� ��ũ��Ʈ ������Ʈ)
        public int count; // ������ ���� (�ش� �������� ���� ������ ��Ÿ���� ������ ����)
    }
}