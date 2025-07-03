using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;


namespace KYG_skyPower
{


    [CreateAssetMenu(fileName = "ItemData", menuName = "Data/ItemData")]
    public class ItemData : ScriptableObject
    {
        public int id;
        public string itemName;
        public int itemTime; // ���ӽð�
        public int value; // ������
        public int itemEffect;
        public GameObject itemPrefab;
        public string description; // ������ ����
        

    }
}
