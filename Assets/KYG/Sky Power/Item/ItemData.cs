using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;


namespace KYG_skyPower
{


    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
    public class ItemData : ScriptableObject //에셋 생성 가능 (에셋을 통해 아이템 정보를 관리하는 스크립트 오브젝트)
    {
        public int id;
        public string itemName; // 아이템 이름
        public int itemTime; // 지속시간
        public int value; // 증가량
        public int itemEffect; // 아이템 효과 (예: 0 = 없음, 1 = 공격력 증가, 2 = 방어력 증가 등)
        public GameObject itemPrefab; // 아이템 프리팹 (아이템을 생성할 때 사용할 프리팹)
        public string description; // 아이템 설명
        

    }
}
