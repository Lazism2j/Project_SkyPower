using LJ2;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using KYG_skyPower;
using TMPro;

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
        private List<CharacterSave> charDataList;
        private RectTransform dragIconTransform;
        private Vector2 originAnchoredPos;
        private RectTransform popUpPanel;
        private CanvasGroup canvasGroup;
        public static bool isPartySetting = false;
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

        void Update() 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPartySetting)
                {
                    characterLoader.GetCharPrefab();
                    CreateIcons();
                    Util.ConsumeESC();
                    isPartySetting = false;
                }
            }
        }
        private void LateUpdate() { }
        private void Init()
        {
            charDict = new Dictionary<string, CharactorController>();
            charDataList = Manager.Game.saveFiles[Manager.Game.currentSaveIndex].characterInventory.characters;
            iconList = new List<Image>();
            characterLoader = GetComponent<CharacterSaveLoader>();
            //canvasGroup = GetComponent<CanvasGroup>();
            mainIllustImg = GetUI<Image>("PSCharImg1");
            sub1IllustImg = GetUI<Image>("PSCharImg2");
            sub2IllustImg = GetUI<Image>("PSCharImg3");
            parent = GetUI<RectTransform>("Content");
            //popUpPanel = GetUI<RectTransform>("PartySetPopUp");
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
                foreach(Image icon in iconList)
                {
                    GameObject outIcon = DeleteFromDictionary(icon.gameObject.name,icon.gameObject);
                    Destroy(outIcon);
                }
                iconList.Clear();
                charDict.Clear();
            }

            int imgIndex = 0;
            foreach (CharactorController character in characterLoader.charactorController)
            {
                // ���� 1 �̻��� ��쿡�� �̺�Ʈ ���. �������� ĳ���͵���
                if (character.level >0)
                {
                    Image go;
                    go = Instantiate(iconPrefab, parent);
                    go.name = $"StayCharImg{imgIndex + 1}";
                    // TODO Add Test
                    AddUIToDictionary(go.gameObject);
                    imgIndex++;
                    go.sprite = character.image;
                    GetEvent($"{go.name}").Drag += BeginIconDrag;
                    GetEvent($"{go.name}").Drag += IconDrag;
                    GetEvent($"{go.name}").EndDrag += OnIconDragEnd;
                    iconList.Add(go);
                    if(!charDict.TryAdd(go.name, character))
                    {
                        Debug.LogWarning($"�̹� charDict�� ����{go.name}");
                    }
                }
                //TODO: �������� �����ܰ� �Ϸ���Ʈ�� ������ �־���� ��
                
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
        // ������ �巡�� ����
        private void BeginIconDrag(PointerEventData eventData)
        {

            GameObject dragIcon = GetUI($"{eventData.pointerDrag.gameObject.name}");
            dragIcon.transform.SetParent(transform.root);
            //canvasGroup.blocksRaycasts = false;
            dragIconTransform = dragIcon.GetComponent<RectTransform>();
            originAnchoredPos = dragIconTransform.anchoredPosition;
            isPartySetting = true;
        }

        // �巡�� ��
        private void IconDrag(PointerEventData eventData)
        {
            GetUI($"{eventData.pointerDrag.gameObject.name}").transform.position = eventData.position;
        }

        //�巡�� ��
        private void OnIconDragEnd(PointerEventData eventData)
        {
            GameObject selectedIcon = GetUI($"{eventData.pointerDrag.gameObject.name}");
            if (selectedIcon == null) return;

            //��� ��ġ UI �˻���
            List<RaycastResult> results = new List<RaycastResult>();
            PointerEventData ped = new PointerEventData(EventSystem.current);
            ped.position = eventData.position;
            GraphicRaycaster raycaster = GetComponentInParent<Canvas>().GetComponent<GraphicRaycaster>();
            raycaster.Raycast(ped, results); //ped ��ġ(�巡�׳�����ġ)�� ���̸� �߻��ؼ� ����Ʈ�� ����� ����
            
            // ���� �������� ���� �������� ĳ���͸� ã��
            charDict.TryGetValue($"{selectedIcon.name}", out CharactorController character); // ���⼭ �巡������ �������� ĳ���� ��Ʈ�ѷ� ����
            if (character == null)
            {
                Debug.LogWarning($"�ش� ĳ������Ʈ�ѷ��� ��ųʸ��� ����{selectedIcon.name}");
            }
            // �巡���ϴ� ĳ������ ����
            CharacterSave dragCharData = charDataList.Find(c => c.id == character.id);
            int dragCharDataIndex = charDataList.FindIndex(c => c.id == character.id);

            // ��� ����ĳ��Ʈ ����� ��
            foreach(RaycastResult result in results)
            {
                GameObject targetSlot = result.gameObject;
                // ����, �ش� Ÿ��UI�� �±װ� "��Ƽ����"�̸� �츮�� ã�� UI��.
                if(targetSlot.CompareTag("PartySlot"))
                {
                    Util.ExtractTrailNumber($"{targetSlot.name}",out int slotNum);
                    // UI ������Ʈ�� �̸��� ���� ����, ���긦 �Ǻ�
                    if((int)dragCharData.partySet == slotNum)
                    {
                        ResetDragIcon(selectedIcon);
                        return;
                    }
                    switch(slotNum)
                    {
                        case 1: //����
                            // ����ĳ���� ���� ��������
                            CharacterSave mainCharData = charDataList.Find(c => c.partySet == PartySet.Main);
                            int mainCharIndex = charDataList.FindIndex(c => c.partySet == PartySet.Main);
                            
                            mainCharData.partySet = PartySet.None;
                            Debug.Log($"Main Index : {mainCharIndex}");
                            charDataList[mainCharIndex] = mainCharData;
                            
                            // �巡�� �ϴ� �ָ� �������� �ø�
                            dragCharData.partySet = PartySet.Main;
                            charDataList[dragCharDataIndex] = dragCharData;
                            break;
                        case 2: //����1
                            CharacterSave sub1CharData = charDataList.Find(c => c.partySet == PartySet.Sub1);
                            int sub1CharIndex = charDataList.FindIndex(c => c.partySet == PartySet.Sub1);
                            sub1CharData.partySet = PartySet.None;
                            Debug.Log($"Sub1 Index : {sub1CharIndex}");
                            charDataList[sub1CharIndex] = sub1CharData;

                            dragCharData.partySet = PartySet.Sub1;
                            charDataList[dragCharDataIndex] = dragCharData;
                            break;
                        case 3: //����2
                            CharacterSave sub2CharData = charDataList.Find(c => c.partySet == PartySet.Sub2);
                            int sub2CharIndex = charDataList.FindIndex(c => c.partySet == PartySet.Sub2);
                            sub2CharData.partySet = PartySet.None;
                            Debug.Log($"Sub2 Index : {sub2CharIndex}");
                            charDataList[sub2CharIndex] = sub2CharData;


                            // �巡�� �ϴ� �ָ� ����2�� �ø�
                            dragCharData.partySet = PartySet.Sub2;
                            charDataList[dragCharDataIndex] = dragCharData;
                            break;
                    }
                    // ���⼭ ĳ���� ��Ʈ�ѷ��� �ֽ�ȭ
                    // �ֽ�ȭ�� ĳ���� ��Ʈ�ѷ� �������� �������� UI �ֽ�ȭ

                    //// �̹��� ������Ʈ ���� �õ� �� nullüũ. null�� �ƴϸ�, �ش� UI �̸��� �� ���ڸ� �����´�.
                    //Image image = targetSlot.GetComponent<Image>();
                    //if(image == null)
                    //{
                    //    Debug.LogWarning($"��Ƽ���� ���ӿ�����Ʈ�� �̹��� ������Ʈ�� ����.");
                    //}
                }
                // ��Ƽ���Կ� �������� �������� �ʾҴ�
                else
                {
                    ResetDragIcon(selectedIcon);
                    isPartySetting = false;
                }
            }
            characterLoader.GetCharPrefab();
            CreateIcons();
            // �������� �巡�װ� ���� ������ "��Ƽ ���� �̹���"�� ����
            // if(���� �ָ� �������� ���� ��ġ�� �ǵ���)
            // else //���� �ְ� �ƴϸ�
            // �ش� ��Ƽ ���� �̹����� �������� ĳ���� �Ϸ���Ʈ�� ��ü��
            // ���̺� �����Ϳ� �����ؼ� ��Ƽ ���� ������
        }
        private void ResetDragIcon(GameObject icon)
        {
            icon.transform.SetParent(parent);
            icon.GetComponent<RectTransform>().anchoredPosition = originAnchoredPos;
        }
    }
}