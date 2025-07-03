using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Data/EquipmentTableSO")]
    public class EquipmentTableSO : ScriptableObject
    {
        public List<EquipmentData> equipmentList;
        public EquipmentData GetEquipmentById(int id) => equipmentList.Find(x => x.Equip_Id == id);
        public List<EquipmentData> GetListBySlot(EquipmentSlotType slotType)
            => equipmentList.FindAll(x => x.GetSlotType() == slotType);
    }
}
