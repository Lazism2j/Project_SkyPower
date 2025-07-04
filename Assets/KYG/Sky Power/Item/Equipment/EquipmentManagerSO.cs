using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/EquipmentManagerSO")]
    public class EquipmentManagerSO : ScriptableObject
    {
        private static EquipmentManagerSO _instance;
        public static EquipmentManagerSO Instance
        {
            get
            {
                if (_instance == null)
                    _instance = Resources.Load<EquipmentManagerSO>("EquipmentManagerSO");
                return _instance;
            }
        }

        public List<EquipmentDataSO> equipmentDatabase;         // SO ���̺� (�ʼ�)
        public List<EquipmentSave> savedEquipments = new();     // ���̺����� �ݿ�

        [System.NonSerialized]
        public List<RuntimeEquipment> runtimeEquipments = new(); // ��Ÿ�� ����

        // SO�� Save�� ��ģ ���� ��� ����ü
        public class RuntimeEquipment
        {
            public EquipmentDataSO so;
            public EquipmentSave save;

            public RuntimeEquipment(EquipmentDataSO so, EquipmentSave save)
            {
                this.so = so;
                this.save = save;
            }
        }

        // ��Ÿ�� ����ȭ
        public void BuildRuntimeInventory()
        {
            runtimeEquipments.Clear();
            foreach (var save in savedEquipments)
            {
                var so = equipmentDatabase.Find(x => x.Equip_Id == save.equipId);
                if (so != null)
                    runtimeEquipments.Add(new RuntimeEquipment(so, save));
            }
        }

        // ��� ȹ��/������
        public void AddEquipment(int equipId, EquipmentType slotType)
        {
            int idx = savedEquipments.FindIndex(x => x.equipId == equipId);
            if (idx >= 0)
            {
                var eq = savedEquipments[idx];
                eq.level++;
                savedEquipments[idx] = eq;
            }
            else
            {
                savedEquipments.Add(new EquipmentSave(equipId, slotType));
            }
            BuildRuntimeInventory();
        }

        // ����/���� (ĳ���� ID ����)
        public void Equip(int equipId, int charId)
        {
            int idx = savedEquipments.FindIndex(x => x.equipId == equipId);
            if (idx >= 0)
            {
                var eq = savedEquipments[idx];
                eq.isEquipped = true;
                eq.equippedCharId = charId;
                savedEquipments[idx] = eq;
                BuildRuntimeInventory();
            }
        }
        public void Unequip(int equipId)
        {
            int idx = savedEquipments.FindIndex(x => x.equipId == equipId);
            if (idx >= 0)
            {
                var eq = savedEquipments[idx];
                eq.isEquipped = false;
                eq.equippedCharId = -1;
                savedEquipments[idx] = eq;
                BuildRuntimeInventory();
            }
        }
        // ��Ÿ ��ȭ, ����, ���� �� �ʿ�� �߰�
    }
}
