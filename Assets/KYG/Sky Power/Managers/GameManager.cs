using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;


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

    */

    
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        //[Header("���� ����")]

        public int score { get; private set; } // ���� ���� ���ھ�
        public bool isGameOver { get; private set; } // ���� ����
        public bool isPaused { get; private set; } // ���� �Ͻ� ����

        //[SerializeField] private int defaultPlayerHP = 5;
        //public int playerHp { get; private set; } // �÷��̾ ���� ���� ������ ���߿� �߰� ���� ���� �ּ� ó��

        [Header("�̺�Ʈ")]
        public UnityEvent onGameOver; // ���� ���� �� ȣ��
        public UnityEvent<int> onScoreChanged; // ���� ���� �� ȣ��(�Ķ����: ����� ����)
        public UnityEvent onPause;  // �Ͻ����� �� ȣ��
        public UnityEvent onResume;  // �Ͻ����� ���� �� ȣ��

        private void Awake() // �̱��� ����
        {
            if (Instance != null && Instance != this) // ���� �ٸ� Instance ������ �� Instance
            {
                Destroy(gameObject); // ����
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // ���� ������Ʈ �ı����� �ʰ� ����
            Init(); // ���� ���� �ʱ�ȭ
        }


        public void Init()
        {
            score = 0;
            isGameOver = false;
            isPaused = false;
            Time.timeScale = 1f; // �ٽ� �ð� �帣��
            //playerHp = defaultPlayerHp;
        }

        public void AddScore(int value) // ���� �߰�
        {
            if (isGameOver) return; // ���ӿ��� ���¿��� ���� ���� X
            score += value;
            onScoreChanged?.Invoke(score); // UI, ����Ʈ �� �̺�Ʈ�� ���� ����
        }

        public void SetGameOver()
        {
            isGameOver = true; // ���� ������ true��
            Time.timeScale = 0f; // �ð� ���� ���
            onGameOver?.Invoke();
            Debug.Log("���� ����");            
        }

        public void PausedGame()
        {
            if (isPaused || isGameOver) return;
            isPaused = true;
            Time.timeScale = 0f; // ��ü ���� ����
            onGameOver?.Invoke();
            Debug.Log("�Ͻ� ����");
        }

        public void ResumeGame()
        {
            if (!isPaused || isGameOver) return;
            isPaused = false;
            Time.timeScale = 1f; // ���� �ð� ����ȭ
            onGameOver?.Invoke();
            Debug.Log("���� �簳");
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

