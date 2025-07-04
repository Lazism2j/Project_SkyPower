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
        public List<EquipmentDataSO> equipmentDatabase; // SO 테이블 (필수)
        public List<EquipmentSave> savedEquipments = new(); // 세이브파일 반영

        [System.NonSerialized]
        public List<RuntimeEquipment> runtimeEquipments = new();

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

        // 장비 획득/레벨업
        public void AddEquipment(int equipId, EquipmentType slotType)
        {
            int idx = savedEquipments.FindIndex(x => x.equipId == equipId);
            if (idx >= 0)
                savedEquipments[idx] = new EquipmentSave(equipId, slotType) { level = savedEquipments[idx].level + 1 };
            else
                savedEquipments.Add(new EquipmentSave(equipId, slotType));
            BuildRuntimeInventory();
        }

        // 장비 장착/해제
        public void Equip(int equipId, int charId)
        {
            var equip = runtimeEquipments.Find(x => x.SO.Equip_Id == equipId);
            equip?.Equip(charId);
            UpdateSaveFromRuntime();
        }
        public void Unequip(int equipId)
        {
            var equip = runtimeEquipments.Find(x => x.SO.Equip_Id == equipId);
            equip?.Unequip();
            UpdateSaveFromRuntime();
        }

        // SO <-> Save 동기화 (런타임 변경분 저장)
        public void UpdateSaveFromRuntime()
        {
            foreach (var rt in runtimeEquipments)
            {
                int idx = savedEquipments.FindIndex(x => x.equipId == rt.Save.equipId);
                if (idx >= 0)
                    savedEquipments[idx] = rt.Save;
            }
        }

        // 외부 시스템에서 장비 목록 요청 (필터/정렬 확장 가능)
        public IEnumerable<RuntimeEquipment> GetEquipments(EquipmentType? filter = null, bool equippedOnly = false)
        {
            foreach (var e in runtimeEquipments)
            {
                if (filter != null && e.SO.Equip_Type != filter.Value) continue;
                if (equippedOnly && !e.IsEquipped) continue;
                yield return e;
            }
        }

        // 기타: 삭제, 강화 등도 동일하게 일원화
    }
}
