using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

public class PlayerInventoryHolder : MonoBehaviour // �÷��̾ InventoryManagerSO�� ���� �Ҵ�޴� ������Ʈ
{
    public InventoryManagerSO inventoryManagerSO;

    // �κ��丮 �����(�׽�Ʈ��)
    public void PrintInventory()
    {
        foreach (var slot in inventoryManagerSO.inventory)
        {
            Debug.Log($"[�κ��丮] {slot.itemData.itemName} x {slot.count}");
        }
    }
    }
