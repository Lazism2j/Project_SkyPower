using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSK;
using System.Collections;

namespace YSK
{
    public class StageManager : MonoBehaviour
    {
        [Header("StageData")]
        [SerializeField] private List<StageData> stageDataList; // ��� ���������� ������
        private StageData currentStage;
        private List<GameObject> MapPrefabs;
        [SerializeField] int selectedstageID; // Test�� Stage ID�� ��� �ܺ� ���ÿ� ���ؼ� �������� �Ǵ� ������ �����ؾ���.

        [Header("MoveInfo")]
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform startPoint;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float mapLength = 20;

        [Header("References")]
        [SerializeField] private GameStateManager gameStateManager;

        private List<GameObject> spawnedMaps = new(); // �������� �̿��� Stage Map ����

        private List<GameObject> movingMaps = new(); // ���� �̵����� ��.
        private StageTransition stageControl;

        #region Unity Lifecycle

        private void Awake()
        {
            // Awake������ �ּ����� �ʱ�ȭ��
        }

        private void Start()
        {
            Debug.Log("=== StageManager Start ���� ===");
            
            InitializeComponents();
            
            // GameStateManager�� �غ�� ������ ���
            StartCoroutine(WaitForGameStateManager());
            
            Debug.Log("=== StageManager Start �Ϸ� ===");
        }

        private IEnumerator WaitForGameStateManager()
        {
            Debug.Log("=== WaitForGameStateManager ���� ===");
            
            int waitCount = 0;
            // GameStateManager�� �غ�� ������ ���
            while (GameStateManager.Instance == null)
            {
                waitCount++;
                if (waitCount % 60 == 0) // 1�ʸ��� �α�
                {
                    Debug.Log($"GameStateManager ��� ��... ({waitCount}������)");
                }
                yield return null;
            }
            
            Debug.Log($"GameStateManager �߰�! ({waitCount}������ ���)");
            
            // �̺�Ʈ ����
            GameStateManager.OnStageChanged += OnStageChanged;
            GameStateManager.OnGameStateChanged += OnGameStateChanged;
            
            Debug.Log("StageManager �̺�Ʈ ���� �Ϸ�");
            
            // GameStateManager�� �̹� Playing ���¶�� ������ �� ���� Ʈ����
            if (GameStateManager.Instance.CurrentGameState == GameState.Playing)
            {
                Debug.Log("GameStateManager�� �̹� Playing �����Դϴ�. �� ���� Ʈ����");
                OnGameStateChanged(GameState.Playing);
            }
            
            Debug.Log("=== WaitForGameStateManager �Ϸ� ===");
        }

        private void Update()
        {
            UpdateMovingMaps();
            CheckInput();
        }

        private void OnDestroy()
        {
            // �̺�Ʈ ���� ����
            GameStateManager.OnStageChanged -= OnStageChanged;
            GameStateManager.OnGameStateChanged -= OnGameStateChanged;
        }

        #endregion

        /// <summary>
        /// GameStateManager���� �������� ���� �̺�Ʈ�� �޾��� �� ȣ��˴ϴ�.
        /// </summary>
        private void OnStageChanged(int newStageID)
        {
            Debug.Log($"GameStateManager���� �������� ���� ��û: {newStageID}");
            LoadStage(newStageID);
        }

        /// <summary>
        /// GameStateManager���� ���� ���� ���� �̺�Ʈ�� �޾��� �� ȣ��˴ϴ�.
        /// </summary>
        private void OnGameStateChanged(GameState newState)
        {
            Debug.Log($"=== OnGameStateChanged ȣ��: {newState} ===");

            switch (newState)
            {
                case GameState.Playing:
                    Debug.Log($"Playing ���� ó�� ���� - spawnedMaps.Count: {spawnedMaps.Count}");
                    
                    // �̹� ���������� �ε�Ǿ� ���� ���� ��쿡�� �ε�
                    if (spawnedMaps.Count == 0)
                    {
                        Debug.Log("���� ����: �������� �ε�");
                        // PlayerPrefs���� ���� �������� ���� ��������
                        int mainStageID = PlayerPrefs.GetInt("SelectedMainStage", 1);
                        Debug.Log($"PlayerPrefs���� ������ ���� ��������: {mainStageID}");
                        LoadStage(mainStageID);
                    }
                    else
                    {
                        Debug.Log($"���� ����: �̹� ���������� �ε�� (�� ����: {spawnedMaps.Count})");
                    }
                    break;

                case GameState.MainMenu:
                case GameState.StageSelect:
                    // ���θ޴��� �������� ���� �ÿ��� ���� �������� ����
                    Debug.Log("���θ޴�/�������� ����: ���� �� ����");
                    ClearAllMaps();
                    break;
            }
            
            Debug.Log($"=== OnGameStateChanged �Ϸ�: {newState} ===");
        }

        /// <summary>
        /// �ʿ��� ������Ʈ���� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void InitializeComponents()
        {
            FindStageTransition();
            FindTransformPoints();
            FindGameStateManager();
        }

        /// <summary>
        /// GameStateManager�� ã���ϴ�.
        /// </summary>
        private void FindGameStateManager()
        {
            if (gameStateManager == null)
            {
                gameStateManager = GameStateManager.Instance;
                if (gameStateManager == null)
                {
                    Debug.LogWarning("GameStateManager�� ã�� �� �����ϴ�! (�� ��ȯ ���̰ų� ���� �ʱ�ȭ���� �ʾ��� �� �ֽ��ϴ�)");
                }
            }
        }

        /// <summary>
        /// �ڽ� ������Ʈ���� StageTransition�� ã���ϴ�.
        /// </summary>
        private void FindStageTransition()
        {
            stageControl = GetComponentInChildren<StageTransition>();

            if (stageControl == null)
            {
                Debug.LogWarning("StageTransition ������Ʈ�� �ڽ� ������Ʈ���� ã�� �� �����ϴ�! (�ش� �������� ���Ǵ� ������Ʈ�Դϴ�)");
            }
        }

        /// <summary>
        /// �ڽ� ������Ʈ���� StartPoint�� EndPoint�� ã���ϴ�.
        /// </summary>
        private void FindTransformPoints()
        {
            if (startPoint == null)
            {
                startPoint = transform.Find("StartPoint");
                if (startPoint == null)
                {
                    Debug.LogWarning("StartPoint�� ã�� �� �����ϴ�! (������ �����ؾ� �մϴ�)");
                }
            }

            if (endPoint == null)
            {
                endPoint = transform.Find("EndPoint");
                if (endPoint == null)
                {
                    Debug.LogWarning("EndPoint�� ã�� �� �����ϴ�! (������ �����ؾ� �մϴ�)");
                }
            }
        }

        private void CheckInput()
        {
            if (Input.anyKeyDown)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1)) HandleKey(1);
                else if (Input.GetKeyDown(KeyCode.Alpha2)) HandleKey(2);
                else if (Input.GetKeyDown(KeyCode.Alpha3)) HandleKey(3);
                else if (Input.GetKeyDown(KeyCode.Alpha4)) HandleKey(4);
                else if (Input.GetKeyDown(KeyCode.Alpha5)) HandleKey(5);
            }
        }

        private void HandleKey(int keyNumber)
        {
            switch (keyNumber)
            {
                case 1:
                    Debug.Log("1�� Ű: �������� Ŭ���� �� ���� ����������");
                    ClearCurrentStageAndNext();
                    break;
                case 2:
                    Debug.Log("2�� Ű: ���� �� ���� �׽�Ʈ");
                    ForceTestMapGeneration();
                    break;
                case 3:
                    Debug.Log("3�� Ű: �������� ���� ���� �ʱ�ȭ");
                    ResetStageProgress();
                    break;
                case 4:
                    Debug.Log("4�� Ű: 2-3���� ���� �̵�");
                    ForceStage(2, 3);
                    break;
                case 5:
                    Debug.Log("5�� Ű: ���� ���� ���� ���");
                    DebugCurrentState();
                    break;
                default:
                    Debug.LogWarning("�� �� ���� Ű �Է�");
                    break;
            }
        }

        private void LoadStage(int mainStageID, int subStageID = 1)
        {
            Debug.Log($"=== LoadStage ����: ���� �������� {mainStageID}, ���� �������� {subStageID} ===");

            // ���� �� ����
            ClearAllMaps();

            // PlayerPrefs ������Ʈ
            PlayerPrefs.SetInt("SelectedMainStage", mainStageID);
            PlayerPrefs.SetInt("SelectedSubStage", subStageID);
            PlayerPrefs.Save();

            Debug.Log($"�������� ���� - ����: {mainStageID}, ����: {subStageID}");

            // �������� ������ ����Ʈ�� ����ִ��� Ȯ��
            if (stageDataList == null || stageDataList.Count == 0)
            {
                Debug.LogError("�������� ������ ����Ʈ�� ����ֽ��ϴ�!");
                Debug.LogError("StageManager�� Inspector���� StageDataList�� StageData ���µ��� �߰����ּ���!");
                return;
            }

            Debug.Log($"��� ������ ��������: {string.Join(", ", stageDataList.Select(s => s.stageID))}");

            // ���� �������� ID�� �ش� �������� ������ ã��
            currentStage = stageDataList.Find(data => data.stageID == mainStageID);

            if (currentStage == null)
            {
                Debug.LogError($"Main Stage ID {mainStageID} not found!");
                Debug.LogError($"��� ������ ��������: {string.Join(", ", stageDataList.Select(s => s.stageID))}");
                return;
            }

            Debug.Log($"���� �������� {mainStageID} ������ �ε� �Ϸ�, ���� �������� {subStageID} �� ���� ����");
            SpawnMaps();
            
            Debug.Log($"=== LoadStage �Ϸ�: ���� �������� {mainStageID}, ���� �������� {subStageID} ===");
        }

        private void SpawnMaps()
        {
            Debug.Log($"SpawnMaps ����: ���� �� {spawnedMaps.Count}�� ����");
            spawnedMaps.Clear();
            movingMaps.Clear();

            // currentStage�� null���� Ȯ��
            if (currentStage == null)
            {
                Debug.LogError("currentStage�� null�Դϴ�! LoadStage�� ���� ȣ�����ּ���.");
                return;
            }

            // PlayerPrefs���� ���� �������� ���� ��������
            int subStageID = PlayerPrefs.GetInt("SelectedSubStage", 1);

            // ����� �� ������ ����Ʈ ����
            List<GameObject> mapPrefabsToUse = currentStage.mapPrefabs;

            // ���� ���������� Ŀ���� ���� �ִ��� Ȯ��
            if (currentStage.subStages != null && currentStage.subStages.Count > 0)
            {
                SubStageData subStageData = currentStage.subStages.Find(s => s.subStageID == subStageID);
                if (subStageData != null && subStageData.customMapPrefabs != null && subStageData.customMapPrefabs.Count > 0)
                {
                    mapPrefabsToUse = subStageData.customMapPrefabs;
                    Debug.Log($"���� �������� {subStageID} ���� �� ���");
                }
                else
                {
                    Debug.Log($"���� �������� {subStageID}�� �⺻ �� ���");
                }
            }

            // �� ������ ����Ʈ�� ����ִ��� Ȯ��
            if (mapPrefabsToUse == null || mapPrefabsToUse.Count == 0)
            {
                Debug.LogWarning($"�������� {currentStage.stageID}�� �� �������� ����ֽ��ϴ�!");
                return;
            }

            Debug.Log($"�� �� ���� ����: {mapPrefabsToUse.Count}�� ������ (���� �������� {subStageID})");
            for (int i = 0; i < mapPrefabsToUse.Count; i++)
            {
                // �������� null���� Ȯ��
                if (mapPrefabsToUse[i] == null)
                {
                    Debug.LogWarning($"�������� {currentStage.stageID}�� �� ������ {i}���� null�Դϴ�!");
                    continue;
                }

                GameObject map = Instantiate(mapPrefabsToUse[i]);

                // startPoint�� null���� Ȯ��
                if (startPoint == null)
                {
                    Debug.LogWarning("startPoint�� null�Դϴ�! ���� �⺻ ��ġ�� �����մϴ�.");
                    map.transform.position = Vector3.back * (mapLength * i);
                }
                else
                {
                    // ���� �Ϸķ� ��ġ (��: Z�� ����)
                    map.transform.position = startPoint.position + Vector3.back * (mapLength * i);
                }

                spawnedMaps.Add(map);
                movingMaps.Add(map);
                Debug.Log($"�� {i + 1} ���� �Ϸ�: {map.name}");
            }

            Debug.Log($"SpawnMaps �Ϸ�: �� {spawnedMaps.Count}�� �� ���� (���� �������� {subStageID})");
        }

        private void MoveMap(GameObject map)
        {
            if (map == null) return;

            map.transform.position += Vector3.back * speed * Time.deltaTime;

            // endPoint�� null���� Ȯ��
            if (endPoint == null)
            {
                Debug.LogWarning("endPoint�� null�Դϴ�! �� �̵��� �ߴ��մϴ�.");
                return;
            }

            // endPoint�� ������ startPoint�� ���ġ
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

        private void UpdateMovingMaps()
        {
            // movingMaps�� null�̰ų� ����ִ��� Ȯ��
            if (movingMaps == null || movingMaps.Count == 0)
            {
                return;
            }

            foreach (var map in movingMaps)
            {
                if (map != null)
                {
                    MoveMap(map);
                }
            }
        }

        public void ChangeStage(int newStageID)
        {
            Debug.Log($"ChangeStage ȣ��: ���������� {newStageID}�� ����");

            // LoadStage���� �̹� ClearAllMaps�� ȣ���ϹǷ� ���⼭�� ����
            // ���� ���������� �� ���������� ���� ��ü
            LoadStage(newStageID);

            // PlayerPrefs���� ���� ���������� ���� �������� ���� ��������
            int mainStageID = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int subStageID = PlayerPrefs.GetInt("SelectedSubStage", 1);

            // UI �ؽ�Ʈ ������Ʈ - ����-���� ���·� ǥ��
            if (UIFactory.Instance != null)
            {
                UIFactory.Instance.UpdateStageText($"{mainStageID}-{subStageID}");
            }
            else
            {
                Debug.LogWarning("UIFactory.Instance�� null�Դϴ�. UIFactory�� ã�ƺ��ڽ��ϴ�.");
                UIFactory uiFactory = FindObjectOfType<UIFactory>();
                if (uiFactory != null)
                {
                    uiFactory.UpdateStageText($"{mainStageID}-{subStageID}");
                }
                else
                {
                    Debug.LogWarning("UIFactory�� ã�� �� �����ϴ�! UI �ؽ�Ʈ ������Ʈ�� �ǳʶݴϴ�.");
                }
            }

            // 3. �÷��̾�/ī�޶� ��ġ �ʱ�ȭ
            // player.transform.position = ...;
            // camera.transform.position = ...;

            // 4. UI �� ���� ���� �ʱ�ȭ
            // UpdateUI();
            // SetGameState(GameState.Ready);
        }

        /// <summary>
        /// Ư�� ���������� ID�� StageData�� ��ȯ�մϴ�.
        /// </summary>
        /// <param name="stageID">ã�� ���������� ID</param>
        /// <returns>�ش� ���������� ������ �Ǵ� null</returns>
        public StageData GetStageData(int stageID)
        {
            return stageDataList.Find(data => data.stageID == stageID);
        }

        /// <summary>
        /// ���� ���������� �����͸� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� ���������� ������</returns>
        public StageData GetCurrentStageData()
        {
            return currentStage;
        }

        /// <summary>
        /// ���� ������ �� ������Ʈ ����Ʈ�� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>������ �� ����Ʈ</returns>
        public List<GameObject> GetSpawnedMaps()
        {
            return new List<GameObject>(spawnedMaps);
        }

        /// <summary>
        /// ���� �̵� ���� �� ������Ʈ ����Ʈ�� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>�̵� ���� �� ����Ʈ</returns>
        public List<GameObject> GetMovingMaps()
        {
            return new List<GameObject>(movingMaps);
        }

        /// <summary>
        /// Ư�� ���� �����մϴ�.
        /// </summary>
        /// <param name="map">������ �� ������Ʈ</param>
        public void RemoveMap(GameObject map)
        {
            if (spawnedMaps.Contains(map))
            {
                spawnedMaps.Remove(map);
                movingMaps.Remove(map);
                Destroy(map);
            }
        }

        /// <summary>
        /// ��� ���� �����մϴ�.
        /// </summary>
        public void ClearAllMaps()
        {
            Debug.Log($"ClearAllMaps ����: {spawnedMaps.Count}�� �� ����");

            foreach (var map in spawnedMaps)
            {
                if (map != null)
                {
                    Debug.Log($"�� ����: {map.name}");
                    Destroy(map);
                }
            }
            spawnedMaps.Clear();
            movingMaps.Clear();

            Debug.Log("ClearAllMaps �Ϸ�: ��� �� ���ŵ�");
        }

        /// <summary>
        /// �� �̵��� �Ͻ������մϴ�.
        /// </summary>
        public void PauseMapMovement()
        {
            enabled = false;
        }

        /// <summary>
        /// �� �̵��� �簳�մϴ�.
        /// </summary>
        public void ResumeMapMovement()
        {
            enabled = true;
        }

        /// <summary>
        /// ���� ���õ� ���� �������� ID�� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� �������� ID</returns>
        public int GetCurrentMainStageID()
        {
            return PlayerPrefs.GetInt("SelectedMainStage", 1);
        }

        /// <summary>
        /// ���� ���õ� ���� �������� ID�� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� �������� ID</returns>
        public int GetCurrentSubStageID()
        {
            return PlayerPrefs.GetInt("SelectedSubStage", 1);
        }

        /// <summary>
        /// ���� ���� ���������� �����͸� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� �������� ������ �Ǵ� null</returns>
        public SubStageData GetCurrentSubStageData()
        {
            if (currentStage == null) return null;

            int subStageID = GetCurrentSubStageID();
            return currentStage.subStages?.Find(s => s.subStageID == subStageID);
        }

        /// <summary>
        /// ���� ���������� ���̵��� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���̵� (1-5)</returns>
        public float GetCurrentDifficulty()
        {
            if (currentStage == null) return 1f;

            int subStageID = GetCurrentSubStageID();
            SubStageData subStageData = GetCurrentSubStageData();

            // ���� ���������� Ŀ���� ���̵��� ������ ���
            if (subStageData != null && subStageData.customDifficulty > 0)
            {
                return subStageData.customDifficulty;
            }

            // �⺻ ���̵� + ���� ���������� ������
            return currentStage.baseDifficulty + (subStageID - 1) * currentStage.difficultyIncreasePerSubStage;
        }

        /// <summary>
        /// GameStateManager ������ �����մϴ�.
        /// </summary>
        /// <param name="newGameStateManager">������ GameStateManager �ν��Ͻ�</param>
        public void SetGameStateManager(GameStateManager newGameStateManager)
        {
            gameStateManager = newGameStateManager;

            if (gameStateManager != null)
            {
                Debug.Log("StageManager�� GameStateManager ���� ���� �Ϸ�");
                
                // �̺�Ʈ ���� (�ߺ� ����)
                GameStateManager.OnStageChanged -= OnStageChanged;
                GameStateManager.OnGameStateChanged -= OnGameStateChanged;
                GameStateManager.OnStageChanged += OnStageChanged;
                GameStateManager.OnGameStateChanged += OnGameStateChanged;
                
                Debug.Log("StageManager �̺�Ʈ ���� �Ϸ� (SetGameStateManager����)");
            }
            else
            {
                Debug.LogWarning("StageManager���� GameStateManager ������ null�� �����Ǿ����ϴ�.");
            }
        }

        /// <summary>
        /// ���� ������ GameStateManager ������ ��ȯ�մϴ�.
        /// </summary>
        /// <returns>GameStateManager �ν��Ͻ� �Ǵ� null</returns>
        public GameStateManager GetGameStateManager()
        {
            return gameStateManager;
        }

        /// <summary>
        /// StageTransition ������ �����մϴ�.
        /// </summary>
        /// <param name="newStageTransition">������ StageTransition �ν��Ͻ�</param>
        public void SetStageTransition(StageTransition newStageTransition)
        {
            stageControl = newStageTransition;

            if (stageControl != null)
            {
                Debug.Log("StageManager�� StageTransition ���� ���� �Ϸ�");
            }
            else
            {
                Debug.LogWarning("StageManager���� StageTransition ������ null�� �����Ǿ����ϴ�.");
            }
        }

        /// <summary>
        /// ���� ������ StageTransition ������ ��ȯ�մϴ�.
        /// </summary>
        /// <returns>StageTransition �ν��Ͻ� �Ǵ� null</returns>
        public StageTransition GetStageTransition()
        {
            return stageControl;
        }

        /// <summary>
        /// ���� ���������� Ŭ�����ϰ� ���� ���������� ��ȯ�մϴ�.
        /// </summary>
        public void ClearCurrentStageAndNext()
        {
            int currentMainStage = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int currentSubStage = PlayerPrefs.GetInt("SelectedSubStage", 1);
            
            Debug.Log($"�������� Ŭ����: {currentMainStage}-{currentSubStage}");
            
            // ���� �������� ���
            var nextStage = CalculateNextStage(currentMainStage, currentSubStage);
            
            if (nextStage.isGameComplete)
            {
                Debug.Log("��� �������� Ŭ����! ���� Ŭ���� ó��");
                OnGameComplete();
                return;
            }
            
            // ���� ���������� �̵�
            LoadStage(nextStage.mainStage, nextStage.subStage);
            Debug.Log($"���� ���������� ��ȯ: {nextStage.mainStage}-{nextStage.subStage}");
        }

        /// <summary>
        /// ���� �������� ������ ����մϴ�.
        /// </summary>
        /// <param name="currentMainStage">���� ���� ��������</param>
        /// <param name="currentSubStage">���� ���� ��������</param>
        /// <returns>���� �������� ����</returns>
        private (int mainStage, int subStage, bool isGameComplete) CalculateNextStage(int currentMainStage, int currentSubStage)
        {
            // ���� �������� �ִ밪 (5��)
            const int MAX_SUB_STAGES = 5;
            
            // ���� ���� �������� ���
            int nextSubStage = currentSubStage + 1;
            
            // ���� ���������� �ִ밪�� ������ ���� ���� ����������
            if (nextSubStage > MAX_SUB_STAGES)
            {
                int nextMainStage = currentMainStage + 1;
                
                // ���� �������� �ִ밪 (4��)
                const int MAX_MAIN_STAGES = 4;
                
                // ���� ���������� �ִ밪�� ������ ���� Ŭ����
                if (nextMainStage > MAX_MAIN_STAGES)
                {
                    return (0, 0, true); // ���� Ŭ����
                }
                
                // ���� ���� ���������� ù ��° ���� ����������
                return (nextMainStage, 1, false);
            }
            else
            {
                // ���� ���� ���������� ���� ���� ����������
                return (currentMainStage, nextSubStage, false);
            }
        }

        /// <summary>
        /// Ư�� ���� ���������� �ִ� ���� �������� ���� ��ȯ�մϴ�.
        /// </summary>
        private int GetMaxSubStageCount(int mainStageID)
        {
            // StageData���� ���� ���� �������� ���� �������ų�, �⺻�� 5 ���
            StageData stageData = stageDataList?.Find(data => data.stageID == mainStageID);
            if (stageData != null && stageData.subStages != null)
            {
                return stageData.subStages.Count;
            }
            return 5; // �⺻��
        }

        /// <summary>
        /// ��ü ���� �������� ���� ��ȯ�մϴ�.
        /// </summary>
        private int GetMaxMainStageCount()
        {
            return stageDataList != null ? stageDataList.Count : 4; // �⺻��
        }

        /// <summary>
        /// ���� Ŭ���� �� ȣ��˴ϴ�.
        /// </summary>
        private void OnGameComplete()
        {
            Debug.Log("���� Ŭ����!");
            
            // GameStateManager�� ���� Ŭ���� �˸�
            if (gameStateManager != null)
            {
                gameStateManager.SetGameState(GameState.StageComplete);
            }
            
            // ��� ȭ������ �̵�
            if (GameSceneManager.Instance != null)
            {
                //GameSceneManager.Instance.LoadResultScene(1000, true); // ���� ����
            }
        }

        /// <summary>
        /// �������� ������ �׽�Ʈ�ϴ� �޼����Դϴ�.
        /// </summary>
        public void TestStageProgression()
        {
            Debug.Log("=== �������� ���� �׽�Ʈ ===");
            
            // 1-1���� �����ؼ� ��� �������� ���� �׽�Ʈ
            for (int mainStage = 1; mainStage <= 4; mainStage++)
            {
                for (int subStage = 1; subStage <= 5; subStage++)
                {
                    var nextStage = CalculateNextStage(mainStage, subStage);
                    
                    if (nextStage.isGameComplete)
                    {
                        Debug.Log($"{mainStage}-{subStage} �� ���� Ŭ����!");
                        break;
                    }
                    else
                    {
                        Debug.Log($"{mainStage}-{subStage} �� {nextStage.mainStage}-{nextStage.subStage}");
                    }
                }
            }
            
            Debug.Log("=== �������� ���� �׽�Ʈ �Ϸ� ===");
        }

        /// <summary>
        /// Ư�� ���������� ���� �̵��մϴ� (�׽�Ʈ��).
        /// </summary>
        public void ForceStage(int mainStageID, int subStageID)
        {
            Debug.Log($"���� �������� �̵�: {mainStageID}-{subStageID}");
            LoadStage(mainStageID, subStageID);
        }

        /// <summary>
        /// �������� ���� ���¸� �ʱ�ȭ�մϴ� (�׽�Ʈ��).
        /// </summary>
        public void ResetStageProgress()
        {
            PlayerPrefs.SetInt("SelectedMainStage", 1);
            PlayerPrefs.SetInt("SelectedSubStage", 1);
            PlayerPrefs.Save();
            
            Debug.Log("�������� ���� ���� �ʱ�ȭ: 1-1");
        }

        /// <summary>
        /// ������ �� ������ �׽�Ʈ�մϴ�.
        /// </summary>
        public void ForceTestMapGeneration()
        {
            Debug.Log("=== ���� �� ���� �׽�Ʈ ���� ===");
            
            // 1. StageData �Ҵ� Ȯ��
            Debug.Log($"StageDataList ����: {stageDataList?.Count ?? 0}");
            if (stageDataList != null)
            {
                for (int i = 0; i < stageDataList.Count; i++)
                {
                    Debug.Log($"StageData[{i}]: ID={stageDataList[i]?.stageID}, Name={stageDataList[i]?.stageName}");
                }
            }
            
            // 2. PlayerPrefs Ȯ��
            int mainStage = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int subStage = PlayerPrefs.GetInt("SelectedSubStage", 1);
            Debug.Log($"PlayerPrefs - Main: {mainStage}, Sub: {subStage}");
            
            // 3. ������ �� ����
            LoadStage(mainStage, subStage);
            
            Debug.Log("=== ���� �� ���� �׽�Ʈ �Ϸ� ===");
        }

        /// <summary>
        /// ���� ���� ������ ����մϴ�.
        /// </summary>
        private void DebugCurrentState()
        {
            Debug.Log("=== ���� ���� ���� ===");
            Debug.Log($"StageDataList: {(stageDataList != null ? stageDataList.Count : 0)}��");
            Debug.Log($"CurrentStage: {(currentStage != null ? currentStage.stageName : "null")}");
            Debug.Log($"SpawnedMaps: {spawnedMaps.Count}��");
            Debug.Log($"MovingMaps: {movingMaps.Count}��");
            Debug.Log($"GameStateManager: {(GameStateManager.Instance != null ? "����" : "null")}");
            Debug.Log($"PlayerPrefs - Main: {PlayerPrefs.GetInt("SelectedMainStage", 1)}, Sub: {PlayerPrefs.GetInt("SelectedSubStage", 1)}");
            Debug.Log("=== ���� ���� �Ϸ� ===");
        }
    }
}