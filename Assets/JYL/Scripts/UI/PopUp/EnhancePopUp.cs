using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


namespace JYL
{
    public class EnhancePopUp : BaseUI
    {
        private Image enhanceTypeImg => GetUI<Image>("EnhanceMenuBack");
        [SerializeField] private Sprite charEnhanceImg;
        [SerializeField] private Sprite wpEnhanceImg;
        [SerializeField] private Sprite amEnhanceImg;
        // UIManager, GameManager ���� ���� ���� ���� ��ȭ â�� ĳ����ȭâ���� ��� ��ȭâ���� �Ǻ���
        // ĳ�� �Ǵ� ��� ��ȭ �Ҹ� �� Level�� �ø�.
        
        // �� �ʵ���� ������������ �Ŵ������� ���� �����´�.
        // private CharacterController CharacterController;
        // private Item item;

        void Start()
        {
            switch(UIManager.Instance.selectIndexUI)
            {
                case 0:
                    enhanceTypeImg.sprite = charEnhanceImg;
                    GetEvent("EnhanceBtn").Click += CharacterEnhance;
                    break;
                case 1:
                    enhanceTypeImg.sprite = wpEnhanceImg;
                    GetEvent("EnhanceBtn").Click += EquipEnhance;
                    break;
                case 2:
                    enhanceTypeImg.sprite = amEnhanceImg;
                    GetEvent("EnhanceBtn").Click += EquipEnhance;
                    break;

            }
            //GetEvent("EnhanceBtn").Click += data => item.level++; �Ǵ� �������� �Լ� ����
        }

        private void CharacterEnhance(PointerEventData eventData)
        {
            // TODO : ĳ���� ��ȭ ����
            // ��ȭ�� ����� ��
             UIManager.Instance.ClosePopUp();
        }
        private void EquipEnhance(PointerEventData eventData)
        {
            // TODO : ��� ��ȭ ����
            // ��ȭ�� ����� ��
            UIManager.Instance.ClosePopUp();
        }
    }
}

