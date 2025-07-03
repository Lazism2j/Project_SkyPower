using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace KYG_skyPower
{
    public class EquipmentSlotUI : MonoBehaviour 
    {
        public Image icon; // 아이콘 이미지 컴포넌트
        public TMP_Text nameText; // 아이템 이름 텍스트 컴포넌트
        public Button equipButton; // 장비 버튼 컴포넌트
        public Image highlight; // 장비 강조 표시 이미지 컴포넌트

        public void Set(EquipmentData data, bool isEquipped, System.Action<EquipmentData> onEquip) // 슬롯 설정 메서드 (장비 데이터, 장비 여부, 장비 클릭 시 콜백)
        {
            icon.sprite = data.GetIcon(); // 아이콘 설정 (아이콘은 Resources 폴더에서 로드)
            nameText.text = data.GetDisplayName(); // 아이템 이름 설정
            highlight.gameObject.SetActive(isEquipped); // 장비가 장착된 경우 강조 표시 활성화
            equipButton.interactable = !isEquipped; // 장비가 장착된 경우 버튼 비활성화
            equipButton.onClick.RemoveAllListeners(); // 기존 리스너 제거
            equipButton.onClick.AddListener(() => onEquip?.Invoke(data)); // 장비 버튼 클릭 시 콜백 호출
        }
    }
}
