using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

namespace JYL
{
    public class StorePresenter : BaseUI
    {

        void Start()
        {
            GetEvent("GachaChrBtn1").Click += CharacterGacha;
            GetEvent("GachaChrBtn5").Click += EquipmentGacha;
            GetEvent("GachaEquipBtn1").Click += CharacterGacha;
            GetEvent("GachaEquipBtn5").Click += EquipmentGacha;
            GetEvent("StoreItemImg").Click += ItemStore;
        }
        private void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                Debug.Log($"{PopUpUI.IsPopUpActive}, {Util.escPressed}");
            }
            if(Input.GetKeyDown(KeyCode.Escape)&&!PopUpUI.IsPopUpActive&&!Util.escPressed)
            {
                // �� ��ȯ
                SceneManager.LoadSceneAsync("bMainScene_JYL");
            }
        }
        
        private void CharacterGacha(PointerEventData eventData)
        {
            // TODO : ��í ����� ���ÿ�, �κ��丮(ĳ���� ���)�� �߰�. ���ӸŴ������� ���̺���
            // ���� �Ϸ� ��, ��í ���� ����. ���� �Ϸ��� ��� �˾� â�� ���
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int num);
            switch (num)
            {
                case 1:

                    UIManager.Instance.ShowPopUp<GachaPopUp>();
                    break;
                case 5:
                    UIManager.Instance.ShowPopUp<Gacha5PopUp>();
                    break;
            }
        }
        private void EquipmentGacha(PointerEventData eventData)
        {
            Util.ExtractTrailNumber(eventData.pointerClick.name, out int num);
            switch (num)
            {
                case 1:
                    UIManager.Instance.ShowPopUp<GachaPopUp>();
                    break;
                case 5:
                    UIManager.Instance.ShowPopUp<Gacha5PopUp>();
                    break;
            }
        }

        // TODO : ������ �߰� �� �۾�
        private void ItemStore(PointerEventData eventData)
        {

        }
    }
}


