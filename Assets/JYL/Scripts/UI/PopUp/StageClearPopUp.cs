using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace JYL
{
    public class StageClearPopUp : BaseUI
    {
        // TODO : �ش� �˾��� ���� Ŭ���� �������� �˾��ȴ�
        private void Start()
        {
            GetEvent("SCNextStageBtn").Click += NextStage;
            GetEvent("SCReBtn").Click += RestartStage;
            GetEvent("SCQuitBtn").Click += QuitStage;
        }
        private void NextStage(PointerEventData eventData)
        {
            // ���������� �����ؼ� �ε��ϴ� �Ͱ� ���� ȿ��. ���� ��Ȳ ������ ���� Ŭ���� ������ �ڵ����� ����
            // ���̵��� ���̵�ƿ� ȿ���� �ʿ��ѵ�, �������� �Ŵ������� �ش� ����� �����Ǿ�� ���� �ʳ� ��
            SceneManager.LoadSceneAsync("dStageScene_JYL");
        }
        private void RestartStage(PointerEventData eventData)
        {
            // "���� ��������" ��ɰ� �ٸ� �� ����. �ε��ϴ� ���� ���� ���������� ��
            SceneManager.LoadSceneAsync("dStageScene_JYL");
        }
        private void QuitStage(PointerEventData eventData)
        {
            // ����ȭ������ ���ư�
            SceneManager.LoadSceneAsync("bMainScene_JYL");
        }
    }

}
