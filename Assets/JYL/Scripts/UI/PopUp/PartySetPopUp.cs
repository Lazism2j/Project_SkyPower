using LJ2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JYL
{
    public class PartySetPopUp : BaseUI
    {
        private event Action OnPartySetEnter;
        private string iconPrefabPath = "JYL/UI/CharacterIconPrefab";
        private CharacterSaveLoader characterLoader;
        private Image mainIllustImg;
        private Image sub1IllustImg;
        private Image sub2IllustImg;
        private Image iconPrefab;
        private RectTransform parent;
        private List<Image> iconList;
        private Dictionary<string, CharactorController> charDict;
        private new void Awake()
        {
            base.Awake();
            Init();
        }
        private void OnEnable()
        {
            // ���ӸŴ����� ���̺������� ���� ĳ���� ����Ʈ�� �ҷ���
            // ĳ���� ���� ��ŭ stayCharImg ���� List.Length
            // �������� =0�� ĳ���� �帲 ó��
            // ���� ���̸鼭, ��Ƽ�� ���� ĳ���� ȸ�� ó��
            // �巡��&������� ĳ�� ��
            // charImage1~3�� ���� ĳ���� ��������Ʈ �̹����� ������
            //characterLoader.charactorController[0].image
           
        }
        void Start()
        {
            characterLoader = GetComponent<CharacterSaveLoader>();
            GetEvent("PSCharImg1").Click += OpenInvenPopUp;
            GetEvent("PSCharImg2").Click += OpenInvenPopUp;
            GetEvent("PSCharImg3").Click += OpenInvenPopUp;
            CreateIcons();
        }

        void Update() { }
        private void LateUpdate()
        {

        }
        private void Init()
        {
            charDict = new Dictionary<string, CharactorController>();
            iconList = new List<Image>();
            characterLoader = GetComponent<CharacterSaveLoader>();
            mainIllustImg = GetUI<Image>("PSCharImg1");
            sub1IllustImg = GetUI<Image>("PSCharImg2");
            sub2IllustImg = GetUI<Image>("PSCharImg3");
            parent = GetUI<RectTransform>("Content");
            iconPrefab = Resources.Load<Image>(iconPrefabPath);
            characterLoader.GetCharPrefab();
        }
        private void OpenInvenPopUp(PointerEventData eventData)
        {
            // ���õ� ĳ���� �������� �κ��� ����
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int index);
            // GameManager.Instance.selectSave.party[index] -> ĳ���� ID
            // ĳ���� ��Ʈ�ѷ� (ĳ���� ID)
            UIManager.Instance.selectIndexUI = index;
            UIManager.Instance.ShowPopUp<InvenPopUp>();
        }
        private void CreateIcons()
        {
            if(iconList.Count>0)
            {
                foreach(var icon in iconList)
                {
                    Destroy(icon);
                }
                iconList.Clear();
            }

            int imgIndex = 0;
            foreach (CharactorController character in characterLoader.charactorController)
            {
                Image go = Instantiate(iconPrefab, parent);
                go.name = $"StayCharImg{imgIndex + 1}";
                imgIndex++;
                go.sprite = character.image;
                //TODO: �������� �����ܰ� �Ϸ���Ʈ�� ������ �־���� ��
                if (character.level < 0)
                {
                    // ĳ���Ͱ� ���� 1���� ������ �̼��� ĳ����
                    go.gameObject.SetActive(false);
                }
                switch (character.partySet)
                {
                    case PartySet.Main:
                        mainIllustImg.sprite = character.image;
                        break;
                    case PartySet.Sub1:
                        sub1IllustImg.sprite = character.image;
                        break;
                    case PartySet.Sub2:
                        sub2IllustImg.sprite = character.image;
                        break;
                }
                GetEvent($"{go.name}").Drag += IconDrag;
                //GetEvent($"{go.name}").EndDrag +=;
                iconList.Add(go);
                charDict.Add(go.name, character);
            }
        }
        private void IconDrag(PointerEventData eventData)
        {
            GetUI($"{eventData.pointerDrag.gameObject.name}").transform.position = eventData.position;
        }
        private void OnIconDragEnd(PointerEventData eventData)
        {
            // �������� �巡�װ� ���� ������ "��Ƽ ���� �̹���"�� ����
            // if(���� �ָ� �������� ���� ��ġ�� �ǵ���)
            // else //���� �ְ� �ƴϸ�
            // �ش� ��Ƽ ���� �̹����� �������� ĳ���� �Ϸ���Ʈ�� ��ü��
            // ���̺� �����Ϳ� �����ؼ� ��Ƽ ���� ������
        }
    }
}