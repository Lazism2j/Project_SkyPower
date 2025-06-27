using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace KYG_skyPower
{
    [CreateAssetMenu(fileName = "SceneChangerManagerSO", menuName = "Manager/SceneChangerManager")]
    public class SceneChangerManagerSO : SOSingleton<SceneChangerManagerSO>
    {
        public override void Init()
        {
            // �Ŀ� �ʿ��
        }
        public void LoadScene(string name) // ������ �̸��� �� �ε�
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("[SceneChangerManagerSO] �� �̸��� ����ֽ��ϴ�.");
                return;
            }

            if (!Application.CanStreamedLevelBeLoaded(name))
            {
                Debug.LogError($"[SceneTransitionManagerSO] '{name}' ���� Build Settings���� ã�� �� �����ϴ�.");
                return;
            }
            SceneManager.LoadScene(name);
        }

        public void ReloadCurrentScene() // ���� �� ���ΰ�ħ
        {
            var current = SceneManager.GetActiveScene().name;
            if (string.IsNullOrEmpty(current))
            {
                Debug.LogError("[SceneTransitionManagerSO] ���� �� �̸��� �������� ���߽��ϴ�.");
                return;
            }
            SceneManager.LoadScene(current);
        }
    }
}
