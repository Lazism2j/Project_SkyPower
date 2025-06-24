using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG
{
    [CreateAssetMenu(fileName = "GameManager", menuName = "Managers/GameManager")]
    public class GameManagerSO : ScriptableObject
    {
        // ���� �Ͻ� ���� ����
        [HideInInspector] public bool isGamePaused;

        // �÷��̾� ����
        [HideInInspector] public int score;

        // ���� �߰�
        public void AddScore(int value)
        {
            score += value;
        }

        // ���� �Ͻ� ����
        public void PauseGame()
        {
            isGamePaused = true;
            Time.timeScale = 0f;
        }

        // ���� �簳
        public void ResumeGame()
        {
            isGamePaused = false;
            Time.timeScale = 1f;
        }

        public void ResetScore()
        {
            score = 0;
        }
    }
}