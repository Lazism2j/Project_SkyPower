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

    
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent onGameOver, onPause, onResume, onGameClear;

        
        public bool isGameOver { get; private set; } // ���� ����
        public bool isPaused { get; private set; } // ���� �Ͻ� ����

        public bool isGameCleared { get; private set; } // ���� Ŭ����

        //[SerializeField] private int defaultPlayerHP = 5;
        //public int playerHp { get; private set; } // �÷��̾ ���� ���� ������ ���߿� �߰� ���� ���� �ּ� ó��

        public override void Init()
        {
            
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

