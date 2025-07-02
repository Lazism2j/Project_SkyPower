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
                playerHolder.inventoryManagerSO.AddItem(itemData, 1); // 아이템 추가 (아이템 데이터와 개수 1개를 추가)

                // UI 갱신
                if (playerHolder.inventoryUIController != null) // 인벤토리 UI 컨트롤러가 할당되어 있다면
                    playerHolder.inventoryUIController.RefreshInventoryUI(); // 인벤토리 UI를 갱신

                // 디버그 출력 (즉시 인벤토리 확인)
                playerHolder.PrintInventory();

                Destroy(gameObject);
            }
        }
    }
    }