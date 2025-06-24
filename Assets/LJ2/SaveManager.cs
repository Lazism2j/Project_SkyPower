using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IO;

namespace LJ2
{
    public class SaveManager : MonoBehaviour
    {
        private static SaveManager instance;
        public static SaveManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // ���̺��� ���� ��� : SaveData �� ��� �޾ƾ� ��
        public PlayerSave player;


        // ���� �� ����, �ε�, ���� �Լ� ���� ����
        public void PlayerSave(int index)
        {
            DataSaveController.Save(player.saveDataSample, index);
        }

        public void PlayerLoad(int index)
        {
            DataSaveController.Load(ref player.saveDataSample, index);
        }

        public void PlayerDelete(int index)
        {
            DataSaveController.Delete(player.saveDataSample, index);
        }
    }
}
