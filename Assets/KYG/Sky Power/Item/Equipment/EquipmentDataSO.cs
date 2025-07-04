using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using KYG_skyPower;

namespace KYG_skyPower
{
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
    }
}