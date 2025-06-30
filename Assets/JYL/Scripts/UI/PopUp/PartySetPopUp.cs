using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace JYL
{
    public class PartySetPopUp : BaseUI
    {

        private RawImage charImg1;
        private RawImage charImg2;
        private RawImage charImg3;

        private List<RawImage> stayCharImg;
        // Start is called before the first frame update
        private void OnEnable()
        {
            // ���ӸŴ����� ���̺������� ���� ĳ���� ����Ʈ�� �ҷ���
            // ĳ���� ���� ��ŭ stayCharImg ���� List.Length
            // �������� =0�� ĳ���� �帲 ó��
            // ���� ���̸鼭, ��Ƽ�� ���� ĳ���� ȸ�� ó��
            // �巡��&������� ĳ�� ��
            // charImage1~3�� ���� ĳ���� ��������Ʈ �̹����� ������

        }
        void Start()
        {
            // 
            GetEvent("PartyCharImage1").Click += OpenInvenPopUp;
            GetEvent("PartyCharImage2").Click += OpenInvenPopUp;
            GetEvent("PartyCharImage3").Click += OpenInvenPopUp;
        }

        // Update is called once per frame
        void Update()
        {

        }
        private void OpenInvenPopUp(PointerEventData eventData)
        {
            // ���õ� ĳ���� �������� �κ��� ����
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int index);
            // GameManager.Instance.selectSave.party[index] -> ĳ���� ID
            // ĳ���� ��Ʈ�ѷ� (ĳ���� ID)
            Debug.Log($"{index}");
            UIManager.Instance.ShowPopUp<InvenPopUp>();
        }
    }
}

