using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using LJ2;
using IO;


namespace KYG_skyPower
{

    /*
    
    �̱��� ��� ���� �Ŵ���

    �ʿ� ���
    ���� ���ھ�
    ���� ���� ����
    ���� �Ͻ�����,���� �簳 ���

    �߰� ���
    �̺�Ʈ ��� �ڵ�� Ȯ�强 ���
    ���̺� �����͸� �迭�� ���� �Ŵ����� ������ �;� �Ѵ�
    ���� ���۽� ���̺� ������ ������ �ͼ� ������ �ִ´�

    */

    
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent onGameOver, onPause, onResume, onGameClear;

        public GameData[] saveFiles = new GameData[3]; // ���̺� ���� 3��

        public int currentSaveIndex { get; private set; } = 0;

        public GameData CurrentSave => saveFiles[currentSaveIndex]; // ���̺� ���� �ε����� �迭

        public bool isGameOver { get; private set; } // ���� ����
        public bool isPaused { get; private set; } // ���� �Ͻ� ����

        public bool isGameCleared { get; private set; } // ���� Ŭ����

        public int selectWorldIndex=0;
        public int selectStageIndex=0;

        //[SerializeField] private int defaultPlayerHP = 5;
        //public int playerHp { get; private set; } // �÷��̾ ���� ���� ������ ���߿� �߰� ���� ���� �ּ� ó��

        public override void Init() // ���� ���۽� ���̺� ������ �ε�
        {
            ResetSaveRef();
        }
        public void ResetSaveRef()
        {
            for (int i = 0; i < saveFiles.Length; i++)
            {
                saveFiles[i] = new GameData();
                SaveManager.Instance.GameLoad(ref saveFiles[i], i + 1); // �ε��� 1����
            }
        }

        public Dictionary<KYG_skyPower.EquipmentSlotType, EquipmentData> equippedDict = new();

        public void Equip(EquipmentData equip)
        {
            equippedDict[equip.GetSlotType()] = equip;
            // ���� ����, �ɷ�ġ ���� ��
        }

        public bool IsEquipped(IInventoryItemAdapter equip)
        {
            if (equip is EquipmentData data)
            {
                if (equippedDict.TryGetValue(data.GetSlotType(), out var equipped))
                    return equipped == data;
            }
            return false;
        }

        public void SelectSaveFile(int index)
        {
            if (index >= 0 && index < saveFiles.Length)
                currentSaveIndex = index;
        }

        /*private void Awake() // �̱��� ����
        {
            if (Instance != null && Instance != this) // ���� �ٸ� Instance ������ �� Instance
            {
                Destroy(gameObject); // ����
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ������Ʈ �ı����� �ʰ� ����

        }*/



        public void SetGameOver()
        {
            if (isGameOver) return;
            isGameOver = true; // ���� ������ true��
            Time.timeScale = 0f; // �ð� ���� ���
            onGameOver?.Invoke();
            Debug.Log("���� ����");
        }

        public void SetGameClear()
        {
            if (isGameCleared || isGameOver) return;
            isGameCleared = true;
            Time.timeScale = 0f;
            onGameClear?.Invoke();
            Debug.Log("���� Ŭ����");
        }

        public void PausedGame()
        {
            if (isPaused || isGameOver) return;
            isPaused = true;
            Time.timeScale = 0f; // ��ü ���� ����
            onPause?.Invoke();
            Debug.Log("�Ͻ� ����");
        }

        public void ResumeGame()
        {
            if (!isPaused || isGameOver) return;
            isPaused = false;
            Time.timeScale = 1f; // ���� �ð� ����ȭ
            onResume?.Invoke();
            Debug.Log("���� �簳");
        }

        public void ResetStageIndex()
        {
            selectWorldIndex = 0;
            selectStageIndex = 0;
        }
        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPaused) ResumeGame();
                else PausedGame();
            }
        }*/ // ESC Ű�Է����� �Ͻ����� ��� ���÷� �ۼ�
    }
}

