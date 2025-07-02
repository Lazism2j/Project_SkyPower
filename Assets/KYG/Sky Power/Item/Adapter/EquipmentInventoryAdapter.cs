using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

namespace KYG_skyPower
{
public class EquipmentInventoryAdapter : IInventoryItemAdapter
{
        readonly EquipmentSaveData saveData; // ��� ���� ������
        readonly EquipmentDataSO equipSO; // ��� ������ SO
        readonly CharacterController owner; // ���� ���� Ȯ�ο�

        public EquipmentInventoryAdapter(EquipmentSaveData saveData, EquipmentDataSO so, CharacterController owner) 
        {
            this.saveData = saveData; // ��� ���� ������
            this.equipSO = so; // ��� ������ SO
            this.owner = owner; // ���� ���� Ȯ�ο� ĳ���� ��Ʈ�ѷ�
        }
        public string GetName() => equipSO.itemName; // ��� �̸� ��ȯ
        public Sprite GetIcon() => equipSO.icon; // ��� ������ ��ȯ
        public int GetSortOrder() 
        {
            // 1����: ��������, 2����: ����
            if (owner != null && owner.IsEquipped(equipSO.id)) 
                return -1000 + saveData.level; // �������� ������ ��
            return 1000 - saveData.level; // �������� �ƴϸ� ������ ���� ����
        }
        public bool IsEquipped() => owner != null && owner.IsEquipped(equipSO.id); // ���� ���� Ȯ��
        public int GetLevel() => saveData.level; // ��� ���� ��ȯ, ���� �����Ϳ��� ������
    }
}