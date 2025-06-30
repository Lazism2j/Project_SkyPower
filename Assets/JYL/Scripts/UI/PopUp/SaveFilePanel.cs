using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace JYL
{
    public class SaveFilePanel : BaseUI
    {
        // �ҷ��Դ� ���̺� ������ �������� �����͸� ä���
        void Start()
        {
            //GetUI<TMP_Text>("SaveFileData").text = $"{GameManager.Instance.selectSaveFile.name";
            GetUI<TMP_Text>("SaveFileData").text = $"SkyPower1";
            GetEvent("SaveDelBtn").Click += OnDelClick;
            GetEvent("SaveStartBtn").Click += OnStartClick;
        }

        private void OnDelClick(PointerEventData eventData)
        {
            // ���̺� �Ŵ����� ���� ���̺����� ����
            //SaveManager.PlayerDelete(index);
            // �ؽ�Ʈ ������� ���������� �˸�.
            UIManager.Instance.ClosePopUp();
        }
        private void OnStartClick(PointerEventData eventData)
        {
            //�� �Ѿ -> mainScene
            SceneManager.LoadSceneAsync("bMainScene_JYL");
        }
    
    }
}
