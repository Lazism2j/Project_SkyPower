using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;
using JYL;

namespace LJ2
{
    public class SaveManager : Singleton<SaveManager>
    {
        protected override void Awake() => base.Awake();

        // ���� �� ����, �ε�, ���� �Լ� ���� ����
        public void PlayerSave(CharictorSave target, int index)
        {
            DataSaveController.Save(target, index);
        }

        public void PlayerLoad(CharictorSave target, int index)
        {
            DataSaveController.Load(ref target, index);
        }

        public void PlayerDelete(CharictorSave target, int index)
        {
            DataSaveController.Delete(target, index);
        }

        // ���� partial class�� ������ GameData�� control�ϴ� �Լ���
        public void GameSave(GameData target, int index,string name)
        {
            target.playerName = name;
            DataSaveController.Save(target, index);
        }

        public void GameLoad(ref GameData target, int index)
        {
            DataSaveController.Load(ref target, index);
        }

        public void GameDelete(GameData target, int index)
        {
            DataSaveController.Delete(target, index);
        }
    }
}
