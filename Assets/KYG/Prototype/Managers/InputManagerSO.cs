using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG
{
    [CreateAssetMenu(fileName = "InputManager", menuName = "Managers/InputManager")]
    public class InputManagerSO : ScriptableObject
    {
        public KeyCode pauseKey = KeyCode.Escape;

        // �Ͻ� ���� Ű �Է� Ȯ��
        public bool IsPausePressed()
        {
            return Input.GetKeyDown(pauseKey);
        }
    }
}