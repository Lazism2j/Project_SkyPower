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

        public GameData[] saveFiles = new GameData[3]; // 세이브 파일 3개

        public int currentSaveIndex { get; private set; } = 0;

        public GameData CurrentSave => saveFiles[currentSaveIndex]; // 세이브 파일 인덱스로 배열

        public bool isGameOver { get; private set; } // 게임 오버
        public bool isPaused { get; private set; } // 게임 일시 정지

        public bool isGameCleared { get; private set; } // 게임 클리어

        public int selectWorldIndex = 0;
        public int selectStageIndex = 0;

        public Dictionary<EquipmentType, EquipmentDataSO> equippedDict = new(); // 장착된 장비 딕셔너리 (슬롯 타입 -> 장비 데이터)

        // Removed duplicate declaration of 'equippedDict' here.
        // public Dictionary<KYG_skyPower.EquipmentType, KYG_skyPower.EquipmentDataSO> equippedDict = new(); 

        public override void Init() // 게임 시작시 세이브 데이터 로드  
        {
            ResetSaveRef(); // 세이브 데이터 초기화  

            // 수정된 코드: equipmentInventory를 GameData에서 적절한 형식으로 캐스팅하여 사용  
            if (CurrentSave.equipmentInventory is EquipmentInventory inventory)
            {
                equipmentManagerSO.savedEquipments = inventory.equipments; // 현재 세이브 파일의 장비 정보 로드  
                equipmentManagerSO.BuildRuntimeInventory(); // 런타임 인벤토리 갱신  
            }
            else
            {
                Debug.LogError("equipmentInventory가 EquipmentInventory 형식이 아닙니다.");
            }
        }

        public void ResetSaveRef()
        {
            for (int i = 0; i < saveFiles.Length; i++) // 세이브 파일 초기화
            {
                saveFiles[i] = new GameData(); // 새 GameData 객체 생성
                SaveManager.Instance.GameLoad(ref saveFiles[i], i + 1); // 인덱스 1부터
            }
        }

        public void EquipToCharacter(int equipId, int charId)
        {
            equipmentManagerSO.Equip(equipId, charId);
            // TODO: 캐릭터 능력치, 상태 등 추가 반영
        }

        public void UnequipFromCharacter(int equipId)
        {
            equipmentManagerSO.Unequip(equipId);
            // TODO: 캐릭터 능력치, 상태 등 추가 반영
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
            isGameOver = true; // 게임 오버가 true면
            Time.timeScale = 0f; // 시간 정지 기능
            onGameOver?.Invoke();
            Debug.Log("게임 오버");
        }

        public void SetGameClear()
        {
            if (isGameCleared || isGameOver) return;
            isGameCleared = true;
            Time.timeScale = 0f;
            onGameClear?.Invoke();
            Debug.Log("게임 클리어");
        }

        public void PausedGame()
        {
            if (isPaused || isGameOver) return;
            isPaused = true;
            Time.timeScale = 0f; // 전체 게임 정지
            onPause?.Invoke();
            Debug.Log("일시 정지");
        }

        public void ResumeGame()
        {
            if (!isPaused || isGameOver) return;
            isPaused = false;
            Time.timeScale = 1f; // 게임 시간 정상화
            onResume?.Invoke();
            Debug.Log("게임 재개");
        }

        public void ResetStageIndex()
        {
            selectWorldIndex = 0;
            selectStageIndex = 0;
        }
    }
}

