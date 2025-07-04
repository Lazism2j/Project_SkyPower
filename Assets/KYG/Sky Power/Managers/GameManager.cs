using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;
using LJ2;
using IO;
using KYG_skyPower;

namespace KYG_skyPower
{

    // Removed duplicate declaration of 'equippedDict' to fix CS0102 error.
    public class GameManager : Singleton<GameManager>
    {
        public UnityEvent onGameOver, onPause, onResume, onGameClear;

        public EquipmentManagerSO equipmentManagerSO;

        public GameData[] saveFiles = new GameData[3]; // ���̺� ���� 3��

        public int currentSaveIndex { get; private set; } = 0;

        public GameData CurrentSave => saveFiles[currentSaveIndex]; // ���̺� ���� �ε����� �迭

        public bool isGameOver { get; private set; } // ���� ����
        public bool isPaused { get; private set; } // ���� �Ͻ� ����

        public bool isGameCleared { get; private set; } // ���� Ŭ����

        public int selectWorldIndex = 0;
        public int selectStageIndex = 0;

        public Dictionary<EquipmentType, EquipmentDataSO> equippedDict = new(); // ������ ��� ��ųʸ� (���� Ÿ�� -> ��� ������)

        // Removed duplicate declaration of 'equippedDict' here.
        // public Dictionary<KYG_skyPower.EquipmentType, KYG_skyPower.EquipmentDataSO> equippedDict = new(); 

        public override void Init() // ���� ���۽� ���̺� ������ �ε�  
        {
            ResetSaveRef(); // ���̺� ������ �ʱ�ȭ  

            // ������ �ڵ�: equipmentInventory�� GameData���� ������ �������� ĳ�����Ͽ� ���  
            if (CurrentSave.equipmentInventory is EquipmentInventory inventory)
            {
                equipmentManagerSO.savedEquipments = inventory.equipments; // ���� ���̺� ������ ��� ���� �ε�  
                equipmentManagerSO.BuildRuntimeInventory(); // ��Ÿ�� �κ��丮 ����  
            }
            else
            {
                Debug.LogError("equipmentInventory�� EquipmentInventory ������ �ƴմϴ�.");
            }
        }

        public void ResetSaveRef()
        {
            for (int i = 0; i < saveFiles.Length; i++) // ���̺� ���� �ʱ�ȭ
            {
                saveFiles[i] = new GameData(); // �� GameData ��ü ����
                SaveManager.Instance.GameLoad(ref saveFiles[i], i + 1); // �ε��� 1����
            }
        }

        public void EquipToCharacter(int equipId, int charId)
        {
            equipmentManagerSO.Equip(equipId, charId);
            // TODO: ĳ���� �ɷ�ġ, ���� �� �߰� �ݿ�
        }

        public void UnequipFromCharacter(int equipId)
        {
            equipmentManagerSO.Unequip(equipId);
            // TODO: ĳ���� �ɷ�ġ, ���� �� �߰� �ݿ�
        }

        public void AddEquipmentToInventory(int equipId, EquipmentType slotType)
        {
            equipmentManagerSO.AddEquipment(equipId, slotType);
        }

        public void SelectSaveFile(int index)
        {
            if (index >= 0 && index < saveFiles.Length)
                currentSaveIndex = index;

            
        }

        public void SetGameOver()
        {
            if (isGameOver) return;
            isGameOver = true; // ���� ������ true��
            Time.timeScale = 0f; // �ð� ���� ���
            onGameOver?.Invoke();
            Debug.Log("���� ����");
        }

        public void SetGameClear()
        {
            if (isGameCleared || isGameOver) return;
            isGameCleared = true;
            Time.timeScale = 0f;
            onGameClear?.Invoke();
            Debug.Log("���� Ŭ����");
        }

        public void PausedGame()
        {
            if (isPaused || isGameOver) return;
            isPaused = true;
            Time.timeScale = 0f; // ��ü ���� ����
            onPause?.Invoke();
            Debug.Log("�Ͻ� ����");
        }

        public void ResumeGame()
        {
            if (!isPaused || isGameOver) return;
            isPaused = false;
            Time.timeScale = 1f; // ���� �ð� ����ȭ
            onResume?.Invoke();
            Debug.Log("���� �簳");
        }

        public void ResetStageIndex()
        {
            selectWorldIndex = 0;
            selectStageIndex = 0;
        }
    }
}

