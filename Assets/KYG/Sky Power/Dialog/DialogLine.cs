using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KYG_skyPower
{


    [Serializable]
    public class DialogLine // Dialog ��Ģ(����)
    {

        public int id; 
        public string speaker; // ȭ��(���ϴ� ���)
        public string portraitKey; // Sprite�� ��Ÿ�ӿ� ���ҽ�.�ε�� �Ҵ�
        public string text; // ���
        public bool isBranch; // ������ ����
        public List<int> nextIds = new List<int>(); // ���� ���
        public string voiceKey; //  ���̽�

    }
}
