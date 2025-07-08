using KYG_skyPower;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace KYG_skyPower
{
    [DefaultExecutionOrder(-100)]
    public class DialogueManager : MonoBehaviour
    {
        // === �̱��� ������ ===
        private static DialogueManager instance;
        [Header("Prefab")]
        public GameObject dialogueUIPrefab;
        public static DialogueManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<DialogueManager>();
                    if (instance == null)
                    {
                        GameObject obj = new GameObject(nameof(DialogueManager));
                        instance = obj.AddComponent<DialogueManager>();
                        // �ʿ��: DontDestroyOnLoad(obj);
                    }
                }
                return instance;
            }
        }
        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            DontDestroyOnLoad(gameObject); // �� �߿�!
            AutoFindOrSpawnUI();
        }

        // === ���� �ʵ� ===
        [Header("UI")]
        public GameObject dialoguePanel;
        public Image speakerImage;
        public TMP_Text speakerNameText;
        public TMP_Text dialogueText;
        public Button nextButton;

        [Header("Data")]
        public DialogDB dialogDB;
        public UnityEvent<DialogLine> OnDialogLine = new UnityEvent<DialogLine>();

        private int currentIndex = 0;
        private bool isDialogueActive = false;

        // === �޼ҵ� ===
        public void StartDialogue()
        {
            // �г��� �ı��ưų� ���� ������� �� �ڵ� ����
            if (dialoguePanel == null || !dialoguePanel)
            {
                Debug.LogWarning("dialoguePanel�� ���ǵǾ� UI ���Ҵ� �õ�");
                AutoFindOrSpawnUI();
                if (dialoguePanel == null || !dialoguePanel)
                {
                    Debug.LogError("dialoguePanel�� ã�� �� �����ϴ�!");
                    return;
                }
            }

            if (dialogDB == null || dialogDB.lines.Count == 0) return;

            isDialogueActive = true;
            dialoguePanel.SetActive(true);
            currentIndex = 0;
            Time.timeScale = 0f;
            ShowLine();
        }

        public void SetDialogDBByStageName(string stageName)
        {
            // Resources/DialogDB/Stage_1 �̷� ������ ����
            dialogDB = Resources.Load<DialogDB>($"DialogDB/{stageName}");
        }

        public void LoadDialogDBByStageID(int mainStageID, int subStageID)
        {
            string dialogDBName = $"Stage_{mainStageID}_{subStageID}";
            dialogDB = Resources.Load<DialogDB>($"DialogDB/{dialogDBName}");
            if (dialogDB == null)
                Debug.LogWarning($"DialogDB '{dialogDBName}'�� ã�� �� �����ϴ�!");
        }

        private void AutoFindOrSpawnUI()
        {
            if (dialoguePanel != null)
            {
                AutoAssignUIFields(dialoguePanel);
                return;
            }

            // ���� Canvas ã�� (Scene/Hierarchy���� �ƹ� Canvas�� ���)
            Canvas canvas = FindObjectOfType<Canvas>();
            if (dialogueUIPrefab != null)
            {
                GameObject go;
                if (canvas != null)
                    go = Instantiate(dialogueUIPrefab, canvas.transform); // **Canvas ������**
                else
                    go = Instantiate(dialogueUIPrefab); // Fallback

                DontDestroyOnLoad(go);
                dialoguePanel = go;
                AutoAssignUIFields(go);
                return;
            }

            Debug.LogWarning("DialogueManager: dialogueUIPrefab �Ǵ� Panel�� ã�� �� �����ϴ�!");
        }

        private void AutoAssignUIFields(GameObject root)
        {
            // ��Ȱ��ȭ ���� ��� �������� Ž��!
            speakerImage = root.GetComponentsInChildren<Image>(true)
                .FirstOrDefault(x => x.name == "SpeakerImage");
            speakerNameText = root.GetComponentsInChildren<TMP_Text>(true)
                .FirstOrDefault(x => x.name == "SpeakerNameText");
            dialogueText = root.GetComponentsInChildren<TMP_Text>(true)
                .FirstOrDefault(x => x.name == "DialogueText");
            nextButton = root.GetComponentsInChildren<Button>(true)
                .FirstOrDefault(x => x.name == "NextButton");
            // OnClick �ڵ� �Ҵ�!
            if (nextButton != null)
            {
                nextButton.onClick.RemoveAllListeners(); // Ȥ�� �ߺ� ����
                nextButton.onClick.AddListener(OnClickNext); // �ݵ�� DialogueManager�� �޼��� ����!
            }

            if (speakerImage == null) Debug.LogWarning("speakerImage �ڵ��Ҵ� ����");
            if (speakerNameText == null) Debug.LogWarning("speakerNameText �ڵ��Ҵ� ����");
            if (dialogueText == null) Debug.LogWarning("dialogueText �ڵ��Ҵ� ����");
            if (nextButton == null) Debug.LogWarning("nextButton �ڵ��Ҵ� ����");
        }

        private void ShowLine()
        {
            if (currentIndex >= dialogDB.lines.Count)
            {
                EndDialogue();
                return;
            }

            var line = dialogDB.lines[currentIndex];

            if (speakerImage == null || speakerNameText == null || dialogueText == null)
            {
                Debug.LogError("DialogueManager UI �ʵ� ���� �ȵ�!");
                return;
            }

            // UI ����
            speakerImage.sprite = line.speakerSprite;
            if (line.speakerSprite == null)
                speakerImage.gameObject.SetActive(false);
            else
                speakerImage.gameObject.SetActive(true);

            speakerNameText.text = line.speaker;
            dialogueText.text = line.content;

            OnDialogLine.Invoke(line);
        }

        public void OnClickNext()
        {
            if (!isDialogueActive) return;

            currentIndex++;
            ShowLine();
        }

        private void EndDialogue()
        {
            isDialogueActive = false;
            dialoguePanel.SetActive(false);
            Time.timeScale = 1f; // �Ͻ����� ���� (���� �����)
        }
    }
}
