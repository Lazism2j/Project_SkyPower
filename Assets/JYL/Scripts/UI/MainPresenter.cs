using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using LJ2;

namespace JYL
{
    public class MainPresenter : BaseUI
    {
        private GameObject mainScreen;
        private Image charImg1;
        private Image charImg2;
        private Image charImg3;
        private event Action onEnterMain;
        private CharacterSaveLoader characterLoader;
        void Start()
        {
            
            characterLoader = GetComponent<CharacterSaveLoader>();
            characterLoader.GetCharPrefab();

            mainScreen = GetUI("MainScreen");
            charImg1 = GetUI<Image>("CharImage1");
            charImg2 = GetUI<Image>("CharImage2");
            charImg3 = GetUI<Image>("CharImage3");
            SetPartyImage();

            GetEvent("ShopBtn").Click += OpenShop;
            GetEvent("PartySetBtn").Click += OpenPartySetting;
            GetEvent("PlayBtn").Click += OpenGameMode;

            // GetEvent("InfoBtn").Click += OpenGameInfo;
        }
        private void LateUpdate()
        {
            CheckPopUp();
        }
        private void OpenShop(PointerEventData eventData)
        {
            // TODO : ���� ����
            // GameSceneManager.Instance.SceneChange("Shop");
            SceneManager.LoadSceneAsync("cStoreScene_JYL");
        }
        private void OpenPartySetting(PointerEventData eventData)
        {
            UIManager.Instance.ShowPopUp<PartySetPopUp>();
        }
        private void OpenGameMode(PointerEventData eventData)
        {
            UIManager.Instance.ShowPopUp<GameModePopUp>();
        }
        //private void OpenGameInfo(PointerEventData eventData)
        //{
        //    // TODO : �ļ��� ���� ����
        //}
        private void SetPartyImage()
        {
            foreach (CharactorController character in characterLoader.charactorController)
            {
                switch (character.partySet)
                {
                    case PartySet.Main:
                        charImg1.sprite = character.image;
                        break;
                    case PartySet.Sub1:
                        charImg2.sprite = character.image;
                        break;
                    case PartySet.Sub2:
                        charImg3.sprite = character.image;
                        break;
                }
            }
        }
        private void CheckPopUp()
        {
            if (PopUpUI.IsPopUpActive && onEnterMain == null)
            {
                onEnterMain += characterLoader.GetCharPrefab;
                onEnterMain += SetPartyImage;
            }
            else if (!PopUpUI.IsPopUpActive)
            {
                onEnterMain?.Invoke();
                if (onEnterMain != null)
                {
                    onEnterMain -= characterLoader.GetCharPrefab;
                    onEnterMain -= SetPartyImage;
                }
            }
        }
    }
}

