using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayingState : IGameState
{
    public void Enter()
    {
        Debug.Log("���� ����!");
        // Ÿ�̸� ����, �������� ���� ��
    }

    public void Update()
    {
        // ���� ���� ���� üũ, Ŭ���� ���� ��
    }

    public void Exit()
    {
        Debug.Log("���� �÷��� ����");
        // ���� ����, ���ҽ� ���� ��
    }
}
