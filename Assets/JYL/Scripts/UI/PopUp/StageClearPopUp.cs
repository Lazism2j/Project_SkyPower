using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using KYG_skyPower;

namespace JYL
{
    public class StageClearPopUp : BaseUI
    {
        private Button nextButton;
        // TODO : �ش� �˾��� ���� Ŭ���� �������� �˾��ȴ�
        private void Start()
        {
            SetNextStageBtn();
            GetEvent("SCReBtn").Click += RestartStage;
            GetEvent("SCQuitBtn").Click += QuitStage;
            Manager.Score.RecordBestScore();
        }
        private void OnEnable()
        {
            UIManager.canClosePopUp = false;
        }
        private void OnDisable()
        {
            UIManager.canClosePopUp = true;
        }

        // TODO : �׽�Ʈ �ʿ�
        private void SetNextStageBtn() // ��ư Ȱ��ȭ ����.  ���� �������� ������ ���� ���, ��Ȱ��ȭ
        {
            nextButton = GetUI<Button>("SCNextStageBtn");
            int worldIndex = Manager.Game.selectWorldIndex;
            int stageIndex = Manager.Game.selectStageIndex;
            if (stageIndex > 5)
            {
                worldIndex++;
                stageIndex = 1;
            }
            if (Manager.SDM.runtimeData[worldIndex-1].subStages[stageIndex-1] == null)
            {
                nextButton.interactable = false;
            }
            else
            {
                GetEvent("SCNextStageBtn").Click += NextStage;
            }
        }
        private void NextStage(PointerEventData eventData)
        {
            Time.timeScale = 1.0f;
            Manager.Game.selectStageIndex++;
            if(Manager.Game.selectStageIndex>5)
            {
                Manager.Game.selectWorldIndex++;
                Manager.Game.selectStageIndex = 1;
            }
            UIManager.Instance.CleanPopUp();
            Manager.Score.ResetScore();
            Manager.Game.ResetState();
            Manager.GSM.LoadGameSceneWithStage("dStageScene_JYL", Manager.Game.selectWorldIndex, Manager.Game.selectStageIndex);
        }
        private void RestartStage(PointerEventData eventData)
        {
            Time.timeScale = 1.0f;
            UIManager.Instance.CleanPopUp();
            Manager.Score.RecordBestScore();
            Manager.Game.ResetState();
            Manager.GSM.ReloadCurrentStage();
        }
        private void QuitStage(PointerEventData eventData)
        {
            Time.timeScale = 1.0f;
            UIManager.Instance.CleanPopUp();
            Manager.Score.RecordBestScore();
            Manager.Game.ResetState();
            Manager.GSM.LoadScene("bMainScene_JYL");
        }
    }
}
