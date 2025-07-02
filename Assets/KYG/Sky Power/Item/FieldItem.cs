using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    public class FieldItem : MonoBehaviour // �ʵ忡 ���� ������ ������Ʈ (��: ���� �� �ִ� ������, NPC ��)
    {
        public ItemData itemData;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerInventoryHolder>(out var playerHolder)) 
            {
                playerHolder.inventoryManagerSO.AddItem(itemData, 1); // ������ �߰� (������ �����Ϳ� ���� 1���� �߰�)

                // UI ����
                if (playerHolder.inventoryUIController != null) // �κ��丮 UI ��Ʈ�ѷ��� �Ҵ�Ǿ� �ִٸ�
                    playerHolder.inventoryUIController.RefreshInventoryUI(); // �κ��丮 UI�� ����

                // ����� ��� (��� �κ��丮 Ȯ��)
                playerHolder.PrintInventory();

                Destroy(gameObject);
            }
        }
    }
    }