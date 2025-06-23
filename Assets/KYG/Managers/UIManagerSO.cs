using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UIManager", menuName = "Managers/UIManager")]
public class UIManagerSO : ScriptableObject
{
    [HideInInspector] public GameObject pauseUI;

    // UI �г� ��ü ����
    public void Init(GameObject pausePanel)
    {
        pauseUI = pausePanel;
    }

    // �Ͻ� ���� UI On/Off
    public void ShowPauseUI(bool show)
    {
        if (pauseUI != null)
            pauseUI.SetActive(show);
    }
}