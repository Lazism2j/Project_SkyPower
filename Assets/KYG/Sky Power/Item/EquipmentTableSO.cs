using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Data/EquipmentTableSO")]
    public class EquipmentTableSO : ScriptableObject // 장비 데이터 테이블 SO (장비 데이터를 관리하는 스크립트 오브젝트)
    {
        public List<EquipmentData> equipmentList; // 장비 데이터 리스트
        public EquipmentData GetEquipmentById(int id) => equipmentList.Find(x => x.Equip_Id == id); // ID로 장비 검색
        public List<EquipmentData> GetListBySlot(EquipmentSlotType slotType) 
            => equipmentList.FindAll(x => x.GetSlotType() == slotType); // 슬롯 타입으로 장비 리스트 검색
    }
}
