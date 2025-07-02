using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    [CreateAssetMenu(menuName = "Manager/ItemManagerSO")]
    public class ItemManagerSO : ScriptableObject // 아이템 관리 매니저 SO (아이템 데이터 SO를 관리하는 스크립트 오브젝트) 
    {
        [Header("아이템 데이터 SO 리스트 (수동 or 자동 할당)")]
        public List<ItemData> allItems = new List<ItemData>(); // 아이템 데이터 SO를 저장하는 리스트 (수동 또는 자동으로 할당)

        // ID로 검색
        public ItemData GetItemById(int id) // 아이템 ID로 검색하는 메서드
        {
            return allItems.Find(item => item != null && item.id == id); // 아이템 ID로 검색하는 메서드
        }

        // 이름으로 검색
        public ItemData GetItemByName(string name) // 아이템 이름으로 검색하는 메서드
        {
            return allItems.Find(item => item != null && item.itemName == name); // 아이템 이름으로 검색하는 메서드
        }

#if UNITY_EDITOR
        // 에디터 자동 등록 기능 (폴더 내의 모든 ItemData SO 등록)
        [ContextMenu("폴더에서 자동으로 아이템 SO 모두 등록")] 
        public void AutoCollectAllItems() // 에디터에서만 동작하는 자동 등록 메서드
        {
            allItems.Clear(); // 기존 아이템 리스트 초기화
            string[] guids = UnityEditor.AssetDatabase.FindAssets("t:ItemData", new[] { "Assets/KYG/Sky Power/Item" }); // "Assets/KYG/Sky Power/Item" 폴더 내의 모든 ItemData SO를 검색
            foreach (string guid in guids) // 검색된 GUID를 순회하며
            {
                string path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid); // GUID를 경로로 변환
                ItemData item = UnityEditor.AssetDatabase.LoadAssetAtPath<ItemData>(path); // 경로에서 ItemData SO를 로드
                if (item != null && !allItems.Contains(item)) // 아이템이 null이 아니고 리스트에 포함되어 있지 않으면
                    allItems.Add(item); // 아이템을 리스트에 추가
            }
            UnityEditor.EditorUtility.SetDirty(this); // 변경 사항 저장
            Debug.Log($"[ItemManagerSO] {allItems.Count}개 SO 자동 등록 완료!"); // 등록 완료 메시지 출력
        }
#endif
    }
}
