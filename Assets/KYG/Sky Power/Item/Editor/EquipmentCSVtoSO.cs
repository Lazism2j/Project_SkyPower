using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace KYG_skyPower
{
public class EquipmentCSVtoSO
{
        [MenuItem("Tools/CSV/Convert Equipment Table to SO")]
        public static void Convert()
        {
            string csvPath = "Assets/KYG/Sky Power/Item/Resources/Equipment_Table.csv";  // CSV ���(�������� ����)
            string soPath = "Assets/KYG/Sky Power/Item/Editor/EquipmentTableSO.asset"; // SO ���� ���

            List<EquipmentData> dataList = new List<EquipmentData>();
            var lines = File.ReadAllLines(csvPath);

            int ParseInt(string str)
            {
                str = str.Trim();
                if (string.IsNullOrEmpty(str) || str == "-") return 0;
                int v = 0;
                int.TryParse(str, out v);
                return v;
            }

            float ParseFloat(string str)
            {
                str = str.Trim();
                if (string.IsNullOrEmpty(str) || str == "-") return 0f;
                float v = 0;
                float.TryParse(str, out v);
                return v;
            }

            // ������ ����(�����Ͻ� CSV�� �� �������� ����)
            for (int i = 1; i < lines.Length; i++) // 0���� ���
            {
                var tokens = lines[i].Split(',');

                // �� �� ����
                if (string.IsNullOrWhiteSpace(lines[i]) || tokens.Length < 19) continue;

                var data = new EquipmentData
                {
                    Equip_Id = ParseInt(tokens[0]),
                    Equip_Grade = tokens[1].Trim(),
                    Equip_Name = tokens[2].Trim(),
                    Equip_Type = tokens[3].Trim(),
                    Equip_Set_Type = tokens[4].Trim(),
                    Equip_Img = tokens[5].Trim(),
                    Equip_Level = ParseInt(tokens[6]),
                    Equip_Maxlevel = ParseInt(tokens[7]),
                    Equip_Upgrade_Default = ParseFloat(tokens[8]),
                    Equip_Upgrade_Plus = ParseFloat(tokens[9]),
                    Stat_Type = tokens[10].Trim(),
                    Base_Value = ParseFloat(tokens[11]),
                    Per_Level = ParseFloat(tokens[12]),
                    Effect_Trigger = tokens[13].Trim(),
                    Effect_Timing = tokens[14].Trim(),
                    Effect_Type = tokens[15].Trim(),
                    Effect_Value = ParseFloat(tokens[16]),
                    Effect_Time = ParseFloat(tokens[17]),
                    Effect_Chance = ParseFloat(tokens[18]),
                    Effect_Desc = tokens.Length > 19 ? tokens[19].Trim() : ""
                };
                dataList.Add(data);
            }

            EquipmentTableSO asset = ScriptableObject.CreateInstance<EquipmentTableSO>();
            asset.equipmentList = dataList;

            AssetDatabase.CreateAsset(asset, soPath);
            AssetDatabase.SaveAssets();
            Debug.Log("��� CSV �� SO ��ȯ �Ϸ�!");
        }
    }
}