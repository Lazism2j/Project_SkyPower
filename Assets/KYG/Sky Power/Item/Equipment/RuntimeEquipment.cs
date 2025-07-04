using KYG_skyPower;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

namespace KYG_skyPower
{

// 인터페이스를 통한 런타임 장비 어댑터
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

    // 구현체 (SO, Save 결합)
    public class RuntimeEquipment : IRuntimeEquipment
    {
        public EquipmentDataSO SO { get; private set; }
        private EquipmentSave _save; // Save를 private 필드로 변경

        public EquipmentSave Save => _save; // Save를 읽기 전용 프로퍼티로 제공

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
            _save.isEquipped = true; // 장비 착용
            _save.equippedCharId = charId; // 장비 착용 시 캐릭터 ID 설정
        }
        
            
        
        public void Unequip()
        {
            _save.isEquipped = false; // 장비 해제
            _save.equippedCharId = -1; // 장비 해제 시 캐릭터 ID 초기화
        }
    }
}
