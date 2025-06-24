using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG
{
    public class GameManagerRunner : MonoBehaviour
    {
        public GameManagerSO gameManager;

        void Start()
        {
            gameManager.ResetScore(); // ���� ����� ������ �ʱ�ȭ
                                      // ���� �� ���� �簳 ���·� �ʱ�ȭ
            gameManager.ResumeGame();

        }
    }
}