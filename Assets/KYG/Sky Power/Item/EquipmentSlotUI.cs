using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class EquipmentSlotUI : MonoBehaviour 
    {
        public Image icon; // ������ �̹��� ������Ʈ
        public TMP_Text nameText; // ������ �̸� �ؽ�Ʈ ������Ʈ
        public Button equipButton; // ��� ��ư ������Ʈ
        public Image highlight; // ��� ���� ǥ�� �̹��� ������Ʈ

        public void Set(EquipmentData data, bool isEquipped, System.Action<EquipmentData> onEquip) // ���� ���� �޼��� (��� ������, ��� ����, ��� Ŭ�� �� �ݹ�)
        {
            icon.sprite = data.GetIcon(); // ������ ���� (�������� Resources �������� �ε�)
            nameText.text = data.GetDisplayName(); // ������ �̸� ����
            highlight.gameObject.SetActive(isEquipped); // ��� ������ ��� ���� ǥ�� Ȱ��ȭ
            equipButton.interactable = !isEquipped; // ��� ������ ��� ��ư ��Ȱ��ȭ
            equipButton.onClick.RemoveAllListeners(); // ���� ������ ����
            equipButton.onClick.AddListener(() => onEquip?.Invoke(data)); // ��� ��ư Ŭ�� �� �ݹ� ȣ��
        }
    }
}
