using System;
using UnityEngine;

namespace KYG_skyPower
{
    public class EquipmentData
    {
        public object Equip_Id { get; internal set; }
        public string Equip_Type { get; internal set; }

        internal Sprite GetIcon()
        {
            throw new NotImplementedException();
        }

        internal string GetName()
        {
            throw new NotImplementedException();
        }
    }
}