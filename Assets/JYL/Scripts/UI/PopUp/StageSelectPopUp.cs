using KYG_skyPower;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using YSK;

namespace JYL
{
    public class StageSelectPopUp : BaseUI
    {
        private int worldNum = 10;
        private List<StageRuntimeData> stageData;
        // UIManager�� ���� ������ ������ �ε����� ������ �ʿ䰡 ����.
        // ���� �ش� ���尡 lock�� ���, Ŭ���� ����. �ش� ������ ���� �Ŵ��� �Ǵ� ���������Ŵ���, ���Ŵ����� ����
        void Start()
        {
        }
        private void OnEnable()
        {
            stageData = Manager.SDM.runtimeData;
            for (int i = 0; i < stageData.Count; i++)
            {
                if (stageData[i].subStages[0].isUnlocked)
                {
                    GetEvent($"Stage{i + 1}").Click += OnStageClick;
                    if (i + 1 <= stageData.Count && !stageData[i + 1].subStages[0].isUnlocked)
                    {
                        GetUI($"World{i + 1}SelectIcon").gameObject.SetActive(true);
                    }
                }
            }
        }
        void Update()
        {

        }

        private void OnStageClick(PointerEventData eventData)
        {
            Util.ExtractTrailNumber(eventData.pointerClick.gameObject.name, out int index);
            UIManager.Instance.selectIndexUI = index-1;
            UIManager.Instance.ShowPopUp<StageInerSelectPopUp>();

        }
    }
}

