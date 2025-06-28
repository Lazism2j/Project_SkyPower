// RnD/Scripts/ScriptableObjectMethod/StageSelectUI.cs
using UnityEngine;
using UnityEngine.UI;
using YSK;
using KYG_skyPower;
using JYL;
using TMPro;

namespace YSK
{
    public class StageSelectUI : BaseUI
    {
        [Header("Stage Data")]
        [SerializeField] private StageDataManager dataManager;

        [Header("UI Button Names")]
        [SerializeField] private string mainMenuButtonName = "MainMenuButton";
        [SerializeField] private string mainStageSelectButtonName = "MainStageSelectButton";
        [SerializeField] private string testSceneButtonName = "TestSceneButton";

        [Header("Stage Button Names")]
        [SerializeField] private string stage1_1ButtonName = "Stage1_1Button";
        [SerializeField] private string stage1_2ButtonName = "Stage1_2Button";
        [SerializeField] private string stage2_1ButtonName = "Stage2_1Button";
        [SerializeField] private string stage2_2ButtonName = "Stage2_2Button";
        [SerializeField] private string stage3_1ButtonName = "Stage3_1Button";

        [Header("Score Display Names")]
        [SerializeField] private string stage1_1ScoreTextName = "Stage1_1ScoreText";
        [SerializeField] private string stage1_2ScoreTextName = "Stage1_2ScoreText";
        [SerializeField] private string stage2_1ScoreTextName = "Stage2_1ScoreText";
        [SerializeField] private string stage2_2ScoreTextName = "Stage2_2ScoreText";
        [SerializeField] private string stage3_1ScoreTextName = "Stage3_1ScoreText";

        [Header("Lock Icon Names")]
        [SerializeField] private string stage1_1LockIconName = "Stage1_1LockIcon";
        [SerializeField] private string stage1_2LockIconName = "Stage1_2LockIcon";
        [SerializeField] private string stage2_1LockIconName = "Stage2_1LockIcon";
        [SerializeField] private string stage2_2LockIconName = "Stage2_2LockIcon";
        [SerializeField] private string stage3_1LockIconName = "Stage3_1LockIcon";

        private void Start()
        {
            InitializeDataManager();
            ConnectButtons();
            UpdateUI();
        }

        private void InitializeDataManager()
        {
            if (dataManager == null)
            {
                dataManager = FindObjectOfType<StageDataManager>();
                if (dataManager == null)
                {
                    Debug.LogWarning("StageDataManager�� ã�� �� �����ϴ�!");
                }
            }
        }

        private void ConnectButtons()
        {
            // �޴� ��ư�� ����
            ConnectMenuButton(mainMenuButtonName, "RnDMainMenu");
            ConnectMenuButton(mainStageSelectButtonName, "RnDMainStageSelectScene");
            ConnectMenuButton(testSceneButtonName, "RnDBaseStageTestScene");

            // �������� ��ư�� ����
            ConnectStageButton(stage1_1ButtonName, 1, 1);
            ConnectStageButton(stage1_2ButtonName, 1, 2);
            ConnectStageButton(stage2_1ButtonName, 2, 1);
            ConnectStageButton(stage2_2ButtonName, 2, 2);
            ConnectStageButton(stage3_1ButtonName, 3, 1);
        }

        private void ConnectMenuButton(string buttonName, string sceneName)
        {
            Button button = GetUI<Button>(buttonName);
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    Debug.Log($"{buttonName} ��ư Ŭ����! �� {sceneName} �ε�");
                    LoadScene(sceneName);
                });
                Debug.Log($"{buttonName} �޴� ��ư ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning($"{buttonName} ��ư�� ã�� �� �����ϴ�!");
            }
        }

        private void ConnectStageButton(string buttonName, int mainStage, int subStage)
        {
            Button button = GetUI<Button>(buttonName);
            if (button != null)
            {
                button.onClick.RemoveAllListeners();
                button.onClick.AddListener(() => {
                    Debug.Log($"{buttonName} ��ư Ŭ����! �������� {mainStage}-{subStage} �ε�");

                    // �ر� ���� Ȯ��
                    if (dataManager != null && !CanPlayStage(mainStage, subStage))
                    {
                        Debug.Log($"�������� {mainStage}-{subStage}�� �رݵ��� �ʾҽ��ϴ�!");
                        ShowLockMessage(mainStage, subStage);
                        return;
                    }

                    LoadStage(mainStage, subStage);
                });
                Debug.Log($"{buttonName} �������� ��ư ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning($"{buttonName} ��ư�� ã�� �� �����ϴ�!");
            }
        }

        private void UpdateUI()
        {
            if (dataManager == null) return;

            // �������� ��ư ���� ������Ʈ
            UpdateStageButtonState(stage1_1ButtonName, stage1_1LockIconName, stage1_1ScoreTextName, 1, 1);
            UpdateStageButtonState(stage1_2ButtonName, stage1_2LockIconName, stage1_2ScoreTextName, 1, 2);
            UpdateStageButtonState(stage2_1ButtonName, stage2_1LockIconName, stage2_1ScoreTextName, 2, 1);
            UpdateStageButtonState(stage2_2ButtonName, stage2_2LockIconName, stage2_2ScoreTextName, 2, 2);
            UpdateStageButtonState(stage3_1ButtonName, stage3_1LockIconName, stage3_1ScoreTextName, 3, 1);
        }

        private void UpdateStageButtonState(string buttonName, string lockIconName, string scoreTextName, int mainStage, int subStage)
        {
            Button button = GetUI<Button>(buttonName);
            if (button == null) return;

            bool isUnlocked = CanPlayStage(mainStage, subStage);

            // ��ư Ȱ��ȭ/��Ȱ��ȭ
            button.interactable = isUnlocked;

            // ��ư ���� ����
            ColorBlock colors = button.colors;
            if (isUnlocked)
            {
                colors.normalColor = Color.white;
                colors.disabledColor = Color.white;
            }
            else
            {
                colors.normalColor = Color.gray;
                colors.disabledColor = Color.gray;
            }
            button.colors = colors;

            // ��� ������ ǥ��/�����
            GameObject lockIcon = GetUI(lockIconName);
            if (lockIcon != null)
            {
                lockIcon.SetActive(!isUnlocked);
            }

            // ���� ǥ��
            if (isUnlocked)
            {
                UpdateScoreDisplay(scoreTextName, mainStage, subStage);
            }
        }

        private void UpdateScoreDisplay(string scoreTextName, int mainStage, int subStage)
        {
            if (dataManager == null) return;

            TextMeshProUGUI scoreText = GetUI<TextMeshProUGUI>(scoreTextName);
            if (scoreText != null)
            {
                int score = dataManager.GetStageScore(mainStage, subStage);
                if (score > 0)
                {
                    scoreText.text = $"�ְ� ����: {score}";
                    scoreText.gameObject.SetActive(true);
                }
                else
                {
                    scoreText.gameObject.SetActive(false);
                }
            }
        }

        private bool CanPlayStage(int mainStage, int subStage)
        {
            if (dataManager == null) return true;

            return dataManager.IsStageUnlocked(mainStage) &&
                   dataManager.IsSubStageUnlocked(mainStage, subStage);
        }

        private void ShowLockMessage(int mainStage, int subStage)
        {
            Debug.Log($"�������� {mainStage}-{subStage}�� ���� �رݵ��� �ʾҽ��ϴ�!");
        }

        private void LoadStage(int mainStage, int subStage)
        {
            if (GameSceneManager.Instance != null)
            {
                GameSceneManager.Instance.LoadGameSceneWithStage("RnDBaseStageTestScene", mainStage, subStage);
            }
            else
            {
                Debug.LogError("GameSceneManager.Instance�� null�Դϴ�!");
            }
        }

        private void LoadScene(string sceneName)
        {
            if (GameSceneManager.Instance != null)
            {
                GameSceneManager.Instance.LoadGameScene(sceneName);
            }
            else
            {
                Debug.LogError("GameSceneManager.Instance�� null�Դϴ�!");
            }
        }

        // �ܺο��� UI ������Ʈ�� ��û�� �� ���
        public void RefreshUI()
        {
            UpdateUI();
        }
    }
}