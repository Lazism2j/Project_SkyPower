using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{


public class PlayerInventoryHolder : MonoBehaviour // �÷��̾ InventoryManagerSO�� ���� �Ҵ�޴� ������Ʈ
    {
        public InventoryManagerSO inventoryManagerSO; // �÷��̾ ������ �κ��丮 �Ŵ��� SO (InventoryManagerSO Ÿ��)

        public InventoryUIController inventoryUIController; // �κ��丮 UI ��Ʈ�ѷ� (UI ������Ʈ��)

        // �κ��丮 �����(�׽�Ʈ��)
        public void PrintInventory() // �κ��丮 ������ �ֿܼ� ����ϴ� �޼ҵ� (����׿�)
        {
        foreach (var slot in inventoryManagerSO.inventory) // �κ��丮�� ��� ������ ��ȸ
            {
            Debug.Log($"[�κ��丮] {slot.itemData.itemName} x {slot.count}"); // �� ������ ������ �̸��� ������ ���
            }
    }
    }
}