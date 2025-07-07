using KYG_skyPower;
using LJ2;
using System.Collections;
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
        private GameObject invenScroll => GetUI("InvenScroll");
        private TMP_Text invenCharName => GetUI<TMP_Text>("InvenCharNameText");
        private TMP_Text level => GetUI<TMP_Text>("InvenCharLevelText");
        private TMP_Text hp => GetUI<TMP_Text>("InvenCharHPText");
        private TMP_Text ap => GetUI<TMP_Text>("InvenCharAPText");
        private Image charImage;
        private CharacterSaveLoader characterLoader;
        private CharactorController mainController=>characterLoader.mainController;

        private new void Awake()
        {
            base.Awake();
            characterLoader = GetComponent<CharacterSaveLoader>();
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
            //GetEvent("WeaponBtn").Click += OpenWPInven;
            GetEvent("WPEnhanceBtn1").Click += OpenWPEnhance;
            //GetEvent("ArmorBtn").Click += OpenAMInven;
            GetEvent("AMEnhanceBtn2").Click += OpenAMEnhance;
            //GetEvent("AccessoryBtn").Click += OpenACInven;

            // ���� ĳ������ ������ ǥ�õȴ�
            // index�� UIManager�� ����
            // GameManager.Instance.character[index]

        }
        private void Init()
        {
            characterLoader.GetCharPrefab();
            charImage = GetUI<Image>("InvenCharImage");
            charImage.sprite = mainController.image;
            if(mainController.step == 0)
            {
                invenCharName.text = $"{mainController.charName}";
            }
            else
            {
                invenCharName.text = $"{mainController.charName} + {mainController.step}";
            }
            level.text = $"{mainController.level}";
            hp.text = $"{mainController.Hp}";
            ap.text = $"{mainController.attackDamage}";
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

        //private void OpenWPInven(PointerEventData eventData)
        //{
        //    invenScroll.SetActive(true);
        //    foreach (Transform child in invenScroll.transform)
        //        //Destroy(child.gameObject);

            // ���� ����Ʈ �ҷ�����
            //var weaponList = EquipmentInvenManager.Instance.GetItemList("weapon");
            //Debug.Log("���� ����: " + weaponList.Count);
            //GameObject itemSlotPrefab = Resources.Load<GameObject>("Inventory/WeaponSlot");

            //int idx = 0;
            //foreach (var weapon in weaponList)
            //{
            //    //Debug.Log($"���� ����: {weapon.Equip_Name} ({idx++})");
            //    //GameObject slot = Instantiate(itemSlotPrefab, invenScroll.transform);
            //    //slot.GetComponentInChildren<TMP_Text>().text = weapon.Equip_Name;
            //    //slot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Equipment/" + weapon.Equip_Img);

            //    //var capturedWeapon = weapon;
            //    //slot.GetComponent<Button>().onClick.AddListener(() =>
            //    //{
            //    //    // ���� ĳ���Ϳ� ��� ����
            //    //    EquipmentInvenManager.Instance.EquipItem(mainController.id, capturedWeapon);
            //    //    mainController.ApplyEquipmentStat(); // (�� �Լ��� CharactorController�� �߰� �ʿ�)
            //    //});
            //}
        //}
        
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

//        private void OpenAMInven(PointerEventData eventData)
//        {
//            invenScroll.SetActive(true);
//            foreach (Transform child in invenScroll.transform)
//                Destroy(child.gameObject);

//            var armorList = EquipmentInvenManager.Instance.GetItemList("armor");
//            GameObject itemSlotPrefab = Resources.Load<GameObject>("Inventory/ArmorSlot");

//            foreach (var armor in armorList)
//            {
//                GameObject slot = Instantiate(itemSlotPrefab, invenScroll.transform); 
//                //slot.GetComponentInChildren<TMP_Text>().text = armor.Equip_Name; 
//                //slot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Equipment/" + armor.Equip_Img); 

//                var capturedArmor = armor; 
//                slot.GetComponent<Button>().onClick.AddListener(() =>
//                {
//                    EquipmentInvenManager.Instance.EquipItem(mainController.id, capturedArmor);
//                   // mainController.ApplyEquipmentStat();
//                });
//            }
//        }
//        private void OpenACInven(PointerEventData eventData)
//        {
//            invenScroll.SetActive(true);
//            foreach (Transform child in invenScroll.transform)
//                Destroy(child.gameObject);

//            var accessoryList = EquipmentInvenManager.Instance.GetItemList("accessory"); 
//            GameObject itemSlotPrefab = Resources.Load<GameObject>("Inventory/AccessorySlot"); 

//            foreach (var accessory in accessoryList)
//            {
//                GameObject slot = Instantiate(itemSlotPrefab, invenScroll.transform);
//                //slot.GetComponentInChildren<TMP_Text>().text = accessory.Equip_Name;
//                //slot.GetComponentInChildren<Image>().sprite = Resources.Load<Sprite>("Sprites/Equipment/" + accessory.Equip_Img);

//                var capturedAccessory = accessory;
//                slot.GetComponent<Button>().onClick.AddListener(() =>
//                {
//                    EquipmentInvenManager.Instance.EquipItem(mainController.id, capturedAccessory);
//                   // mainController.ApplyEquipmentStat();
//                });
//            }
//        }
    }
}


