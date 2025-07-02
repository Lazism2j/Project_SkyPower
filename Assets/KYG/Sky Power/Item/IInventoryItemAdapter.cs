using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KYG_skyPower
{


public interface IInventoryItemAdapter // 아이템 어댑터 인터페이스
    {
    string GetName(); // 아이템 이름
        Sprite GetIcon(); // 아이템 아이콘
        int GetSortOrder(); // 정렬 우선순위
    bool IsEquipped(); // 아이템이 장착되어 있는지 여부
        int GetLevel(); // 아이템 레벨
                        // TODO : 등급/타입 등 추가
    }
}