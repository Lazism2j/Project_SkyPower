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

        public UnityEvent onGameOver;
        public UnityEvent<int> onScoreChanged; // �̺�Ʈ ��� Ȯ�� �ڵ�

        public void Init() // ���� �ʱ�ȭ
        {
            score = 0;
            playerLives = defaultPlayerLives; // default��
            isGameOver = false;
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

    }
}
