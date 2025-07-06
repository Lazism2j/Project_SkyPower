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
        }
        private void SetNextStageBtn()
        {
            nextButton = GetUI<Button>("SCNextStageBtn");
            int worldIndex = Manager.Game.selectWorldIndex;
            int stageIndex = Manager.Game.selectStageIndex;
            if (stageIndex > 5)
            {
                worldIndex++;
                stageIndex = 1;
            }
            if (Manager.SDM.runtimeData[worldIndex].subStages[stageIndex] == null)
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
            // ���������� �����ؼ� �ε��ϴ� �Ͱ� ���� ȿ��. ���� ��Ȳ ������ ���� Ŭ���� ������ �ڵ����� ����
            // ���̵��� ���̵�ƿ� ȿ���� �ʿ��ѵ�, �������� �Ŵ������� �ش� ����� �����Ǿ�� ���� �ʳ� ��
            Time.timeScale = 1.0f;
            UIManager.Instance.CleanPopUp();
            Manager.Game.selectStageIndex++;
            if(Manager.Game.selectStageIndex>5)
            {
                Manager.Game.selectWorldIndex++;
                Manager.Game.selectStageIndex = 1;
            }
            Manager.Score.RecordBestScore();
            Manager.Score.ResetScore();
            Manager.GSM.LoadGameSceneWithStage("dStageScene_JYL", Manager.Game.selectWorldIndex, Manager.Game.selectStageIndex);
        }
        private void RestartStage(PointerEventData eventData)
        {
            Time.timeScale = 1.0f;
            Manager.Score.ResetScore();
            Manager.GSM.LoadGameSceneWithStage("dStageScene_JYL",Manager.Game.selectWorldIndex,Manager.Game.selectStageIndex);
        }
        private void QuitStage(PointerEventData eventData)
        {
            Time.timeScale = 1.0f;
            Manager.Score.RecordBestScore();
            Manager.Score.ResetScore();
            Manager.GSM.LoadScene("bMainScene_JYL");
        }
    }

}
