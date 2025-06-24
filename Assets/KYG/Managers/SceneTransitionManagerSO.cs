using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


[CreateAssetMenu(fileName = "SceneManager", menuName = "Managers/SceneManager")]
public class SceneTransitionManagerSO : ScriptableObject
{
    // �̸����� �� �ε�
    public void LoadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    // ���� �� ���ε�
    public void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
