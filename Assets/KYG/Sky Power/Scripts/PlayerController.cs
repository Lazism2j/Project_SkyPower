using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
{

        [Header("�̵� �ӵ�")]
        public float moveSpeed = 7f;

        [Header("�κ��丮 ���� (Holder)")]
        public PlayerInventoryHolder inventoryHolder; // PlayerInventoryHolder ���� (�����Ϳ��� �Ҵ�)

        CharacterController characterController;

        void Awake()
        {
            characterController = GetComponent<CharacterController>();
            // inventoryHolder = GetComponent<PlayerInventoryHolder>(); // �÷��̾� ��ü�� �پ� ������ �ڵ����� �����͵� ��
        }

        void Update()
        {
            // --- 1. �̵� �Է� �ޱ� ---
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            Vector3 dir = new Vector3(h, 0, v).normalized;

            // --- 2. �̵� ó�� ---
            characterController.Move(dir * moveSpeed * Time.deltaTime);

            // (�ɼ�) ȸ�� ó�� �� �߰�
        }

        // ---- ������ ȹ���� FieldItem �� OnTriggerEnter���� ó���� ----
    }


}
