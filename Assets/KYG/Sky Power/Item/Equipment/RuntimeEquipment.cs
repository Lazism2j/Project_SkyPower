using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

namespace KYG_skyPower
{

// �������̽��� ���� ��Ÿ�� ��� �����
public interface IRuntimeEquipment
{
    EquipmentDataSO SO { get; }
    EquipmentSave Save { get; }
    bool IsEquipped { get; }
    int Level { get; }
    int EquippedCharId { get; }
    void Equip(int charId);
    void Unequip();
    }

    // ����ü (SO, Save ����)
    public class RuntimeEquipment : IRuntimeEquipment
    {
        public EquipmentDataSO SO { get; private set; }
        private EquipmentSave _save; // Save�� private �ʵ�� ����

        public EquipmentSave Save => _save; // Save�� �б� ���� ������Ƽ�� ����

        public bool IsEquipped => _save.isEquipped;
        public int Level => _save.level;
        public int EquippedCharId => _save.equippedCharId;

        public RuntimeEquipment(EquipmentDataSO so, EquipmentSave save)
        {
            SO = so;
            _save = save;
        }
        public void Equip(int charId)
        {
            _save.isEquipped = true; // ��� ����
            _save.equippedCharId = charId; // ��� ���� �� ĳ���� ID ����
        }
        
            
        
        public void Unequip()
        {
            _save.isEquipped = false; // ��� ����
            _save.equippedCharId = -1; // ��� ���� �� ĳ���� ID �ʱ�ȭ
        }
    }
}
