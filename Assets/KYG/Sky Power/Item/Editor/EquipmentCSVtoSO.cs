using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using static KYG_skyPower.EquipmentTableSOContainer;

namespace KYG_skyPower
{
    public class EquipmentCSVtoSO
    {
        [MenuItem("Tools/CSV/Convert Equipment Table to SO")]
        public static void Convert()
        {
            string csvPath = "Assets/KYG/Sky Power/Item/Resources/Equipment_Table.csv";
            string soPath = "Assets/KYG/Sky Power/Item/Editor/EquipmentTableSO.asset";

            var lines = File.ReadAllLines(csvPath);

            // EquipmentTableSO �ν��Ͻ�
            EquipmentTableSO asset = ScriptableObject.CreateInstance<EquipmentTableSO>();
            asset.equipmentList = new List<EquipmentDataSO>();

            for (int i = 1; i < lines.Length; i++) // 0���� ���
            {
                var tokens = lines[i].Split(',');
                if (string.IsNullOrWhiteSpace(lines[i]) || tokens.Length < 19) continue;

                // EquipmentDataSO �ν��Ͻ� ���� (SO�� �ݵ�� �̷��� ����!)
                var data = ScriptableObject.CreateInstance<EquipmentDataSO>();

                data.Equip_Id = ParseInt(tokens[0]);
                data.Equip_Grade = ParseEnum<EquipmentGrade>(tokens[1]);
                data.Equip_Name = tokens[2].Trim();
                data.Equip_Type = ParseEnum<EquipmentType>(tokens[3]);
                data.Equip_Set_Type = ParseEnum<EquipmentSetType>(tokens[4]);
                data.Equip_Img = tokens[5].Trim();
                data.Equip_Maxlevel = ParseInt(tokens[7]);
                data.Equip_Upgrade_Default = ParseFloat(tokens[8]);
                data.Equip_Upgrade_Plus = ParseFloat(tokens[9]);
                data.Stat_Type = ParseEnum<StatType>(tokens[10]);
                data.Base_Value = ParseFloat(tokens[11]);
                data.Per_Level = ParseFloat(tokens[12]);
                data.Effect_Trigger = tokens[13].Trim();
                data.Effect_Timing = tokens[14].Trim();
                data.Effect_Type = ParseEnum<EffectType>(tokens[15]);
                data.Effect_Value = ParseFloat(tokens[16]);
                data.Effect_Time = ParseFloat(tokens[17]);
                data.Effect_Chance = ParseFloat(tokens[18]);
                data.Effect_Desc = tokens.Length > 19 ? tokens[19].Trim() : "";

                asset.equipmentList.Add(data); // ����Ʈ�� �߰�
            }

            AssetDatabase.CreateAsset(asset, soPath);
            AssetDatabase.SaveAssets();
            Debug.Log("��� CSV �� SO ��ȯ �Ϸ�!");
        }

        // enum �Ľ� ��ƿ (��ҹ��� ���� ��)
        private static T ParseEnum<T>(string value) where T : struct
        {
            if (string.IsNullOrEmpty(value) || value == "-") return default;
            T result;
            if (System.Enum.TryParse(value.Trim(), true, out result))
                return result;
            Debug.LogWarning($"Enum �Ľ� ����: {typeof(T)} �� '{value}'");
            return default;
        }

        private static int ParseInt(string str)
        {
            str = str.Trim();
            if (string.IsNullOrEmpty(str) || str == "-") return 0;
            int v = 0;
            int.TryParse(str, out v);
            return v;
        }

        private static float ParseFloat(string str)
        {
            str = str.Trim();
            if (string.IsNullOrEmpty(str) || str == "-") return 0f;
            float v = 0;
            float.TryParse(str, out v);
            return v;
        }
    }
}
