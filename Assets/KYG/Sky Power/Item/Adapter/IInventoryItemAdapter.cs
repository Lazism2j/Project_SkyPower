using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KYG_skyPower
{


public interface IInventoryItemAdapter // ������ ����� �������̽�
    {
    string GetName(); // ������ �̸�
        Sprite GetIcon(); // ������ ������
        int GetSortOrder(); // ���� �켱����
    bool IsEquipped(); // �������� �����Ǿ� �ִ��� ����
        int GetLevel(); // ������ ����
                        // TODO : ���/Ÿ�� �� �߰�
    }
}