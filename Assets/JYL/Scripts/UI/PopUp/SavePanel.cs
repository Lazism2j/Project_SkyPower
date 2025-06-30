using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.EventSystems;
namespace JYL
{
    public class SavePanel : BaseUI
    {
        private void OnEnable()
        {
            // ���ӸŴ������� ���̺� ������ Init��
            // GameManager.Instance.SaveInit();
        }
        void Start()
        {
            GetEvent("SaveBtn1").Click += OnSaveSlotBtnClick;
            GetEvent("SaveBtn2").Click += OnSaveSlotBtnClick;
            GetEvent("SaveBtn3").Click += OnSaveSlotBtnClick;
        }

        private void OnSaveSlotBtnClick(PointerEventData eventData)
        {
            // TODO : ���̺� �Ŵ������� �Լ� �ϼ� �� ����
            // string SaveManager.Instance.CheckSave()
            string name = eventData.pointerClick.name;
            char lastChar = name[name.Length - 1];
            Util.ExtractTrailNumber(name, out int index);
            // GameManager.saveIndex = index-1;

            // TODO: UI Ʈ������ �׽�Ʈ
            switch(index)
            {
                case 1:
            UIManager.Instance.ShowPopUp<SaveFilePanel>();

                    break;
                case 2:
            UIManager.Instance.ShowPopUp<SaveCreatePanel>();
                    break;
                case 3:
            UIManager.Instance.ShowPopUp<SaveCreatePanel>();
                    break;
            }
        }
        // ���⼭ ���̺����� ������ �ҷ��;� ��
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
            UIManager.Instance.ShowPopUp<SavePanel>();
            // fade in
        }
    }
}

