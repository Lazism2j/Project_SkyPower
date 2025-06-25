using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KYG_skyPower
{
    [CreateAssetMenu(fileName = "GameManagerSO", menuName = "Manager/GameManager")]
    public class GameManagerSO : ScriptableObject
    {

        private static GameManagerSO instance;

        public static GameManagerSO Instance
        {
            get
            {
                if (!instance)
                    instance = Resources.Load<GameManagerSO>("GameManagerSO");
                return instance;
            }
        }

        [Header("Game State")]
        public int score; // ���� ���ھ�
        [SerializeField] private int defaultPlayerLives = 5; // �ʱ�ȭ �� �����Ϳ��� ���� ����
        [SerializeField] public int playerLives; // �÷��̾� ü��
        public bool isGameOver; // ���� ���� ����
        public bool isGamePaused;

        [Header("Game Events")]
        public UnityEvent onGameOver;
        public UnityEvent<int> onScoreChanged; // �̺�Ʈ ��� Ȯ�� �ڵ�
        public UnityEvent onPause;
        public UnityEvent onResume;

        public void Init() // ���� �ʱ�ȭ
        {
            score = 0;
            playerLives = defaultPlayerLives; // default��
            isGameOver = false;
            isGamePaused = false;
            Debug.Log("Game Initialized"); // ����׿� �ڵ�
        }

        public void AddScore(int amount) // ���� ����
        {
            score += amount;
            onScoreChanged?.Invoke(score);
        }

        public void PlayerHit() // �÷��̾� �ǰ�
        {
            playerLives--;
            if (playerLives <= 0)
            {
                isGameOver = true;
                onGameOver?.Invoke();
            }

        }

        public void PauseGame() // ���� �Ͻ�����
        {
            if (!isGamePaused)
            {
                isGamePaused = true;
                Time.timeScale = 0f;
                onPause?.Invoke();
                Debug.Log("Game Paused");
            }
        }

        public void ResumeGame() // ���� �簳
        {
            if (isGamePaused)
            {
                isGamePaused = false;
                Time.timeScale = 1f;
                onResume?.Invoke();
                Debug.Log("Game Resumed");
            }
        }

    }
}
