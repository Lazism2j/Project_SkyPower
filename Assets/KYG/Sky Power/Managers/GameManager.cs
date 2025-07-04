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

    /*
    
    싱글톤 기반 게임 매니저

    필요 기능
    게임 스코어
    게임 오버 여부
    게임 일시정지,게임 재개 기능

    추가 기능
    이벤트 기반 코드로 확장성 고려
    세이브 테이터를 배열로 게임 매니저가 가지고 와야 한다
    게임 시작시 세이브 데이터 가지고 와서 가지고 있는다

    */

    
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

        public int selectWorldIndex=0;
        public int selectStageIndex=0;

        //[SerializeField] private int defaultPlayerHP = 5;
        //public int playerHp { get; private set; } // 플레이어에 붙을 수도 있지만 나중에 추가 될지 몰라 주석 처리

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

        public Dictionary<KYG_skyPower.EquipmentType, EquipmentDataSO> equippedDict = new(); // 장착된 장비 딕셔너리 (슬롯 타입 -> 장비 데이터)

        public void EquipToCharacter(int equipId, int charId) // 캐릭터에 장비 장착
        {
            equipmentManagerSO.Equip(equipId, charId);
            // 캐릭터 상태, 능력치 등 추가로 갱신
        }

        public void UnequipFromCharacter(int equipId) // 캐릭터에서 장비 해제
        {
            equipmentManagerSO.Unequip(equipId); // 장비 해제
        }

        public void AddEquipmentToInventory(int equipId, EquipmentType slotType) // 인벤토리에 장비 추가
        {
            equipmentManagerSO.AddEquipment(equipId, slotType); // 장비 추가
        }

        public void SelectSaveFile(int index)
        {
            if (index >= 0 && index < saveFiles.Length)
                currentSaveIndex = index;

            
            equipmentManagerSO.BuildRuntimeInventory(); // 현재 세이브 파일의 장비 정보 로드
        }

        /*private void Awake() // 싱글톤 패턴
        {
            if (Instance != null && Instance != this) // 만약 다른 Instance 있으면 본 Instance
            {
                Destroy(gameObject); // 삭제
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject); // 게임 오브젝트 파괴되지 않게 제한

        }*/



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
        /*private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if(isPaused) ResumeGame();
                else PausedGame();
            }
        }*/ // ESC 키입력으로 일시정지 기능 예시로 작성
    }
}

