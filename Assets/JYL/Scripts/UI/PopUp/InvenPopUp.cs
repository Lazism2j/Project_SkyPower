using KYG_skyPower;
using LJ2;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace JYL
{
    public class InvenPopUp : BaseUI
    {
        // ���ӸŴ������� ĳ���� ������ �ҷ��;� ��
        private string iconPrefabPath = "JYL/UI/ItemIconPrefab";
        private GameObject iconPrefab;
        private GameObject invenScroll => GetUI("InvenScroll");
        private TMP_Text invenCharName => GetUI<TMP_Text>("InvenCharNameText");
        private TMP_Text level => GetUI<TMP_Text>("InvenCharLevelText");
        private TMP_Text hp => GetUI<TMP_Text>("InvenCharHPText");
        private TMP_Text ap => GetUI<TMP_Text>("InvenCharAPText");
        private TMP_Text gold => GetUI<TMP_Text>("InvenUnitText");
        private RawImage apIcon => GetUI<RawImage>("WeaponBtn");
        private RawImage amIcon => GetUI<RawImage>("ArmorBtn");
        private RawImage acIcon => GetUI<RawImage>("AccessoryBtn");
        private RectTransform parent => GetUI<RectTransform>("Content");
        private Image charImage;
        private CharacterSaveLoader characterLoader;
        private CharactorController mainController => characterLoader.mainController;
        private EquipController equipController;
        public static bool isInvenOpened = false;

        private List<GameObject> iconList;
        private Dictionary<GameObject, int> equipIdDict;
        private new void Awake()
        {
            base.Awake();
            characterLoader = GetComponent<CharacterSaveLoader>();
            equipController = GetComponent<EquipController>();

            Init();

        }
        private void OnEnable()
        {
            Init();
        }
        void Start()
        {

            // ��� Ŭ���� Ȱ��ȭ

            invenScroll.SetActive(false);
            GetEvent("CharEnhanceBtn").Click += OpenCharEnhance;
            GetEvent("WeaponBtn").Click += OpenWPInven;
            GetEvent("WPEnhanceBtn1").Click += OpenWPEnhance;
            GetEvent("ArmorBtn").Click += OpenAMInven;
            GetEvent("AMEnhanceBtn2").Click += OpenAMEnhance;
            GetEvent("AccessoryBtn").Click += OpenACInven;
            // ���� ĳ������ ������ ǥ�õȴ�
            // index�� UIManager�� ����
            // GameManager.Instance.character[index]
            gameObject.GetComponent<PointerHandler>().Click += CloseInvenScroll;

            iconPrefab = Resources.Load<GameObject>(iconPrefabPath);

        }
        private void LateUpdate()
        {
            if (isInvenOpened)
            {
                Init();
                isInvenOpened = false;
            }
        }
        private void Init()
        {
            iconList = new List<GameObject>();
            equipIdDict = new Dictionary<GameObject, int>();
            equipController.Init();
            characterLoader.GetCharPrefab();
            charImage = GetUI<Image>("InvenCharImage");
            charImage.sprite = mainController.image;
            if (mainController.step == 0)
            {
                invenCharName.text = $"{mainController.charName}";
            }
            else
            {
                invenCharName.text = $"{mainController.charName} + {mainController.step}";
            }
            UpdateCharacterInfo();
            CreateEquipedIcons();
        }
        private void UpdateCharacterInfo()
        {
            level.text = $"{mainController.level}";
            hp.text = $"{mainController.Hp}";
            ap.text = $"{mainController.attackDamage}";
            gold.text = $"{Manager.Game.CurrentSave.gold}";
        }
        private void CreateIcons(EquipType type)
        {

            // ���� ����Ʈ Ŭ���� �� �ٽ� ���
            if (iconList.Count > 0)
            {
                foreach (GameObject icon in iconList)
                {
                    GameObject outIcon = DeleteFromDictionary(icon.gameObject.name, icon.gameObject);
                    Destroy(icon);
                    Debug.Log("������ �ı���");
                }
                iconList.Clear();
            }

            List<EquipInfo> equipList = new List<EquipInfo>();
            equipList = equipController.GetEquipListByType(type);
            int index = 0;
            foreach (EquipInfo tmp in equipList)
            {
                if (tmp.level > 0)
                {
                    GameObject iconObj = Instantiate(iconPrefab, parent);
                    iconObj.name = $"ItemIconPrefab{index + 1}";
                    AddUIToDictionary(iconObj.gameObject);
                    Image iconImg = iconObj.GetComponentInChildren<Image>();
                    iconImg.sprite = tmp.icon;
                    
                    if(tmp.id == Manager.Game.CurrentSave.wearingId[(int)type]&&tmp.id != 0) // ���� �� ���� ������!
                    {
                        Color color = iconImg.color;
                        color = Color.Lerp(color, Color.black, 0.3f);
                        iconImg.color = color;
                        Debug.Log($"�̰� Ȱ��ȭ �Ǵ���{tmp.name}");
                        iconObj.GetComponentInChildren<TMP_Text>(true).gameObject.SetActive(true);
                        iconList.Insert(0, iconObj); // �������� ���� �� �տ� ���� �Ѵ�
                        iconObj.GetOrAddComponent<PointerHandler>().Click += ClickEquipIcon;
                        if (!equipIdDict.TryAdd(iconObj, tmp.id))
                        {
                            Debug.LogWarning($"�̹� ��� ��ųʸ��� ���� : GameObject({iconObj.name}) ID({tmp.id}) ");
                        }
                    }
                    else//���������� ���� ����
                    {
                        Debug.Log("�������� �ƴ�!");
                        iconObj.GetOrAddComponent<PointerHandler>().Click += ClickEquipIcon;
                        iconList.Add(iconObj);
                        if (!equipIdDict.TryAdd(iconObj, tmp.id))
                        {
                            Debug.LogWarning($"�̹� ��� ��ųʸ��� ���� : GameObject({iconObj.name}) ID({tmp.id}) ");
                        }
                    }
                }
                index++;
            }
        }

        private void CreateEquipedIcons()
        {
            Color tmpColor = Color.white;
            Color tmpColorOrigin = Color.white;
            tmpColorOrigin.a = 1f;
            tmpColor.a = 0f;
            if (equipController.weapon.id == 0) // ���� ���� �ȵǾ� ����
            {
                Debug.Log($"���������� ����. �̹��� ����{equipController.weapon.id}");
                apIcon.gameObject.GetComponentInChildren<Image>().color = tmpColor;
            }
            else
            {
                Debug.Log($"������! �̹��� �߰�{equipController.weapon.id}");
                Image tmpImg = apIcon.gameObject.GetComponentInChildren<Image>();
                tmpImg.color = tmpColorOrigin;
                tmpImg.sprite = equipController.weapon.icon;
            }

            if (equipController.armor.id == 0) // �� ���� �ȵǾ� ����
            {
                amIcon.gameObject.GetComponentInChildren<Image>().color = tmpColor;
            }
            else
            {
                Image tmpImg = amIcon.gameObject.GetComponentInChildren<Image>();
                tmpImg.color = tmpColorOrigin;
                tmpImg.sprite = equipController.armor.icon;
            }

            if (equipController.accessory.id == 0) // �Ǽ��縮 ���� �ȵǾ� ����
            {
                acIcon.gameObject.GetComponentInChildren<Image>().color = tmpColor;
            }
            else
            {
                Image tmpImg = acIcon.gameObject.GetComponentInChildren<Image>();
                tmpImg.color = tmpColorOrigin;
                tmpImg.sprite = equipController.accessory.icon;
            }

        }
        private void OpenWPInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
            CreateIcons(EquipType.Weapon);
        }
        private void OpenAMInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
            CreateIcons(EquipType.Armor);
        }
        private void OpenACInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
            CreateIcons(EquipType.Accessory);
        }

        private void CloseInvenScroll(PointerEventData eventData)
        {
            List<RaycastResult> result = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, result);

            foreach (RaycastResult rs in result)
            {
                if (rs.gameObject == null) continue;

                if (rs.gameObject.transform.IsChildOf(invenScroll.transform))
                {
                    // �˾� ���� Ŭ��: ����
                    return;
                }
            }

            invenScroll.SetActive(false);
        }

        private void ClickEquipIcon(PointerEventData eventData) // ��� ����
        {
            if(!equipIdDict.TryGetValue(eventData.pointerClick, out int equipId))
            {
                Debug.LogWarning($"��� ��ųʸ��� �ش� �������� ����{eventData.pointerClick.name}");
            }
            Debug.Log("Ŭ����");
            EquipInfo equipInfoTemp = equipController.FindEquip(equipId);
            ReplaceEquipment(equipInfoTemp.type, equipInfoTemp);
            equipController.UpdateWearing();
            characterLoader.GetCharPrefab();
            CreateEquipedIcons();
            CreateIcons(equipInfoTemp.type);
            UpdateCharacterInfo();
        }
        private void ReplaceEquipment(EquipType type, EquipInfo equipInfo)
        {
            
            switch (type) // 1.���� 2.�� 3.�Ǽ��縮
            {
                case EquipType.Weapon:
                    if(Manager.Game.CurrentSave.wearingId[0] != equipInfo.id)
                    {
                        Manager.Game.CurrentSave.wearingId[0] = equipInfo.id; //���� ���� ��ü
                    }
                    else
                    {
                        Manager.Game.CurrentSave.wearingId[0] = 0; // ��� ����
                    }
                    break;
                case EquipType.Armor:
                    if(Manager.Game.CurrentSave.wearingId[1] != equipInfo.id)
                    {
                        Manager.Game.CurrentSave.wearingId[1] = equipInfo.id;
                    }
                    else
                    {
                        Manager.Game.CurrentSave.wearingId[1] = 0;
                    }
                    break;
                case EquipType.Accessory:
                    if(Manager.Game.CurrentSave.wearingId[2] != equipInfo.id)
                    {
                        Manager.Game.CurrentSave.wearingId[2] = equipInfo.id;
                    }
                    else
                    {
                        Manager.Game.CurrentSave.wearingId[2] = 0;
                    }
                    break;
            }
        }

        private void OpenCharEnhance(PointerEventData eventData)
        {
            // ĳ���� ������ ������ ��ȭâ ����
            // UIManager���� ���õ� ĳ������ �ε��� ������ GameManager�� ��Ƽ �������� ������ ���� ĳ���� ��Ʈ�ѷ� ���� �ҷ���
            // �ش� ������ ��ȭâ���� �ҷ��� ���⼭ �Ⱥҷ���
            UIManager.selectIndexUI = 1;
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
            // UI ������ ��, UI���ٰ� �̺�Ʈ �ټ���.
            // Image img = Instantiate();
            // GetEvent($"img.gameObject.name").Click += �̺�Ʈ�Լ�;
        }

        private void OpenWPEnhance(PointerEventData eventData)
        {
            UIManager.selectIndexUI = 2;
            // ���� ������ ������ ����������
            // �����ϴ� UI �������� UIManager�� ���� �����Ѵ�.
            // GameManager.Instance.Party[0].
            // UIManager.Instance. ���� ������ ĳ�������� + ���� -> Enhance �˾��� �ҷ��;� ��
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }

        private void OpenAMEnhance(PointerEventData eventData)
        {
            UIManager.selectIndexUI = 3;
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }

    }
}


