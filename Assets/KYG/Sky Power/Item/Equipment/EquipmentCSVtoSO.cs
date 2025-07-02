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
            string csvPath = "Assets/Resources/Equipment_Table.csv";  // CSV 경로(에셋폴더 기준)
            string soPath = "Assets/Resources/EquipmentTableSO.asset"; // SO 저장 경로

            List<EquipmentData> dataList = new List<EquipmentData>();
            var lines = File.ReadAllLines(csvPath);

            // 탭으로 구분(제공하신 CSV는 탭 구분으로 보임)
            for (int i = 1; i < lines.Length; i++) // 0번은 헤더
            {
                var tokens = lines[i].Split('\t');

                // 빈 줄 방지
                if (string.IsNullOrWhiteSpace(lines[i]) || tokens.Length < 19) continue;

                var data = new EquipmentData
                {
                    Equip_Id = int.Parse(tokens[0]),
                    Equip_Grade = tokens[1],
                    Equip_Name = tokens[2],
                    Equip_Type = tokens[3],
                    Equip_Set_Type = tokens[4],
                    Equip_Img = tokens[5],
                    Equip_Level = int.Parse(tokens[6]),
                    Equip_Maxlevel = int.Parse(tokens[7]),
                    Equip_Upgrade_Default = float.Parse(tokens[8]),
                    Equip_Upgrade_Plus = float.Parse(tokens[9]),
                    Stat_Type = tokens[10],
                    Base_Value = float.Parse(tokens[11]),
                    Per_Level = float.Parse(tokens[12]),
                    Effect_Trigger = tokens[13],
                    Effect_Timing = tokens[14],
                    Effect_Type = tokens[15],
                    Effect_Value = float.Parse(tokens[16]),
                    Effect_Time = float.Parse(tokens[17]),
                    Effect_Chance = float.Parse(tokens[18]),
                    Effect_Desc = tokens.Length > 19 ? tokens[19] : ""
                };
                dataList.Add(data);
            }

            EquipmentTableSO asset = ScriptableObject.CreateInstance<EquipmentTableSO>();
            asset.equipmentList = dataList;

            AssetDatabase.CreateAsset(asset, soPath);
            AssetDatabase.SaveAssets();
            Debug.Log("장비 CSV → SO 변환 완료!");
        }
    }
}