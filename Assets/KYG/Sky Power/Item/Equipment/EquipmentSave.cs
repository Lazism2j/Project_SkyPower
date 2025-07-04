using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    [Serializable]
    public struct EquipmentSave
    {
        public int equipId;
        public int level;
        public bool isEquipped;
        public int equippedCharId;
        public EquipmentType slotType; // 캐릭터 슬롯/장착 부위 등 활용

        public EquipmentSave(int equipId, EquipmentType slotType)
        {
            this.equipId = equipId;
            this.level = 1;
            this.isEquipped = false;
            this.equippedCharId = -1;
            this.slotType = slotType;
        }
    }
}