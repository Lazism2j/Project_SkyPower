using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using LJ2;
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
        private Image charImage;
        private CharacterSaveLoader characterLoader;

        private void OnEnable()
        {
        }
        void Start()
        {
            
            // ��� Ŭ���� Ȱ��ȭ
            characterLoader = GetComponent<CharacterSaveLoader>();
            characterLoader.GetCharPrefab();
            Init();

            invenScroll.SetActive(false);
            GetEvent("CharEnhanceBtn1").Click += OpenCharEnhance;
            GetEvent("WeaponBtn").Click += OpenWPInven;
            GetEvent("WPEnhanceBtn2").Click += OpenWPEnhance;
            GetEvent("ArmorBtn").Click += OpenAMInven;
            GetEvent("AMEnhanceBtn3").Click += OpenAMEnhance;
            GetEvent("AccessoryBtn").Click += OpenACInven;

            // ���� ĳ������ ������ ǥ�õȴ�
            // index�� UIManager�� ����
            // GameManager.Instance.character[index]
            invenCharName.text = "ĳ����1";
            level.text = "24";
            hp.text = "2040";
            ap.text = "332";
            Init();
        }
        private void Init()
        {
            charImage = GetUI<Image>("InvenCharImage");
            CharactorController charCont;
            foreach(CharactorController cont in characterLoader.charactorController)
            {
                if(cont.partySet == PartySet.Main)
                {
                    charCont = cont;
                    charImage.sprite = charCont.image;
                }
            }
        }
        private void OpenCharEnhance(PointerEventData eventData)
        {
            // ĳ���� ������ ������ ��ȭâ ����
            // UIManager���� ���õ� ĳ������ �ε��� ������ GameManager�� ��Ƽ �������� ������ ���� ĳ���� ��Ʈ�ѷ� ���� �ҷ���
            // �ش� ������ ��ȭâ���� �ҷ��� ���⼭ �Ⱥҷ���
            UIManager.Instance.selectIndexUI = 1;
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
            // UI ������ ��, UI���ٰ� �̺�Ʈ �ټ���.
            // Image img = Instantiate();
            // GetEvent($"img.gameObject.name").Click += �̺�Ʈ�Լ�;
        }

        private void OpenWPEnhance(PointerEventData eventData)
        {
            UIManager.Instance.selectIndexUI = 2;
            // ���� ������ ������ ����������
            // �����ϴ� UI �������� UIManager�� ���� �����Ѵ�.
            // GameManager.Instance.Party[0].
            // UIManager.Instance. ���� ������ ĳ�������� + ���� -> Enhance �˾��� �ҷ��;� ��
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }

        private void OpenAMEnhance(PointerEventData eventData)
        {
            UIManager.Instance.selectIndexUI = 3;
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
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

        private void OpenAMInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
        }
        private void OpenACInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
        }
    }
}


