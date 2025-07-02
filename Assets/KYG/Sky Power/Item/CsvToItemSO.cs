#if UNITY_EDITOR
using IO;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace KYG_skyPower
{
    public class CsvToItemSO : MonoBehaviour
    {
        [Header("CSV 데이터 테이블")]
        public CsvTable table; // CSV 데이터 테이블을 에디터에서 할당

        private const string assetPath = "Assets/KYG/Sky Power/Item/"; // 생성될 SO 저장 위치
        private const string prefabPath = "Assets/KYG/Sky Power/Prefabs/"; // 아이템 프리팹 경로

        [ContextMenu("CSV로부터 아이템 SO 생성")]
        public void CreateItemsFromCSV() // CSV 파일로부터 아이템 SO를 생성하는 메서드
        {
            if (table == null) // 테이블이 할당되지 않았는지 확인
            {
                Debug.LogError("CSV 테이블이 할당되지 않았습니다.");
                return;
            }

            CsvReader.Read(table); // (필요하다면) 테이블 새로고침

            for (int i = 1; i < table.Table.GetLength(0); i++) // 0번은 헤더이므로 1부터 시작
            {
                var item = ScriptableObject.CreateInstance<ItemData>(); // 아이템 데이터 SO 인스턴스 생성

                // 데이터 파싱 및 예외 처리
                if (!int.TryParse(table.GetData(i, 0), out item.id)) // ID 파싱
                {
                    Debug.LogError($"ID 파싱 실패: {table.GetData(i, 0)} (라인 {i})"); 
                    continue;
                }
                item.itemName = table.GetData(i, 1); // 아이템 이름 파싱
                if (!int.TryParse(table.GetData(i, 2), out item.itemTime)) // 지속시간 파싱
                    Debug.LogWarning($"itemTime 파싱 실패: {table.GetData(i, 2)} (라인 {i})");  
                if (!int.TryParse(table.GetData(i, 3), out item.value)) // 값 파싱
                    Debug.LogWarning($"value 파싱 실패: {table.GetData(i, 3)} (라인 {i})");  
                if (!int.TryParse(table.GetData(i, 4), out item.itemEffect)) // 아이템 효과 파싱
                    Debug.LogWarning($"itemEffect 파싱 실패: {table.GetData(i, 4)} (라인 {i})");   

                string prefabName = table.GetData(i, 5); // 프리팹 이름 파싱
                if (!string.IsNullOrEmpty(prefabName)) // 프리팹 이름이 비어있지 않은 경우
                    item.itemPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"{prefabPath}{prefabName}.prefab"); // 프리팹 경로에서 로드
                else
                    item.itemPrefab = null; // 프리팹 이름이 비어있으면 null로 설정

                item.description = table.GetData(i, 6); // 아이템 설명 파싱

                // 파일명 및 중복 삭제 처리
                string fileName = $"{assetPath}{item.itemName}_{item.id}.asset";
                if (AssetDatabase.LoadAssetAtPath<ItemData>(fileName) != null) // 이미 같은 이름의 아이템 SO가 존재하는지 확인
                    AssetDatabase.DeleteAsset(fileName); // 존재하면 삭제

                AssetDatabase.CreateAsset(item, fileName); // 아이템 SO 생성
            }

            AssetDatabase.SaveAssets(); // 변경 사항 저장
            AssetDatabase.Refresh(); // 에디터 리프레시
            Debug.Log("아이템 SO 생성 완료!");
        }
    }
}
#endif
