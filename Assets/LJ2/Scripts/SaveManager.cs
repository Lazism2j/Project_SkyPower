using IO;
using KYG_skyPower;
using System;
using UnityEngine;
using UnityEngine.Diagnostics;

namespace LJ2
{
    public class SaveManager : Singleton<SaveManager>
    {
        private int subStage { get; set; } = 5;
        protected override void Awake() => base.Awake();

        // ���� �� ����, �ε�, ���� �Լ� ���� ����
        //public void PlayerSave(CharictorSave target, int index)
        //{
        //    DataSaveController.Save(target, index);
        //}

        //public void PlayerLoad(CharictorSave target, int index)
        //{
        //    DataSaveController.Load(ref target, index);
        //}

        //public void PlayerDelete(CharictorSave target, int index)
        //{
        //    DataSaveController.Delete(target, index);
        //}

        // ���� partial class�� ������ GameData�� control�ϴ� �Լ���
        
        public void GameSave(GameData target, int index, string name = "")
        {
            if(name != "")
            {
                target.playerName = name;
            }
            GameData saveTargetData = SaveStageInfo(target);
            DataSaveController.Save(saveTargetData, index);
        }

        public void GameLoad(ref GameData target, int index)
        {
            DataSaveController.Load(ref target, index);
        }

        public void GameDelete(GameData target, int index)
        {
            DataSaveController.Delete(target, index);
        }

        private GameData SaveStageInfo(GameData target)
        {
            target.stageInfo = new StageInfo[Manager.SDM.runtimeData.Count * subStage];
            for (int i = 0; i < Manager.SDM.runtimeData.Count * subStage; i++)
            {
                target.stageInfo[i] = new StageInfo
                {
                    world = 1 + i / subStage,
                    stage = 1 + i % subStage,
                    unlock = Manager.SDM.runtimeData[i / subStage].subStages[i % subStage].isUnlocked,
                    isClear = Manager.SDM.runtimeData[i / subStage].subStages[i % subStage].isCompleted
                };
            }
            return target;
        }
    }
}
