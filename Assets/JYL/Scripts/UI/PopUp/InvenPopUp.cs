using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System.Linq;
using KYG_skyPower;

namespace JYL
{
    public class InvenPopUp : BaseUI
    {
        // ���ӸŴ������� ĳ���� ������ �ҷ��;� ��

        [SerializeField] private GameObject equipmentSlotPrefab; // ���� ������

        private GameObject invenScroll => GetUI("InvenScroll");
        private Transform invenContent => invenScroll.transform.Find("Viewport/Content"); // ���� �θ� Ʈ������
        private TMP_Text invenCharName => GetUI<TMP_Text>("InvenCharNameText");
        private TMP_Text level => GetUI<TMP_Text>("InvenCharLevelText");
        private TMP_Text hp => GetUI<TMP_Text>("InvenCharHPText");
        private TMP_Text ap => GetUI<TMP_Text>("InvenCharAPText");
        // TODO: GameManager.CharacterController[] character => for(int i = 0;i<character.Length;i++) { �κ��丮�� UI�߰� }

        // private Item[] items;

        
        private EquipmentType _lastOpenedType; // ������ ���� ��� ���� (UI ���� ��)

        void Start()
        {
            // ��� Ŭ���� Ȱ��ȭ
            invenScroll.SetActive(false);

            GetEvent("WeaponBtn").Click += (e) => OpenInven(EquipmentType.Weapon);
            GetEvent("ArmorBtn").Click += (e) => OpenInven(EquipmentType.Armor);
            GetEvent("AccessoryBtn").Click += (e) => OpenInven(EquipmentType.Accessory);

            GetEvent("WeaponBtn").Click += OpenWPInven;
            GetEvent("WPEnhanceBtn1").Click += OpenWPEnhance;
            //GetEvent("ArmorBtn").Click += OpenAMInven;
            GetEvent("AMEnhanceBtn2").Click += OpenAMEnhance;
            //GetEvent("AccessoryBtn").Click += OpenACInven;
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


        private void OpenInven(EquipmentType type)
        {
            invenScroll.SetActive(true);
            _lastOpenedType = type;

            // ���� ���� ����
            foreach (Transform child in invenContent)
                Destroy(child.gameObject);

            // ��񸮽�Ʈ (SO+Save ���� �� ���� ���)
            var equipList = GameManager.Instance.equipmentManagerSO.runtimeEquipments
                .Where(e => e.SO.Equip_Type == type)
                .OrderByDescending(e => e.Save.isEquipped) // ������ �켱
                .ThenByDescending(e => e.Save.level)       // ������ ����
                .ToList();

            foreach (var eq in equipList)
            {
                var go = Instantiate(equipmentSlotPrefab, invenContent);
                var slot = go.GetComponent<EquipmentSlotUI>();
                // ���� ������ ����: (SO, Save, ��������, �ݹ�)
                slot.Set(eq.SO, eq.Save, eq.Save.isEquipped, () => OnClickEquip(eq.Save.equipId, type));
            }
        }

        private void OnClickEquip(int equipId, EquipmentType type)
        {
            // ����: ���� ���� ĳ����ID (������ ��Ƽ or ���� �ε����� ���� ����)
            int currentCharId = 0;
            GameManager.Instance.EquipToCharacter(equipId, currentCharId);

            // TODO: ĳ���� �ɷ�ġ/��� UI ���� �� �ܺο� event�� ����

            // UI ����
            OpenInven(type);
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


