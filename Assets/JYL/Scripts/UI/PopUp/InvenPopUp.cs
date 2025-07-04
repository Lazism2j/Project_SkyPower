using KYG_skyPower;
using LJ2;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using KYG_skyPower; // EquipmentInvenManager, EquipmentDataSO, CharacterEquipmentSlots
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
        private CharactorController selectedChar;
        // TODO: GameManager.CharacterController[] character => for(int i = 0;i<character.Length;i++) { �κ��丮�� UI�߰� }

        // private Item[] items;

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
        private void Init()
        {
            charImage = GetUI<Image>("InvenCharImage");
            CharactorController charCont;
            foreach (var cont in characterLoader.charactorController)
            {
                if (cont.partySet == PartySet.Main)
                {
                    selectedChar = cont;
                    charImage.sprite = cont.image;
                    // ...������ ���� ǥ��
                }
            }
        }

        private void OpenWPInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
            foreach (Transform child in invenScroll.transform)
                Destroy(child.gameObject);

            // ���� ����Ʈ �ҷ�����
            var weaponList = EquipmentInvenManager.Instance.GetItemList("weapon");
            Debug.Log("���� ����: " + weaponList.Count);
            GameObject itemSlotPrefab = Resources.Load<GameObject>("Inventory/WeaponSlot");

            int idx = 0;
            foreach (var weapon in weaponList)
            {
                Debug.Log($"���� ����: {weapon.Equip_Name} ({idx++})");
                GameObject slot = Instantiate(itemSlotPrefab, invenScroll.transform);
                slot.GetComponentInChildren<TMP_Text>().text = weapon.Equip_Name;
                slot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Equipment/" + weapon.Equip_Img);

                var capturedWeapon = weapon;
                slot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    // ���� ĳ���Ϳ� ��� ����
                    EquipmentInvenManager.Instance.EquipItem(selectedChar.id, capturedWeapon);
                    selectedChar.ApplyEquipmentStat(); // (�� �Լ��� CharactorController�� �߰� �ʿ�)
                });
            }
        }
        
            // �κ��丮 �Ŵ������� ����� Ÿ�� �����ؼ� ������ ������
            // items = InvenManager.Instance.GetItemList(Type weapon)
            // items�� ������ invenscroll ����
            // �� UI ��ҵ鿡 �̺�Ʈ �޾ƾ� ��
            // foreach(Button ui in items)
            // { GetEvent("").Click += �����۱�ü �Լ� }
            // �ش� UI Ŭ�� ��, �������̴� ���� ��ȯ
            // InvenManager.Instance.Add()
            // InvenManager.Instance.RemoveAt(�κ�ID������ ã�� �����)
        

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
            foreach (Transform child in invenScroll.transform)
                Destroy(child.gameObject);

            var armorList = EquipmentInvenManager.Instance.GetItemList("armor");
            GameObject itemSlotPrefab = Resources.Load<GameObject>("Inventory/ArmorSlot");

            foreach (var armor in armorList)
            {
                GameObject slot = Instantiate(itemSlotPrefab, invenScroll.transform); 
                slot.GetComponentInChildren<TMP_Text>().text = armor.Equip_Name; 
                slot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Equipment/" + armor.Equip_Img); 

                var capturedArmor = armor; 
                slot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EquipmentInvenManager.Instance.EquipItem(selectedChar.id, capturedArmor);
                    selectedChar.ApplyEquipmentStat();
                });
            }
        }
        

        private void OpenAMEnhance(PointerEventData eventData)
        {
            UIManager.Instance.ShowPopUp<EnhancePopUp>();
        }
        private void OpenACInven(PointerEventData eventData)
        {
            invenScroll.SetActive(true);
            foreach (Transform child in invenScroll.transform)
                Destroy(child.gameObject);

            var accessoryList = EquipmentInvenManager.Instance.GetItemList("accessory"); 
            GameObject itemSlotPrefab = Resources.Load<GameObject>("Inventory/AccessorySlot"); 

            foreach (var accessory in accessoryList)
            {
                GameObject slot = Instantiate(itemSlotPrefab, invenScroll.transform);
                slot.GetComponentInChildren<TMP_Text>().text = accessory.Equip_Name;
                slot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Equipment/" + accessory.Equip_Img);

                var capturedAccessory = accessory;
                slot.GetComponent<Button>().onClick.AddListener(() =>
                {
                    EquipmentInvenManager.Instance.EquipItem(selectedChar.id, capturedAccessory);
                    selectedChar.ApplyEquipmentStat();
                });
            }
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


