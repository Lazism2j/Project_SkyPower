using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class GameModePopUp : BaseUI
    {
        void Start()
        {
            GetEvent("StageModeBtn").Click += data => UIManager.Instance.ShowPopUp<StageSelectPopUp>();
            // ���Ѹ�� �߰� �� ��ư �Ҵ�
        }
    }
}

