using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using LJ2;

namespace JYL
{
    public class PartySetPopUp : BaseUI
    {
        [SerializeField] CharactorController[] charactorController;
        private RawImage charImg1;
        private RawImage charImg2;
        private RawImage charImg3;
        private string charPrefabPath = "CharacterPrefabs";
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
            GetCharPrefab();
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
        private void GetCharPrefab()
        {
            //ĳ���� ������ ���� ��������
            charactorController= Resources.LoadAll<CharactorController>(charPrefabPath);
            foreach (var cont in charactorController)
            {
                cont.SetParameter();
            }
            // ���� ���Ķ���� ��.
        }
    }
}

