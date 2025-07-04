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

        public List<EquipmentDataSO> equipmentDatabase;         // SO 테이블 (필수)
        public List<EquipmentSave> savedEquipments = new();     // 세이브파일 반영

        [System.NonSerialized]
        public List<RuntimeEquipment> runtimeEquipments = new(); // 런타임 통합

        // SO와 Save를 합친 실제 사용 구조체
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

        // 런타임 동기화
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

        // 장착/해제 (캐릭터 ID 연동)
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
        // 기타 강화, 삭제, 정렬 등 필요시 추가
    }
}
