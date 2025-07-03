using LJ2;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace JYL
{
    public class PartySetPopUp : BaseUI
    {
        private CharacterSaveLoader characterLoader;
        private Image mainIllustImg;
        private Image sub1IllustImg;
        private Image sub2IllustImg;
        private Image iconPrefab;
        private RectTransform parent;
        private string iconPrefabPath = "JYL/UI/CharacterIconPrefab";
        private void Awake()
        {
            base.Awake();
            mainIllustImg = GetUI<Image>("PSCharImg1");
            sub1IllustImg = GetUI<Image>("PSCharImg2");
            sub2IllustImg = GetUI<Image>("PSCharImg3");
            characterLoader = GetComponent<CharacterSaveLoader>();
            characterLoader.GetCharPrefab();
            Array.Sort(characterLoader.charactorController, (a, b) => a.partySet.CompareTo(b.partySet)); // �߰����� ���ĵ� ������.
            iconPrefab = Resources.Load<Image>(iconPrefabPath);
            parent = GetUI<RectTransform>("Content");
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
            int imgIndex = 0;
            foreach (CharactorController character in characterLoader.charactorController)
            {
                Image go = Instantiate(iconPrefab, parent);
                go.name = $"StayCharImg{imgIndex + 1}";
                imgIndex++;
                IconSetUp(go, character);
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
            }
        }
        void Start()
        {
            characterLoader = GetComponent<CharacterSaveLoader>();
            GetEvent("PSCharImg1").Click += OpenInvenPopUp;
            GetEvent("PSCharImg2").Click += OpenInvenPopUp;
            GetEvent("PSCharImg3").Click += OpenInvenPopUp;
        }

        void Update() { }
        private void OpenInvenPopUp(PointerEventData eventData)
        {
            // ���õ� ĳ���� �������� �κ��� ����
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int index);
            // GameManager.Instance.selectSave.party[index] -> ĳ���� ID
            // ĳ���� ��Ʈ�ѷ� (ĳ���� ID)
            Debug.Log($"{index}");
            UIManager.Instance.ShowPopUp<InvenPopUp>();
        }
        private void IconSetUp(Image image, CharactorController characterData)
        {
            image.sprite = characterData.image;

        }

    }
}

