using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{
    public class GameManagerRunner : MonoBehaviour
    {

        // GameManagerSO �̱��� �ν��Ͻ� ��� �ʱ�ȭ
        private void Awake()
        {
            GameManagerSO.Instance.Init(); // �̱��� ������ ���� �ʱ�ȭ
        }
    }
}

