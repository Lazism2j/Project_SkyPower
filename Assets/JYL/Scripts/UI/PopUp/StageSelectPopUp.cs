using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace JYL
{
    public class StageSelectPopUp : BaseUI
    {

        // UIManager�� ���� ������ ������ �ε����� ������ �ʿ䰡 ����.
        // ���� �ش� ���尡 lock�� ���, Ŭ���� ����. �ش� ������ ���� �Ŵ��� �Ǵ� ���������Ŵ���, ���Ŵ����� ����
        void Start()
        {
            GetEvent("Stage1").Click += OnStageClick;
        }

        void Update()
        {

        }

        private void OnStageClick(PointerEventData eventData)
        {
            Util.ExtractTrailNumber(eventData.pointerClick.gameObject.name, out int index);
            Debug.Log($"�ش� �������� ���õ� : {index}");
            // ���⼭ index�� UIManager�� ����
            UIManager.Instance.ShowPopUp<StageInerSelectPopUp>();

        }
    }
}

