using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class EquipmentSlotUI : MonoBehaviour
    {
        public Image icon;
        public TMP_Text nameText;
        public Button equipButton;
        public Image highlight;

        public void Set(EquipmentData data, bool isEquipped, System.Action<EquipmentData> onEquip)
        {
            icon.sprite = data.GetIcon();
            nameText.text = data.GetDisplayName();
            highlight.gameObject.SetActive(isEquipped);
            equipButton.interactable = !isEquipped;
            equipButton.onClick.RemoveAllListeners();
            equipButton.onClick.AddListener(() => onEquip?.Invoke(data));
        }
    }
}
