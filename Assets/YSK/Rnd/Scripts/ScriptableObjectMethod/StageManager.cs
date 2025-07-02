using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSK;
using System.Collections;
using UnityEngine.UI;
using KYG_skyPower;

namespace YSK
{
    public class StageManager : MonoBehaviour
    {
        [Header("Stage Data")]
        [SerializeField] private List<StageData> stageDataList;
        [SerializeField] private int maxMainStages = 4;
        [SerializeField] private int maxSubStages = 5;
        [SerializeField] private StageDataManager dataManager;
        
        private StageData currentStage;

        [Header("Map System")]
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform startPoint;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float mapLength = 20;
        
        private List<GameObject> spawnedMaps = new();
        private List<GameObject> movingMaps = new();

        [Header("Transition Settings")]
        [SerializeField] private bool useGameSceneManagerTransition = true;
        [SerializeField] private bool enableTransition = true;
        
        private bool isTransitioning = false;

        #region Unity Lifecycle

        private void Awake()
        {
            // �ּ����� �ʱ�ȭ��
        }

        private void Start()
        {
            Debug.Log("=== StageManager Start ���� ===");
            InitializeComponents();
            Debug.Log("=== StageManager Start �Ϸ� ===");
        }

        private void Update()
        {
            UpdateMovingMaps();
        }

        #endregion

        #region Initialization

        private void InitializeComponents()
        {
            FindTransformPoints();
            FindDataManager();
        }

        private void FindTransformPoints()
        {
            if (startPoint == null)
            {
                startPoint = transform.Find("StartPoint");
                if (startPoint == null)
                {
                    Debug.LogWarning("StartPoint�� ã�� �� �����ϴ�!");
                }
            }

            if (endPoint == null)
            {
                endPoint = transform.Find("EndPoint");
                if (endPoint == null)
                {
                    Debug.LogWarning("EndPoint�� ã�� �� �����ϴ�!");
                }
            }
        }

        private void FindDataManager()
        {
            if (dataManager == null)
            {
                dataManager = FindObjectOfType<StageDataManager>();
                if (dataManager == null)
                {
                    Debug.LogWarning("StageDataManager�� ã�� �� �����ϴ�!");
                }
            }
        }

        #endregion

        #region Map Management

        private void LoadStage(int mainStageID, int subStageID = 1)
        {
            Debug.Log($"=== LoadStage ����: {mainStageID}-{subStageID} ===");

            ClearAllMaps();
            UpdatePlayerPrefs(mainStageID, subStageID);

            if (!ValidateStageData(mainStageID))
                return;

            currentStage = stageDataList.Find(data => data.stageID == mainStageID);
            SpawnMaps();
            
            Debug.Log($"=== LoadStage �Ϸ�: {mainStageID}-{subStageID} ===");
        }

        private bool ValidateStageData(int mainStageID)
        {
            if (stageDataList == null || stageDataList.Count == 0)
            {
                Debug.LogError("�������� ������ ����Ʈ�� ����ֽ��ϴ�!");
                return false;
            }

            if (!stageDataList.Exists(data => data.stageID == mainStageID))
            {
                Debug.LogError($"Main Stage ID {mainStageID} not found!");
                return false;
            }

            return true;
        }

        private void UpdatePlayerPrefs(int mainStageID, int subStageID)
        {
            PlayerPrefs.SetInt("SelectedMainStage", mainStageID);
            PlayerPrefs.SetInt("SelectedSubStage", subStageID);
            PlayerPrefs.Save();
        }

        private void SpawnMaps()
        {
            spawnedMaps.Clear();
            movingMaps.Clear();

            if (currentStage == null)
            {
                Debug.LogError("currentStage�� null�Դϴ�!");
                return;
            }

            int subStageID = PlayerPrefs.GetInt("SelectedSubStage", 1);
            List<GameObject> mapPrefabsToUse = GetMapPrefabsForSubStage(subStageID);

            if (mapPrefabsToUse == null || mapPrefabsToUse.Count == 0)
            {
                Debug.LogWarning($"�������� {currentStage.stageID}�� �� �������� ����ֽ��ϴ�!");
                return;
            }

            Debug.Log($"�� �� ���� ����: {mapPrefabsToUse.Count}�� ������ (���� �������� {subStageID})");
            
            for (int i = 0; i < mapPrefabsToUse.Count; i++)
            {
                if (mapPrefabsToUse[i] == null)
                {
                    Debug.LogWarning($"�� ������ {i}���� null�Դϴ�!");
                    continue;
                }

                GameObject map = Instantiate(mapPrefabsToUse[i]);
                Vector3 spawnPosition = startPoint != null 
                    ? startPoint.position + Vector3.back * (mapLength * i)
                    : Vector3.back * (mapLength * i);
                
                map.transform.position = spawnPosition;
                spawnedMaps.Add(map);
                movingMaps.Add(map);
            }

            Debug.Log($"SpawnMaps �Ϸ�: �� {spawnedMaps.Count}�� �� ����");
        }

        private List<GameObject> GetMapPrefabsForSubStage(int subStageID)
        {
            List<GameObject> mapPrefabsToUse = currentStage.mapPrefabs;

            if (currentStage.subStages != null && currentStage.subStages.Count > 0)
            {
                SubStageData subStageData = currentStage.subStages.Find(s => s.subStageID == subStageID);
                if (subStageData != null && subStageData.customMapPrefabs != null && subStageData.customMapPrefabs.Count > 0)
                {
                    mapPrefabsToUse = subStageData.customMapPrefabs;
                    Debug.Log($"���� �������� {subStageID} Ŀ���� �� ���");
                }
            }

            return mapPrefabsToUse;
        }

        private void UpdateMovingMaps()
        {
            if (movingMaps == null || movingMaps.Count == 0)
                return;

            foreach (var map in movingMaps)
            {
                if (map != null)
                {
                    MoveMap(map);
                }
            }
        }

        private void MoveMap(GameObject map)
        {
            if (map == null) return;

            map.transform.position += Vector3.back * speed * Time.deltaTime;

            if (endPoint == null)
            {
                Debug.LogWarning("endPoint�� null�Դϴ�!");
                return;
            }

            if (map.transform.position.z < endPoint.position.z)
            {
                float maxZ = GetMaxZPosition();
                map.transform.position = new Vector3(map.transform.position.x, map.transform.position.y, maxZ + mapLength);
            }
        }

        private float GetMaxZPosition()
        {
            float maxZ = float.MinValue;
            foreach (var map in movingMaps)
            {
                if (map != null && map.transform.position.z > maxZ)
                    maxZ = map.transform.position.z;
            }
            return maxZ;
        }

        public void ClearAllMaps()
        {
            Debug.Log($"ClearAllMaps ����: {spawnedMaps.Count}�� �� ����");

            foreach (var map in spawnedMaps)
            {
                if (map != null)
                {
                    Destroy(map);
                }
            }
            spawnedMaps.Clear();
            movingMaps.Clear();

            Debug.Log("ClearAllMaps �Ϸ�");
        }

        #endregion

        #region Stage Transition

        public void StartStageTransition(int mainStageID, int subStageID = 1, bool isGameStart = false)
        {
            Debug.Log($"StageManager.StartStageTransition: {mainStageID}-{subStageID}, ���ӽ���: {isGameStart}");
            
            if (!enableTransition)
            {
                LoadStage(mainStageID, subStageID);
                return;
            }
            
            if (!isTransitioning)
            {
                StartCoroutine(TransitionCoroutine(mainStageID, subStageID, isGameStart));
            }
            else
            {
                Debug.LogWarning("�̹� ��ȯ ���Դϴ�!");
            }
        }

        public void StartStageTransitionOnlyFadeIn(int mainStageID, int subStageID = 1, bool isGameStart = false)
        {
            Debug.Log($"StageManager.StartStageTransitionOnlyFadeIn: {mainStageID}-{subStageID}, ���ӽ���: {isGameStart}");

            if (!enableTransition)
            {
                LoadStage(mainStageID, subStageID);
                return;
            }

            if (!isTransitioning)
            {
                StartCoroutine(TransitionWithFadeInCoroutine(mainStageID, subStageID, isGameStart));
            }
            else
            {
                Debug.LogWarning("�̹� ��ȯ ���Դϴ�!");
            }
        }

        private IEnumerator TransitionCoroutine(int mainStageID, int subStageID, bool isGameStart)
        {
            Debug.Log($"TransitionCoroutine ����: {mainStageID}-{subStageID}");
            isTransitioning = true;
            
            if (isGameStart)
            {
                LoadStage(mainStageID, subStageID);
            }
            else
            {
                if (enableTransition && useGameSceneManagerTransition && Manager.GSM != null)
                {
                    // GameSceneManager�� ��ȯ ȭ�� ���
                    Manager.GSM.ShowTransitionScreen();
                    yield return new WaitForSeconds(0.5f); // ��ȯ ȭ�� ǥ�� �ð�
                    
                    LoadStage(mainStageID, subStageID);
                    
                    yield return new WaitForSeconds(0.1f); // �������� �ε� ���
                    Manager.GSM.HideTransitionScreen();
                }
                else
                {
                    LoadStage(mainStageID, subStageID);
                }
            }
            
            isTransitioning = false;
            Debug.Log("TransitionCoroutine �Ϸ�");
        }

        private IEnumerator TransitionWithFadeInCoroutine(int mainStageID, int subStageID, bool isGameStart)
        {
            Debug.Log($"TransitionWithFadeInCoroutine ����: {mainStageID}-{subStageID}");
            isTransitioning = true;

            if (isGameStart)
            {
                LoadStage(mainStageID, subStageID);
            }
            else
            {
                if (enableTransition && useGameSceneManagerTransition && Manager.GSM != null)
                {
                    // GameSceneManager�� ��ȯ ȭ���� ���������� ����
                    Manager.GSM.SetTransitionText("�������� ��ȯ ��...");
                    Manager.GSM.ShowTransitionScreen();
                    
                    LoadStage(mainStageID, subStageID);
                    yield return new WaitForSeconds(0.1f);
                    
                    Manager.GSM.HideTransitionScreen();
                }
                else
                {
                    LoadStage(mainStageID, subStageID);
                }
            }

            isTransitioning = false;
            Debug.Log("TransitionWithFadeInCoroutine �Ϸ�");
        }

        #endregion

        #region Stage Progression

        public void ClearCurrentStageAndNext()
        {
            int currentMainStage = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int currentSubStage = PlayerPrefs.GetInt("SelectedSubStage", 1);
            
            Debug.Log($"�������� Ŭ����: {currentMainStage}-{currentSubStage}");
            
            var nextStage = CalculateNextStage(currentMainStage, currentSubStage);
            
            if (nextStage.isGameComplete)
            {
                Debug.Log("��� �������� Ŭ����! ���� Ŭ���� ó��");
                OnGameComplete();
                return;
            }
            
            LoadStage(nextStage.mainStage, nextStage.subStage);
            Debug.Log($"���� ���������� ��ȯ: {nextStage.mainStage}-{nextStage.subStage}");
        }

        public void ClearCurrentStageAndNextWithTransition()
        {
            int currentMainStage = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int currentSubStage = PlayerPrefs.GetInt("SelectedSubStage", 1);
            
            Debug.Log($"�������� Ŭ���� (��ȯ ȭ�� ���): {currentMainStage}-{currentSubStage}");
            
            var nextStage = CalculateNextStage(currentMainStage, currentSubStage);
            
            if (nextStage.isGameComplete)
            {
                Debug.Log("��� �������� Ŭ����! ���� Ŭ���� ó��");
                OnGameComplete();
                return;
            }
            
            StartStageTransition(nextStage.mainStage, nextStage.subStage, false);
            Debug.Log($"���� ���������� ��ȯ (��ȯ ȭ��): {nextStage.mainStage}-{nextStage.subStage}");
        }

        private (int mainStage, int subStage, bool isGameComplete) CalculateNextStage(int currentMainStage, int currentSubStage)
        {
            int nextSubStage = currentSubStage + 1;
            
            if (nextSubStage > maxSubStages)
            {
                int nextMainStage = currentMainStage + 1;
                
                if (nextMainStage > maxMainStages)
                {
                    return (0, 0, true); // ���� Ŭ����
                }
                
                return (nextMainStage, 1, false);
            }
            else
            {
                return (currentMainStage, nextSubStage, false);
            }
        }

        private void OnGameComplete()
        {
            Debug.Log("���� Ŭ����!");
            
            if (Manager.GSM != null)
            {
                // ��� ȭ������ �̵�
            }
        }

        #endregion

        #region Public API

        public void OnStageButtonClick(int mainStageID, int subStageID = 1)
        {
            if (!isTransitioning)
            {
                StartStageTransition(mainStageID, subStageID, false);
            }
        }

        public void ForceStage(int mainStageID, int subStageID)
        {
            Debug.Log($"���� �������� �̵�: {mainStageID}-{subStageID}");
            LoadStage(mainStageID, subStageID);
        }

        public void ForceStageWithTransition(int mainStageID, int subStageID)
        {
            Debug.Log($"���� �������� �̵� (��ȯ ȭ�� ���): {mainStageID}-{subStageID}");
            StartStageTransition(mainStageID, subStageID, false);
        }

        public void OnStageCompleted(int score = 0)
        {
            int currentMainStage = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int currentSubStage = PlayerPrefs.GetInt("SelectedSubStage", 1);
            
            Debug.Log($"�������� �Ϸ�: {currentMainStage}-{currentSubStage}, ����: {score}");
            
            if (dataManager != null)
            {
                dataManager.UpdateStageScore(currentMainStage, currentSubStage, score);
                dataManager.CompleteStage(currentMainStage, currentSubStage, Time.time);
                UnlockNextStage(currentMainStage, currentSubStage);
            }
            
            if (Manager.Game != null)
            {
                Manager.Game.SetGameClear();
            }
        }

        public void ResetStageProgress()
        {
            PlayerPrefs.SetInt("SelectedMainStage", 1);
            PlayerPrefs.SetInt("SelectedSubStage", 1);
            PlayerPrefs.Save();
            LoadStage(1, 1);
            Debug.Log("�������� ���� ���� �ʱ�ȭ: 1-1");
        }

        public void ChangeStage(int newStageID)
        {
            Debug.Log($"ChangeStage ȣ��: �������� {newStageID}�� ����");
            LoadStage(newStageID);
        }

        #endregion

        #region Utility Methods

        public StageData GetStageData(int stageID)
        {
            return stageDataList.Find(data => data.stageID == stageID);
        }

        public int GetCurrentSubStageID()
        {
            return PlayerPrefs.GetInt("SelectedSubStage", 1);
        }

        public SubStageData GetCurrentSubStageData()
        {
            if (currentStage == null) return null;

            int subStageID = GetCurrentSubStageID();
            return currentStage.subStages?.Find(s => s.subStageID == subStageID);
        }

        public bool IsTransitioning => isTransitioning;

        public void SetTransitionEnabled(bool enabled)
        {
            enableTransition = enabled;
        }

        public void SetUseGameSceneManagerTransition(bool use)
        {
            useGameSceneManagerTransition = use;
        }

        private void UnlockNextStage(int currentMainStage, int currentSubStage)
        {
            if (dataManager == null) return;
            
            var nextStage = CalculateNextStage(currentMainStage, currentSubStage);
            
            if (nextStage.isGameComplete)
            {
                Debug.Log("��� �������� Ŭ����!");
                return;
            }
            
            dataManager.UnlockStage(nextStage.mainStage);
            dataManager.UnlockSubStage(nextStage.mainStage, nextStage.subStage);
            
            Debug.Log($"���� �������� �ر�: {nextStage.mainStage}-{nextStage.subStage}");
        }

        private bool CanLoadStage(int mainStageID, int subStageID)
        {
            if (dataManager == null) return true;
            
            return dataManager.IsStageUnlocked(mainStageID) && 
                   dataManager.IsSubStageUnlocked(mainStageID, subStageID);
        }

        #endregion
    }
}
