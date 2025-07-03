using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

namespace JYL
{
    public class InvenPopUp : BaseUI
    {
        // ���ӸŴ������� ĳ���� ������ �ҷ��;� ��
        private GameObject invenScroll => GetUI("InvenScroll");
        private TMP_Text invenCharName => GetUI<TMP_Text>("InvenCharNameText");
        private TMP_Text level => GetUI<TMP_Text>("InvenCharLevelText");
        private TMP_Text hp => GetUI<TMP_Text>("InvenCharHPText");
        private TMP_Text ap => GetUI<TMP_Text>("InvenCharAPText");
        // TODO: GameManager.CharacterController[] character => for(int i = 0;i<character.Length;i++) { �κ��丮�� UI�߰� }
        
        // private Item[] items;


        void Start()
        {
            // ��� Ŭ���� Ȱ��ȭ
            invenScroll.SetActive(false);
            GetEvent("WeaponBtn").Click += OpenWPInven;
            GetEvent("WPEnhanceBtn1").Click += OpenWPEnhance;
            GetEvent("ArmorBtn").Click += OpenAMInven;
            GetEvent("AMEnhanceBtn2").Click += OpenAMEnhance;
            GetEvent("AccessoryBtn").Click += OpenACInven;
            GetEvent("ACEnhanceBtn3").Click += OpenACEnhance;
            GetEvent("CharEnhanceBtn").Click += OpenCharEnhance;

            // ���� ĳ������ ������ ǥ�õȴ�
            // index�� UIManager�� ����
            // GameManager.Instance.character[index]
            invenCharName.text = "ĳ����1";
            level.text = "24";
            hp.text = "2040";
            ap.text = "332";
        }

        private void OpenWPInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
            // �κ��丮 �Ŵ������� ����� Ÿ�� �����ؼ� ������ ������
            // items = InvenManager.Instance.GetItemList(Type weapon)
            // items�� ������ invenscroll ����
            // �� UI ��ҵ鿡 �̺�Ʈ �޾ƾ� ��
            // foreach(Button ui in items)
            // { GetEvent("").Click += �����۱�ü �Լ� }
            // �ش� UI Ŭ�� ��, �������̴� ���� ��ȯ
            // InvenManager.Instance.Add()
            // InvenManager.Instance.RemoveAt(�κ�ID������ ã�� �����)
        }

        // ������ ��ü �Լ�
        // ĳ���� ��Ʈ�ѷ��� ������� �������� ID�� ���� �ʿ�

        private void OpenWPEnhance(PointerEventData eventData)
        {
            // ���� ������ ������ ����������
            // �����ϴ� UI �������� UIManager�� ���� �����Ѵ�.
            // GameManager.Instance.Party[0].
            // UIManager.Instance. ���� ������ ĳ�������� + ���� -> Enhance �˾��� �ҷ��;� ��
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }
        private void OpenAMInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
        }

        private void OpenAMEnhance(PointerEventData eventData)
        {
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }
        private void OpenACInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
        }

        private void OpenACEnhance(PointerEventData eventData)
        {
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }
        private void OpenCharEnhance(PointerEventData eventData)
        {
            // ĳ���� ������ ������ ��ȭâ ����
            // UIManager���� ���õ� ĳ������ �ε��� ������ GameManager�� ��Ƽ �������� ������ ���� ĳ���� ��Ʈ�ѷ� ���� �ҷ���
            // �ش� ������ ��ȭâ���� �ҷ��� ���⼭ �Ⱥҷ���
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }
    }
}


