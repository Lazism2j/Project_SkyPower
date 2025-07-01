using KYG_skyPower;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using KYG_skyPower;

namespace JYL
{
    public class SaveFilePanel : BaseUI
    {
        // �ҷ��Դ� ���̺� ������ �������� �����͸� ä���
        GameData data;
        void Start()
        {
            data = Manager.Game.saveFiles[Manager.Game.currentSaveIndex];
            GetUI<TMP_Text>("SaveFileData").text = $"{Manager.Game.saveFiles[Manager.Game.currentSaveIndex].playerName}";
            GetEvent("SaveDelBtn").Click += OnDelClick;
            GetEvent("SaveStartBtn").Click += OnStartClick;
        }

        //���̺������� ����
        private void OnDelClick(PointerEventData eventData)
        {
            Manager.Save.GameDelete(data, Manager.Game.currentSaveIndex+1);
            UIManager.Instance.ClosePopUp();
            Manager.Game.ResetSaveRef();
        }

        //���̺� ���Ϸ� ���� ����
        private void OnStartClick(PointerEventData eventData)
        {
            // �� �Ѿ -> mainScene
            // ���� UI��� ���ؼ� ���̺������� ���õǾ� ����.
            Manager.SDM.SyncRuntimeDataWithStageInfo();
            SceneManager.LoadSceneAsync("bMainScene_JYL");
        }

    }
}
