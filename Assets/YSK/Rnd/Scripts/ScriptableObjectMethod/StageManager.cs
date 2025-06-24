using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSK;

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
            InitializeComponents();

            // GameStateManager �̺�Ʈ ����
            if (gameStateManager != null)
            {
                GameStateManager.OnStageChanged += OnStageChanged;
                GameStateManager.OnGameStateChanged += OnGameStateChanged;
            }
        }

        private void Update()
        {
            UpdateMovingMaps();
            CheckInput();
        }

        private void OnDestroy()
        {
            // �̺�Ʈ ���� ����
            if (gameStateManager != null)
            {
                GameStateManager.OnStageChanged -= OnStageChanged;
                GameStateManager.OnGameStateChanged -= OnGameStateChanged;
            }
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
            Debug.Log($"OnGameStateChanged: {newState}");

            switch (newState)
            {
                case GameState.Playing:
                    // �̹� ���������� �ε�Ǿ� ���� ���� ��쿡�� �ε�
                    if (spawnedMaps.Count == 0)
                    {
                        Debug.Log("���� ����: �������� �ε�");
                        // PlayerPrefs���� ���� �������� ���� ��������
                        int mainStageID = PlayerPrefs.GetInt("SelectedMainStage", 1);
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
            }
        }

        private void HandleKey(int keyNumber)
        {
            if (stageControl == null)
            {
                Debug.LogError("StageTransition�� �ʱ�ȭ���� �ʾҽ��ϴ�!");
                return;
            }

            switch (keyNumber)
            {
                case 1:
                    Debug.Log("1�� Ű: ù ��° �������� �ε�");
                    stageControl.StartStageTransition(1, false);
                    break;

                case 2:
                    Debug.Log("2�� Ű: �� ��° �������� �ε�");
                    stageControl.StartStageTransition(2, false);
                    break;

                case 3:
                    Debug.Log("3�� Ű: �� ��° �������� �ε�");
                    stageControl.StartStageTransition(3, false);
                    break;

                case 4:
                    Debug.Log("4�� Ű: �� ��° �������� �ε�");
                    stageControl.StartStageTransition(4, false);
                    break;

                default:
                    Debug.LogWarning("�� �� ���� Ű �Է�");
                    break;
            }
        }

        private void LoadStage(int stageID)
        {
            Debug.Log($"LoadStage ȣ��: �������� {stageID}");

            // ���� �� ����
            ClearAllMaps();

            // PlayerPrefs���� ���� ���������� ���� �������� ���� ��������
            int mainStageID = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int subStageID = PlayerPrefs.GetInt("SelectedSubStage", 1);

            Debug.Log($"�������� ���� - ����: {mainStageID}, ����: {subStageID}");

            // �������� ������ ����Ʈ�� ����ִ��� Ȯ��
            if (stageDataList == null || stageDataList.Count == 0)
            {
                Debug.LogError("�������� ������ ����Ʈ�� ����ֽ��ϴ�! Inspector���� StageData�� �Ҵ����ּ���.");
                return;
            }

            Debug.Log($"��� ������ ��������: {string.Join(", ", stageDataList.Select(s => s.stageID))}");

            // ���� �������� ID�� �ش� �������� ������ ã��
            currentStage = stageDataList.Find(data => data.stageID == mainStageID);

            if (currentStage == null)
            {
                Debug.LogError($"Main Stage ID {mainStageID} not found! ��� ������ ��������: {string.Join(", ", stageDataList.Select(s => s.stageID))}");
                return;
            }

            Debug.Log($"���� �������� {mainStageID} ������ �ε� �Ϸ�, ���� �������� {subStageID} �� ���� ����");
            SpawnMaps();
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

                // �̺�Ʈ ����
                GameStateManager.OnStageChanged += OnStageChanged;
                GameStateManager.OnGameStateChanged += OnGameStateChanged;
            }
            else
            {
                Debug.LogWarning("StageManager���� GameStateManager ������ null�� �����Ǿ����ϴ�.");

                // �̺�Ʈ ���� ����
                GameStateManager.OnStageChanged -= OnStageChanged;
                GameStateManager.OnGameStateChanged -= OnGameStateChanged;
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
    }
}