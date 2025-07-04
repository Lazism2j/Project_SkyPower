using KYG_skyPower;
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
    public TMP_Text nameText, levelText;
    public Button equipButton;
    public GameObject equippedMark;

    public void Set(EquipmentDataSO so, EquipmentSave save, bool isEquipped, System.Action onEquip)
    {
            icon.sprite = so.GetIcon();
            nameText.text = so.Equip_Name;
        levelText.text = $"Lv.{save.level}";
        equippedMark.SetActive(isEquipped);
        equipButton.interactable = !isEquipped;
        equipButton.onClick.RemoveAllListeners();
        equipButton.onClick.AddListener(() => onEquip?.Invoke());
    }
}
}
