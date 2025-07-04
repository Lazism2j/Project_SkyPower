using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    public enum EquipmentGrade { Legend, Normal } // 장비 등급 (레전드, 일반 등)
    public enum EquipmentType { Weapon, Armor, Accessory } // 장비 종류 (무기, 방어구, 악세사리 등)
    public enum EquipmentSetType { 충전의, 응급의, 전장의, 맹공의, 광속의 } // 장비 세트 종류 (충전의, 응급의 등)
    public enum StatType { Atk, Hp } // 능력치 종류 (공격력, 체력 등)
    public enum EffectType { UltGage, Hp, Shild, Atk, AtkSpeed } // 효과 종류 (궁극기 게이지, 체력, 방어막 등)

    [CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "Equipment/EquipmentDataSO")] 
    public class EquipmentDataSO : ScriptableObject
    {
        public int Equip_Id; // 장비 ID (고유 식별자)

        public EquipmentGrade Equip_Grade; // 장비 등급 (레전드, 일반 등)

        public string Equip_Name; // 장비 이름

        public EquipmentType Equip_Type; // 장비 종류 (무기, 방어구, 악세사리 등)

        public EquipmentSetType Equip_Set_Type; // 장비 세트 종류 (충전의, 응급의 등)

        public string Equip_Img; // 장비 이미지 경로 (리소스 폴더 내)

        public int Equip_Maxlevel; // 장비 최대 레벨 (SO에서 설정, 런타임에 사용)

        public float Equip_Upgrade_Default; // 장비 업그레이드 기본값 (레벨 1 기준)

        public float Equip_Upgrade_Plus; // 장비 업그레이드 추가값 (레벨 상승 시 증가하는 값)

        public StatType Stat_Type; // 능력치 종류 (공격력, 체력 등)

        public float Base_Value; // 기본 능력치 값 (레벨 1 기준)

        public float Per_Level; // 레벨당 증가하는 능력치 값 (업그레이드 시 적용)

        public string Effect_Trigger; // 효과 트리거 (예: 장착 시, 공격 시 등)

        public string Effect_Timing; // 효과 타이밍 (예: 즉시, 지속 등)

        public EffectType Effect_Type; // 효과 종류 (궁극기 게이지, 체력, 방어막 등)

        public float Effect_Value; // 효과 값 (예: 공격력 증가, 체력 회복 등)

        public float Effect_Time; // 효과 지속 시간 (초 단위)

        public float Effect_Chance; // 효과 발동 확률 (0~1 사이, 0.5는 50% 확률)

        [TextArea] public string Effect_Desc; // 효과 설명 (에디터에서 입력 가능)
    }
}