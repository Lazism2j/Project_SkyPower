using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    [System.Serializable]
    public class EquipmentInventory
    {
        [SerializeField] public List<EquipmentSave> equipments = new(); // ��� ���

        public EquipmentInventory() => equipments = new List<EquipmentSave>(); // �����ڿ��� ��� ��� �ʱ�ȭ

        // ��� �߰�/������
        public void AddEquipment(int equipId, EquipmentType slotType) 
        {
            for (int i = 0; i < equipments.Count; i++) 
            {
                if (equipments[i].equipId == equipId) // �̹� �����ϴ� �����
                {
                    var temp = equipments[i]; // �ش� ��� �����ͼ�
                    temp.level++; // ������ ������Ű��
                    equipments[i] = temp; // �ٽ� ����Ʈ�� ����
                    return;
                }
            }
            equipments.Add(new EquipmentSave(equipId, slotType)); // �������� �ʴ� ����� ���� �߰�
        }

        // ��� ����/����
        public void Equip(int equipId, int charId)
        {
            int idx = equipments.FindIndex(x => x.equipId == equipId); // ��� ID�� �ε��� ã��
            if (idx >= 0)
            {
                var eq = equipments[idx]; // �ش� ��� ��������
                eq.isEquipped = true; // ���� ���¸� true�� ����
                eq.equippedCharId = charId; // ���� ���¿� ĳ���� ID ����
                equipments[idx] = eq; // ����Ʈ�� �ٽ� ����
            }
        }
        public void Unequip(int equipId)
        {
            int idx = equipments.FindIndex(x => x.equipId == equipId); // ��� ID�� �ε��� ã��
            if (idx >= 0)
            {
                var eq = equipments[idx]; // �ش� ��� ��������
                eq.isEquipped = false; // ���� ���¸� false�� ����
                eq.equippedCharId = -1; // ���� ���� ���·� ĳ���� ID �ʱ�ȭ
                equipments[idx] = eq; // ����Ʈ�� �ٽ� ����
            }
        }
        // ... ��Ÿ ��ȭ, ���� � �߰�
    }
}