using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class InvenPopUp : BaseUI
    {
        // ���ӸŴ������� ĳ���� ������ �ҷ��;� ��
        private GameObject invenScroll => GetUI("InvenScroll");
        void Start()
        {
            // ��� Ŭ���� Ȱ��ȭ
            invenScroll.SetActive(false);
        }

        void Update()
        {

        }
    }
}


