using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerRunner : MonoBehaviour
{
    public GameManagerSO gameManager;

    void Start()
    {
        // ���� �� ���� �簳 ���·� �ʱ�ȭ
        gameManager.ResumeGame();
    }
}
