using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
{

        [Header("이동 속도")]
        public float moveSpeed = 7f;

        [Header("인벤토리 참조 (Holder)")]
        public PlayerInventoryHolder inventoryHolder; // PlayerInventoryHolder 연결 (에디터에서 할당)

        CharacterController characterController;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            // inventoryHolder = GetComponent<PlayerInventoryHolder>(); // 플레이어 자체에 붙어 있으면 자동으로 가져와도 됨
        }

        void Update()
        {
            // --- 1. 이동 입력 받기 ---
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(h, 0, v).normalized;

            // --- 2. 이동 처리 ---
            characterController.Move(dir * moveSpeed * Time.deltaTime);

            // (옵션) 회전 처리 등 추가
        }

        // ---- 아이템 획득은 FieldItem 쪽 OnTriggerEnter에서 처리함 ----
    }


}
