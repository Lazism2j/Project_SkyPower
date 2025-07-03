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
            string csvPath = "Assets/KYG/Sky Power/Item/Resources/Equipment_Table.csv";  // CSV 경로(에셋폴더 기준)
            string soPath = "Assets/KYG/Sky Power/Item/Editor/EquipmentTableSO.asset"; // SO 저장 경로

            List<EquipmentData> dataList = new List<EquipmentData>(); // 장비 데이터 리스트
            var lines = File.ReadAllLines(csvPath); // CSV 파일의 모든 줄 읽기

            int ParseInt(string str)
            {
                str = str.Trim();
                if (string.IsNullOrEmpty(str) || str == "-") return 0; // 빈 문자열이나 '-'인 경우 0 반환
                int v = 0; // 정수로 변환
                int.TryParse(str, out v); // TryParse를 사용하여 변환 실패 시 0 유지
                return v; 
            }

            float ParseFloat(string str) 
            {
                str = str.Trim(); // 문자열 양쪽 공백 제거
                if (string.IsNullOrEmpty(str) || str == "-") return 0f; // 빈 문자열이나 '-'인 경우 0.0f 반환
                float v = 0; 
                float.TryParse(str, out v); // TryParse를 사용하여 변환 실패 시 0.0f 유지
                return v;
            }

            // 탭으로 구분(제공하신 CSV는 탭 구분으로 보임) 
            for (int i = 1; i < lines.Length; i++) // 0번은 헤더
            {
                var tokens = lines[i].Split(','); // CSV의 각 줄을 쉼표로 분리하여 토큰화

                // 빈 줄 방지
                if (string.IsNullOrWhiteSpace(lines[i]) || tokens.Length < 19) continue; // 빈 줄이나 토큰이 부족한 경우 건너뛰기

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

            EquipmentTableSO asset = ScriptableObject.CreateInstance<EquipmentTableSO>(); // EquipmentTableSO 인스턴스 생성
            asset.equipmentList = dataList; // 장비 데이터 리스트 할당

            AssetDatabase.CreateAsset(asset, soPath); // 에셋 데이터베이스에 SO로 저장
            AssetDatabase.SaveAssets(); // 변경 사항 저장
            Debug.Log("장비 CSV → SO 변환 완료!");
        }
    }
}