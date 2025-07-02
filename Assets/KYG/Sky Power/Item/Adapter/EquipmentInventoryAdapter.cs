using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

namespace KYG_skyPower
{
public class EquipmentInventoryAdapter : IInventoryItemAdapter
{
        readonly EquipmentSaveData saveData; // 장비 저장 데이터
        readonly EquipmentDataSO equipSO; // 장비 데이터 SO
        readonly CharacterController owner; // 장착 여부 확인용

        public EquipmentInventoryAdapter(EquipmentSaveData saveData, EquipmentDataSO so, CharacterController owner) 
        {
            this.saveData = saveData; // 장비 저장 데이터
            this.equipSO = so; // 장비 데이터 SO
            this.owner = owner; // 장착 여부 확인용 캐릭터 컨트롤러
        }
        public string GetName() => equipSO.itemName; // 장비 이름 반환
        public Sprite GetIcon() => equipSO.icon; // 장비 아이콘 반환
        public int GetSortOrder() 
        {
            // 1순위: 장착여부, 2순위: 레벨
            if (owner != null && owner.IsEquipped(equipSO.id)) 
                return -1000 + saveData.level; // 장착중이 무조건 위
            return 1000 - saveData.level; // 장착중이 아니면 레벨에 따라 정렬
        }
        public bool IsEquipped() => owner != null && owner.IsEquipped(equipSO.id); // 장착 여부 확인
        public int GetLevel() => saveData.level; // 장비 레벨 반환, 저장 데이터에서 가져옴
    }
}