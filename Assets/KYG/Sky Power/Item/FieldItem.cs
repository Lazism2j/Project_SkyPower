using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    public class FieldItem : MonoBehaviour // 필드에 놓인 아이템 오브젝트 (예: 먹을 수 있는 아이템, NPC 등)
    {
        public ItemData itemData;

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<PlayerInventoryHolder>(out var playerHolder))
            {
                playerHolder.inventoryManagerSO.AddItem(itemData, 1);

                // 디버그 출력 (즉시 인벤토리 확인)
                playerHolder.PrintInventory();

                Destroy(gameObject);
            }
        }
    }
    }