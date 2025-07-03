using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{

    [System.Serializable]
    public class EquipmentData : IInventoryItemAdapter // 장비 데이터 클래스 (아이템 어댑터 인터페이스 구현)
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
        public string GetName() => Equip_Name; // 아이템 이름 반환
        public Sprite GetIcon() => Resources.Load<Sprite>($"Sprites/Equipment/{Equip_Img}"); // Resources 폴더에서 아이콘 로드 (경로는 상황에 맞게 조정 필요)
        public int GetSortOrder() => Equip_Level; // 정렬 우선순위는 레벨로 설정
        public bool IsEquipped() => false; // 런타임시 캐릭터에 붙여서 처리
        public int GetLevel() => Equip_Level; // 장비 레벨 반환

    }

    [CreateAssetMenu(menuName = "Data/EquipmentTableSO")] // 장비 데이터를 저장하는 스크립트 오브젝트 생성 메뉴
    public class EquipmentTableSO : ScriptableObject // 장비 데이터를 저장하는 스크립트 오브젝트
    {
        public List<EquipmentData> equipmentList; // 장비 데이터를 저장하는 리스트
        public EquipmentData GetEquipmentById(int id) // ID로 장비 데이터를 조회하는 메서드
            => equipmentList.Find(x => x.Equip_Id == id); // 리스트에서 ID가 일치하는 장비 데이터를 찾음
    }
}