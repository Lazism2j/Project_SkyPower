using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    internal class EquipmentTableSOContainer
    {
        [CreateAssetMenu(fileName = "EquipmentTableSO", menuName = "Equipment/EquipmentTableSO")]
        public class EquipmentTableSO : ScriptableObject
        {
            public List<EquipmentDataSO> equipmentList = new List<EquipmentDataSO>();
        }
    }
}