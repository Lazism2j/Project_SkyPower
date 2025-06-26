using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    // �Է� ������ ��� (Pause ��)
    public class InputManager : MonoBehaviour
    {
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (GameManager.Instance.isPaused)
                    GameManager.Instance.ResumeGame();
                else
                    GameManager.Instance.PausedGame();
            }
        }
    }
}
