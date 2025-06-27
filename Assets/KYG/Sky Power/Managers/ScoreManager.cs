using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace KYG_skyPower
{
    // ���� ���� �Ŵ���
    public class ScoreManager : Singleton<ScoreManager>
    {
        
        public UnityEvent<int> onScoreChanged; // UI ������� UI ������ ���� �̺�Ʈ�� Ȯ�强
        public int Score { get; private set; }

        public override void Init()
        {
            // �ʿ伺 Ȯ��
        }

        /*private void Awake()
        {
            if (Instance != null && Instance != this) { Destroy(gameObject); return; }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }*/

        public void ResetScore() { Score = 0; onScoreChanged?.Invoke(Score); }
        public void AddScore(int value)
        {
            if (GameManager.Instance != null && GameManager.Instance.isGameOver) return;
            Score += value;
            onScoreChanged?.Invoke(Score);
        }
    }
}
