using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class EnhancePopUp : BaseUI
    {
        // UIManager, GameManager ���� ���� ���� ���� ��ȭ â�� ĳ����ȭâ���� ��� ��ȭâ���� �Ǻ���
        // ĳ�� �Ǵ� ��� ��ȭ �Ҹ� �� Level�� �ø�.
        
        // �� �ʵ���� ������������ �Ŵ������� ���� �����´�.
        // private CharacterController CharacterController;
        // private Item item;

        void Start()
        {
            //GetEvent("EnhanceBtn").Click += data => item.level++; �Ǵ� �������� �Լ� ����
            GetEvent("EnhanceBtn").Click += data => UIManager.Instance.ClosePopUp();
        }
    }
}

