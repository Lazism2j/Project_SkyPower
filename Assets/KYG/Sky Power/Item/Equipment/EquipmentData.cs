using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{

    [System.Serializable]
    public class EquipmentData
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
    }

    [CreateAssetMenu(menuName = "Data/EquipmentTableSO")]
    public class EquipmentTableSO : ScriptableObject
    {
        public List<EquipmentData> equipmentList;
    }
}