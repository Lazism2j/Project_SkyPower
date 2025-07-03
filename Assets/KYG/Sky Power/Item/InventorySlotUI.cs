using System; // �ݵ�� �ʿ�!
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class InventorySlotUI : MonoBehaviour // �κ��丮 ���� UI Ŭ����
    {
        public Image icon; // ������ �̹��� ������Ʈ
        public Text nameText; // ������ �̸� �ؽ�Ʈ ������Ʈ
        public Button equipButton; // ��� ��ư ������Ʈ
        public Image highlight; // ��� ���� ǥ�� �̹��� ������Ʈ

        public void Set(EquipmentData data, bool isEquipped, Action<EquipmentData> onEquip) // ���� ���� �޼��� (��� ������, ��� ����, ��� Ŭ�� �� �ݹ�)
        {
            icon.sprite = data.GetIcon(); // ������ ���� (�������� Resources �������� �ε�)
            nameText.text = data.GetName(); // ������ �̸� ����
            highlight.gameObject.SetActive(isEquipped); // ��� ������ ��� ���� ǥ�� Ȱ��ȭ
            equipButton.interactable = !isEquipped; // ��� ������ ��� ��ư ��Ȱ��ȭ
            equipButton.onClick.RemoveAllListeners(); // ���� ������ ����
            equipButton.onClick.AddListener(() => onEquip?.Invoke(data)); // ��� ��ư Ŭ�� �� �ݹ� ȣ��
        }
    }
}
