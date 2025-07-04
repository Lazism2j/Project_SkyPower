using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
    public static class EnumUtil
    {
        public static T ParseEnum<T>(string value, T defaultValue = default) where T : struct
        {
            if (string.IsNullOrEmpty(value)) return defaultValue;
            if (System.Enum.TryParse(value, true, out T result))
                return result;
            return defaultValue;
        }
    }

    public enum EquipmentGrade { Legend, Normal } // ��� ��� (������, �Ϲ� ��)
    public enum EquipmentType { Weapon, Armor, Accessory } // ��� ���� (����, ��, �Ǽ��縮 ��)
    public enum EquipmentSetType { ������, ������, ������, �Ͱ���, ������ } // ��� ��Ʈ ���� (������, ������ ��)
    public enum StatType { Atk, Hp } // �ɷ�ġ ���� (���ݷ�, ü�� ��)
    public enum EffectType { UltGage, Hp, Shild, Atk, AtkSpeed } // ȿ�� ���� (�ñر� ������, ü��, �� ��)

    [CreateAssetMenu(fileName = "EquipmentDataSO", menuName = "Equipment/EquipmentDataSO")]
    public class EquipmentDataSO : ScriptableObject
    {
        public int Equip_Id; // ��� ID (���� �ĺ���)

        public EquipmentGrade Equip_Grade; // ��� ��� (������, �Ϲ� ��)

        public string Equip_Name; // ��� �̸�

        public EquipmentType Equip_Type; // ��� ���� (����, ��, �Ǽ��縮 ��)

        public EquipmentSetType Equip_Set_Type; // ��� ��Ʈ ���� (������, ������ ��)

        public string Equip_Img; // ��� �̹��� ��� (���ҽ� ���� ��)

        public int Equip_Maxlevel; // ��� �ִ� ���� (SO���� ����, ��Ÿ�ӿ� ���)

        public float Equip_Upgrade_Default; // ��� ���׷��̵� �⺻�� (���� 1 ����)

        public float Equip_Upgrade_Plus; // ��� ���׷��̵� �߰��� (���� ��� �� �����ϴ� ��)

        public StatType Stat_Type; // �ɷ�ġ ���� (���ݷ�, ü�� ��)

        public float Base_Value; // �⺻ �ɷ�ġ �� (���� 1 ����)

        public float Per_Level; // ������ �����ϴ� �ɷ�ġ �� (���׷��̵� �� ����)

        public string Effect_Trigger; // ȿ�� Ʈ���� (��: ���� ��, ���� �� ��)

        public string Effect_Timing; // ȿ�� Ÿ�̹� (��: ���, ���� ��)

        public EffectType Effect_Type; // ȿ�� ���� (�ñر� ������, ü��, �� ��)

        public float Effect_Value; // ȿ�� �� (��: ���ݷ� ����, ü�� ȸ�� ��)

        public float Effect_Time; // ȿ�� ���� �ð� (�� ����)

        public float Effect_Chance; // ȿ�� �ߵ� Ȯ�� (0~1 ����, 0.5�� 50% Ȯ��)

        [TextArea] public string Effect_Desc; // ȿ�� ���� (�����Ϳ��� �Է� ����)

        public string GetDisplayName()
        {
            return $"{Equip_Grade} {Equip_Name}";
        }

        public string GetStatSummary()
        {
            return $"{Stat_Type}: {Base_Value} (+{Per_Level}/Lv)";
        }

        // ���ҽ� ��� ��ƿ
        public Sprite GetIcon()
        {
            return Resources.Load<Sprite>(Equip_Img);
        }

        // Ȥ�� ���ڿ��� ������ Ÿ�� �� enum ĳ������ �ʿ��ϴٸ�:
        public static EquipmentType ToEquipmentType(string str)
        {
            return EnumUtil.ParseEnum(str, EquipmentType.Weapon);
        }
    }

}