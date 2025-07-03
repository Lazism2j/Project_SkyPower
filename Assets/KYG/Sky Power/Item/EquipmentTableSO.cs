using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Data/EquipmentTableSO")]
    public class EquipmentTableSO : ScriptableObject // ��� ������ ���̺� SO (��� �����͸� �����ϴ� ��ũ��Ʈ ������Ʈ)
    {
        public List<EquipmentData> equipmentList; // ��� ������ ����Ʈ
        public EquipmentData GetEquipmentById(int id) => equipmentList.Find(x => x.Equip_Id == id); // ID�� ��� �˻�
        public List<EquipmentData> GetListBySlot(EquipmentSlotType slotType) 
            => equipmentList.FindAll(x => x.GetSlotType() == slotType); // ���� Ÿ������ ��� ����Ʈ �˻�
    }
}
