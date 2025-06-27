using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.Utilities;
using UnityEngine.EventSystems;
using TMPro;
using System.IO;
using UnityEngine;
using IO;

namespace JYL
{
    public class TitlePresenter : BaseUI
    {
        private GameObject titleImage;
        private GameObject saveImage;
        private GameObject savePanel;
        private GameObject saveFilePanel;
        private GameObject saveCreatePanel;

        void OnEnable() => Init();
        
        private void Update()
        {

        }
        void OnDisable()
        {
            GetEvent("PressKeyBack").Click -= OnAnyBtnClick;
            GetEvent("SaveBtn1").Click -= OnSaveSlotBtnClick;
            GetEvent("SaveBtn2").Click -= OnSaveSlotBtnClick;
            GetEvent("SaveBtn3").Click -= OnSaveSlotBtnClick;
        }

        private void Init()
        {
            for(int i = 0;i<3;i++)
            {
                //GetUI<TMP_Text>($"SaveText{i+1}").text = Manager.Game.saveFile[i].name;
            }
            titleImage = GetUI("TitleImage");
            saveImage = GetUI("SaveImage");
            savePanel = GetUI("SavePanel");
            saveFilePanel = GetUI("SaveFilePanel");
            saveCreatePanel = GetUI("SaveCreatePanel");

            InputSystem.onAnyButtonPress.Call(ctrl => OnAnyBtnPress(ctrl));

            GetEvent("PressKeyBack").Click += OnAnyBtnClick;
            GetEvent("SaveBtn1").Click += OnSaveSlotBtnClick;
            GetEvent("SaveBtn2").Click += OnSaveSlotBtnClick;
            GetEvent("SaveBtn3").Click += OnSaveSlotBtnClick;
        }


        private void OnAnyBtnPress(InputControl control)
        {
            // ��ư�� �ش��ϸ鼭 Ű���� �Ǵ� �����е� �Ǵ� ���콺 ���ʹ�ư �Է��� ���
            if (control is ButtonControl && (control.device is Keyboard || control.device is Gamepad gamepad))
            {
                SavePanelPop();
            }
        }
        private void OnAnyBtnClick(PointerEventData eventData)
        {
            if(eventData.button == PointerEventData.InputButton.Left)
            {
                SavePanelPop();
            }
            
        }
        private void OnSaveSlotBtnClick(PointerEventData eventData)
        {
            // TODO : ���̺� �Ŵ������� �Լ� �ϼ� �� ����
            // string SaveManager.Instance.CheckSave() fd
            string name = eventData.pointerClick.name;
            char lastChar = name[name.Length - 1];
            Util.ExtractTrailNumber(name, out int index);
            // GameManager.saveIndex = index-1;

            // TODO: UI Ʈ������ �׽�Ʈ
            if (index == 0)
            {
                savePanel.SetActive(false);
                saveFilePanel.SetActive(true);
            }

        }
        private void SavePanelPop()
        {

            // ���ӸŴ��� ����
            // CharacterSave[] saveFile = new CharacterSave[3]; - 3���� �迭�� ����
            // Manager.Save.PlayerLoad(CharacterSave[index],index+1); - ���ӸŴ������� 3���� ���� �ε� �õ�
            // int saveIndex = 0~2; - ������ ���̺� ���� �ε���
            // saveFile[saveIndex] - ���� ������� ���̺�����

            // UI Ŭ��
            // ��� ǥ�� �Ҷ� != null -> saveFile[index].name
            // ==null -> empty


            //TODO : Ʈ������ ȿ��(ex.fade out)
            titleImage.SetActive(false);
            saveImage.SetActive(true);
            // fade in
        }
    }
}
