using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    public enum EquipmentSlotType { Weapon = 0, Armor = 1, Accessory = 2, None = -1 } // ��� ���� Ÿ�� ���� (����, ��, �׼�����, ����)

    [System.Serializable]
    public class EquipmentData : IInventoryItemAdapter // ��� ������ Ŭ���� (������ ����� �������̽� ����)
    {
        public int Equip_Id;
        public string Equip_Grade;
        public string Equip_Name;
        public string Equip_Type;
        public string Equip_Set_Type;
        public string Equip_Img;
        public int Equip_Level;
        public int Equip_Maxlevel;
        public float Equip_Upgrade_Default;
        public float Equip_Upgrade_Plus;
        public string Stat_Type;
        public float Base_Value;
        public float Per_Level;
        public string Effect_Trigger;
        public string Effect_Timing;
        public string Effect_Type;
        public float Effect_Value;
        public float Effect_Time;
        public float Effect_Chance;
        public string Effect_Desc;

        // --- Adapter Pattern ---
        public string GetName() => Equip_Name; // ������ �̸� ��ȯ
        public Sprite GetIcon() => Resources.Load<Sprite>($"Sprites/Equipment/{Equip_Img}"); // Resources �������� ������ �ε� (��δ� ��Ȳ�� �°� ���� �ʿ�)
        public int GetSortOrder() => Equip_Level; // ���� �켱������ ������ ����
        public bool IsEquipped() => false; // ��Ÿ�ӽ� ĳ���Ϳ� �ٿ��� ó��
        public int GetLevel() => Equip_Level; // ��� ���� ��ȯ

        // Ȯ��
        public EquipmentSlotType GetSlotType() // ��� Ÿ�Կ� ���� ���� Ÿ�� ��ȯ
        {
            switch (Equip_Type.ToLower()) // �ҹ��ڷ� ��ȯ�Ͽ� �� (��ҹ��� ���� ���� ó��)
            {
                case "weapon": return EquipmentSlotType.Weapon; // ���� Ÿ��
                case "armor": return EquipmentSlotType.Armor; // �� Ÿ��
                case "accessory": return EquipmentSlotType.Accessory; // �׼����� Ÿ��
                default: return EquipmentSlotType.None; // �� �� ���� Ÿ���� None���� ó��
            }
        }
        public string GetDisplayName() => $"{Equip_Name} [Lv.{Equip_Level}]"; // ��� �̸��� ������ �����Ͽ� ǥ�ÿ� �̸� ��ȯ
        public Color GetGradeColor() // ��� ��޿� ���� ���� ��ȯ
        {
            switch (Equip_Grade) 
            {
                case "Legend": return Color.yellow; // ���� ����� �����
                case "Epic": return new Color(0.5f, 0, 1); // ���� ����� �����
                default: return Color.white; // �� �� ����� ���
            }
        }

    }

    [CreateAssetMenu(menuName = "Data/EquipmentTableSO")] // ��� �����͸� �����ϴ� ��ũ��Ʈ ������Ʈ ���� �޴�
    public class EquipmentTableSO : ScriptableObject // ��� �����͸� �����ϴ� ��ũ��Ʈ ������Ʈ
    {
        public List<EquipmentData> equipmentList; // ��� �����͸� �����ϴ� ����Ʈ
        public EquipmentData GetEquipmentById(int id) // ID�� ��� �����͸� ��ȸ�ϴ� �޼���
            => equipmentList.Find(x => x.Equip_Id == id); // ����Ʈ���� ID�� ��ġ�ϴ� ��� �����͸� ã��

        public List<EquipmentData> GetListBySlot(EquipmentSlotType slotType) // Ư�� ���� Ÿ�Կ� �ش��ϴ� ��� �����͸� ��ȸ�ϴ� �޼���
            => equipmentList.FindAll(x => x.GetSlotType() == slotType); // Ư�� ���� Ÿ�Կ� �ش��ϴ� ��� ������ ����Ʈ ��ȯ
    }
}