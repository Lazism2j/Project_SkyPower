using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    [System.Serializable]
    public class EquipmentInventory
    {
        [SerializeField] public List<EquipmentSave> equipments = new(); // 장비 목록

        public EquipmentInventory() => equipments = new List<EquipmentSave>(); // 생성자에서 장비 목록 초기화

        // 장비 추가/레벨업
        public void AddEquipment(int equipId, EquipmentType slotType) 
        {
            for (int i = 0; i < equipments.Count; i++) 
            {
                if (equipments[i].equipId == equipId) // 이미 존재하는 장비라면
                {
                    var temp = equipments[i]; // 해당 장비를 가져와서
                    temp.level++; // 레벨을 증가시키고
                    equipments[i] = temp; // 다시 리스트에 저장
                    return;
                }
            }
            equipments.Add(new EquipmentSave(equipId, slotType)); // 존재하지 않는 장비라면 새로 추가
        }

        // 장비 장착/해제
        public void Equip(int equipId, int charId)
        {
            int idx = equipments.FindIndex(x => x.equipId == equipId); // 장비 ID로 인덱스 찾기
            if (idx >= 0)
            {
                var eq = equipments[idx]; // 해당 장비 가져오기
                eq.isEquipped = true; // 장착 상태를 true로 변경
                eq.equippedCharId = charId; // 장착 상태와 캐릭터 ID 설정
                equipments[idx] = eq; // 리스트에 다시 저장
            }
        }
        public void Unequip(int equipId)
        {
            int idx = equipments.FindIndex(x => x.equipId == equipId); // 장비 ID로 인덱스 찾기
            if (idx >= 0)
            {
                var eq = equipments[idx]; // 해당 장비 가져오기
                eq.isEquipped = false; // 장착 상태를 false로 변경
                eq.equippedCharId = -1; // 장착 해제 상태로 캐릭터 ID 초기화
                equipments[idx] = eq; // 리스트에 다시 저장
            }
        }
        // ... 기타 강화, 삭제 등도 추가
    }
}