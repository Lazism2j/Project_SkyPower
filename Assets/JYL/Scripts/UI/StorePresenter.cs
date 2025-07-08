using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using KYG_skyPower;
using TMPro;
using LJ2;
using System;
using System.Collections;
using System.Collections.Generic;


namespace JYL
{
    public class StorePresenter : BaseUI
    {

        private TMP_Text unitText => GetUI<TMP_Text>("StoreUnitText");
        private CharacterSaveLoader characterLoader;
        private EquipController equipController;
        void Start()
        {

            GetEvent("GachaChrBtn1").Click += CharGachaClick;
            GetEvent("GachaChrBtn5").Click += CharGachaClick;
            GetEvent("GachaEquipBtn1").Click += EquipGachaClick;
            GetEvent("GachaEquipBtn5").Click += EquipGachaClick;
            //GetEvent("StoreItemImg").Click += ItemStore;
            characterLoader = gameObject.GetOrAddComponent<CharacterSaveLoader>();
            equipController = gameObject.GetOrAddComponent<EquipController>();
            characterLoader.GetCharPrefab(); // ��Ţ��Ʈ�ѷ��� ���� �ʱ�ȭ �ȴ�.
        }
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !PopUpUI.IsPopUpActive && !Util.escPressed)
            {
                // �� ��ȯ
                SceneManager.LoadSceneAsync("bMainScene_JYL");
                Util.ConsumeESC();
            }
        }

        private void CharGachaClick(PointerEventData eventData)
        {
            // TODO : ��í ����� ���ÿ�, �κ��丮(ĳ���� ���)�� �߰�. ���ӸŴ������� ���̺���
            // ���� �Ϸ� ��, ��í ���� ����. ���� �Ϸ��� ��� �˾� â�� ���
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int num);
            switch (num)
            {
                case 1:
                    if (Manager.Game.CurrentSave.gold >= 100)
                    {
                        Manager.Game.CurrentSave.gold -= 100;
                        
                        
                        Manager.Game.SaveGameProgress();
                        UIManager.Instance.ShowPopUp<GachaPopUp>();
                    }
                    break;
                case 5:
                    if(Manager.Game.CurrentSave.gold >= 500)
                    {
                        Manager.Game.CurrentSave.gold -= 500;
                        
                        
                        Manager.Game.SaveGameProgress();
                        UIManager.Instance.ShowPopUp<Gacha5PopUp>();
                    }
                    break;
            }
        }
        private void EquipGachaClick(PointerEventData eventData)
        {
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int num);
            switch (num)
            {
                case 1:
                    if (Manager.Game.CurrentSave.gold >= 100)
                    {
                        Manager.Game.CurrentSave.gold -= 100;


                        Manager.Game.SaveGameProgress();
                        UIManager.Instance.ShowPopUp<GachaPopUp>();
                    }
                    break;
                case 5:
                    if (Manager.Game.CurrentSave.gold >= 500)
                    {
                        Manager.Game.CurrentSave.gold -= 500;


                        Manager.Game.SaveGameProgress();
                        UIManager.Instance.ShowPopUp<Gacha5PopUp>();
                    }
                    break;
            }
        }
        private List<CharactorController> CharGachaRoll(int num)
        {
            List<CharactorController> results = new List<CharactorController>(num);
            return null;
        }
        private List<EquipInfo> EquipGachaRoll(int num)
        {
            List<EquipInfo> results = new List<EquipInfo>(num);
            return null;

        }
        //// TODO : ������ �߰� �� �۾�
        //private void ItemStore(PointerEventData eventData)
        //{

        //}
    }
}


