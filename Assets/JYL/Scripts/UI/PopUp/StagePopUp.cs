using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

namespace JYL
{
    public class StagePopUp : BaseUI
    {
        private TMP_Text stageText => GetUI<TMP_Text>("StageNameText");
        void Start()
        {
            // ���⼭ ���������Ŵ������� ���� ���������� ������ �޾ƿ´�.
            // StageManager.Instance.curStage.name
            stageText.text = $"STAGE 1-1";
            GetEvent("StageReBack").Click += RestartStage;
            GetEvent("StageQuitBack").Click += QuitStage;
        }

        void Update()
        {

        }
        private void RestartStage(PointerEventData eventData)
        {
            // �������� �Ŵ����� ����ŸƮ ����� Ȱ����
            // StageManager.Instance.restart();
            SceneManager.LoadSceneAsync("dStageScene_JYL");
        }
        private void QuitStage(PointerEventData eventData)
        {
            // �������� �Ǵ� �� �Ŵ����� ����� ���� ����. ���� ������ ���ư���
            // StageManager.Instance.SceneChange();
            SceneManager.LoadSceneAsync("bMainScene_JYL");
        }
    }
}

