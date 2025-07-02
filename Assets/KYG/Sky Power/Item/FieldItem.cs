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
                playerHolder.inventoryManagerSO.AddItem(itemData, 1);

                // ����� ��� (��� �κ��丮 Ȯ��)
                playerHolder.PrintInventory();

                Destroy(gameObject);
            }
        }
    }
    }