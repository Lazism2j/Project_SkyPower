using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YSK;
using System.Collections;
using UnityEngine.UI;

namespace YSK
{
    public class StageManager : MonoBehaviour
    {
        [Header("StageData")]
        [SerializeField] private List<StageData> stageDataList; // ��� ���������� ������
        [SerializeField] private int maxMainStages = 4; // ���� �������� �ִ밪
        [SerializeField] private int maxSubStages = 5; // ���� �������� �ִ밪
        private StageData currentStage;
        private List<GameObject> MapPrefabs;

        [Header("MoveInfo")]
        [SerializeField] private Transform endPoint;
        [SerializeField] private Transform startPoint;
        [SerializeField] private float speed = 3f;
        [SerializeField] private float mapLength = 20;

        [Header("Transition Settings")]
        [SerializeField] private bool enableTransition = true;
        [SerializeField] private bool useFadeTransition = true;
        [SerializeField] private CanvasGroup fadePanel;
        [SerializeField] private float fadeDuration = 1f;
        [SerializeField] private AnimationCurve fadeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
        [SerializeField] private Color fadeColor = Color.black;

        [Header("References")]
        //[SerializeField] private GameStateManager gameStateManager;

        private List<GameObject> spawnedMaps = new(); // �������� �̿��� Stage Map ����
        private List<GameObject> movingMaps = new(); // ���� �̵����� ��.
        private bool isTransitioning = false;

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
            //StartCoroutine(WaitForGameStateManager());
            
            Debug.Log("=== StageManager Start �Ϸ� ===");
        }

        private void Update()
        {
            UpdateMovingMaps();
            CheckInput();
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
        /// �ʿ��� ������Ʈ���� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void InitializeComponents()
        {
            FindTransformPoints();
            InitializeFadePanel();
            //FindGameStateManager();
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

        /// <summary>
        /// ���̵� �г��� �ʱ�ȭ�մϴ�.
        /// </summary>
        private void InitializeFadePanel()
        {
            // ��ȯ ����� ��Ȱ��ȭ�Ǿ� ������ ���̵� �г��� �������� ����
            if (!enableTransition || !useFadeTransition)
            {
                Debug.Log("��ȯ ����� ��Ȱ��ȭ�Ǿ� ���̵� �г��� �������� �ʽ��ϴ�.");
                return;
            }
            
            if (fadePanel == null)
            {
                CreateFadePanel();
            }
            
            // �ʱ� ���� ���� - �׻� �����ϰ� ����
            fadePanel.alpha = 0f;
            fadePanel.gameObject.SetActive(false);
            
            Debug.Log("���̵� �г� �ʱ�ȭ �Ϸ� - ���� ����");
        }
        
        private void CreateFadePanel()
        {
            // Canvas ã�� �Ǵ� ����
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas == null)
            {
                GameObject canvasObj = new GameObject("TransitionCanvas");
                canvas = canvasObj.AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                canvas.sortingOrder = 999; // �ֻ����� ǥ��
                canvasObj.AddComponent<CanvasScaler>();
                canvasObj.AddComponent<GraphicRaycaster>();
            }
            
            // Fade Panel ����
            GameObject fadeObj = new GameObject("FadePanel");
            fadeObj.transform.SetParent(canvas.transform, false);
            
            Image fadeImage = fadeObj.AddComponent<Image>();
            fadeImage.color = fadeColor;
            
            RectTransform rectTransform = fadeObj.GetComponent<RectTransform>();
            rectTransform.anchorMin = Vector2.zero;
            rectTransform.anchorMax = Vector2.one;
            rectTransform.offsetMin = Vector2.zero;
            rectTransform.offsetMax = Vector2.zero;
            
            fadePanel = fadeObj.AddComponent<CanvasGroup>();
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
                    ForceStage(1, 1);
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

            // PlayerPrefs���� ���� ���� �������� ���� ��������
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
                    Debug.Log($"���� �������� {subStageID} Ŀ���� �� ���");
                }
                else
                {
                    Debug.Log($"���� �������� {subStageID} �⺻ �� ���");
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
            Debug.Log($"ChangeStage ȣ��: �������� {newStageID}�� ����");

            // LoadStage���� �̹� ClearAllMaps�� ȣ���ϹǷ� ���⼭�� ����
            // �ܼ��� ���� ���������� �����ϰ� ���� ���������� 1�� �ʱ�ȭ
            LoadStage(newStageID);

            // PlayerPrefs���� ���� ������ ���� ���������� ���� �������� ���� ��������
            int mainStageID = PlayerPrefs.GetInt("SelectedMainStage", 1);
            int subStageID = PlayerPrefs.GetInt("SelectedSubStage", 1);

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
        /// ���� ������ ���� ���������� ID�� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� ���� �������� ID</returns>
        public int GetCurrentSubStageID()
        {
            return PlayerPrefs.GetInt("SelectedSubStage", 1);
        }

        /// <summary>
        /// ���� ������ ���� ���������� �����͸� ��ȯ�մϴ�.
        /// </summary>
        /// <returns>���� �������� ������ �Ǵ� null</returns>
        public SubStageData GetCurrentSubStageData()
        {
            if (currentStage == null) return null;

            int subStageID = GetCurrentSubStageID();
            return currentStage.subStages?.Find(s => s.subStageID == subStageID);
        }

        #region Transition Methods

        /// <summary>
        /// �ε巯�� �������� ��ȯ�� �����մϴ�.
        /// </summary>
        /// <param name="newStageID">��ȯ�� �������� ID</param>
        /// <param name="isGameStart">���� ���� ���� (true: ���� ����, false: �������� ��ȯ)</param>
        public void StartStageTransition(int newStageID, bool isGameStart = false)
        {
            Debug.Log($"StageManager.StartStageTransition ȣ��: �������� {newStageID}, ���ӽ���: {isGameStart}");
            
            // ��ȯ ����� ��Ȱ��ȭ�Ǿ� ������ ��� ��ȯ
            if (!enableTransition)
            {
                Debug.Log("��ȯ ����� ��Ȱ��ȭ�Ǿ� ��� ��ȯ�մϴ�.");
                ChangeStage(newStageID);
                return;
            }
            
            if (!isTransitioning)
            {
                Debug.Log("�������� ��ȯ �ڷ�ƾ ����");
                StartCoroutine(TransitionCoroutine(newStageID, isGameStart));
            }
            else
            {
                Debug.LogWarning("�̹� ��ȯ ���Դϴ�!");
            }
        }
        
        /// <summary>
        /// ���̵� ��/�ƿ��� �̿��� �������� ��ȯ
        /// </summary>
        private IEnumerator TransitionCoroutine(int newStageID, bool isGameStart)
        {
            Debug.Log($"TransitionCoroutine ����: �������� {newStageID}");
            isTransitioning = true;
            
            if (isGameStart)
            {
                Debug.Log("���� ����: ���̵� ȿ�� ���� ��� ��ȯ");
                ChangeStage(newStageID);
            }
            else
            {
                if (useFadeTransition)
                {
                    Debug.Log("�������� ��ȯ: ���̵� ȿ�� ���");
                    // ���̵� �ƿ�
                    yield return StartCoroutine(FadeOut());
                    
                    // �������� ��ȯ
                    ChangeStage(newStageID);
                    
                    // ���̵� ��
                    yield return StartCoroutine(FadeIn());
                }
                else
                {
                    Debug.Log("�������� ��ȯ: ���̵� ȿ�� ���� ��� ��ȯ");
                    ChangeStage(newStageID);
                }
            }
            
            isTransitioning = false;
            Debug.Log("TransitionCoroutine �Ϸ�");
        }
        
        private IEnumerator FadeOut()
        {
            if (fadePanel == null)
            {
                Debug.LogWarning("���̵� �г��� null�Դϴ�. ��� ��ȯ�մϴ�.");
                yield break;
            }
            
            fadePanel.gameObject.SetActive(true);
            float elapsed = 0f;
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                fadePanel.alpha = fadeCurve.Evaluate(t);
                yield return null;
            }
            
            fadePanel.alpha = 1f;
        }
        
        private IEnumerator FadeIn()
        {
            if (fadePanel == null)
            {
                Debug.LogWarning("���̵� �г��� null�Դϴ�.");
                yield break;
            }
            
            float elapsed = 0f;
            
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                float t = elapsed / fadeDuration;
                fadePanel.alpha = 1f - fadeCurve.Evaluate(t);
                yield return null;
            }
            
            fadePanel.alpha = 0f;
            fadePanel.gameObject.SetActive(false);
        }
        
        /// <summary>
        /// ���� ��ȯ ������ Ȯ��
        /// </summary>
        public bool IsTransitioning => isTransitioning;
        
        /// <summary>
        /// ��ȯ ȿ�� ����
        /// </summary>
        public void SetFadeDuration(float duration)
        {
            fadeDuration = duration;
        }
        
        /// <summary>
        /// ���̵� �г� ���� ����
        /// </summary>
        public void SetFadeColor(Color color)
        {
            fadeColor = color;
            if (fadePanel != null)
            {
                Image fadeImage = fadePanel.GetComponent<Image>();
                if (fadeImage != null)
                {
                    fadeImage.color = color;
                }
            }
        }
        
        /// <summary>
        /// �������� ��ư Ŭ�� �̺�Ʈ (UI���� ȣ��)
        /// </summary>
        /// <param name="stageID">���õ� �������� ID</param>
        public void OnStageButtonClick(int stageID)
        {
            if (!isTransitioning)
            {
                StartStageTransition(stageID, false); // �������� ��ȯ
            }
        }

        #endregion

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
            
            // ���� ���������� ��ȯ
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
            // ���� ���� �������� ���
            int nextSubStage = currentSubStage + 1;
            
            // ���� ���������� �ִ밪�� ������ ���� ���� ����������
            if (nextSubStage > maxSubStages)
            {
                int nextMainStage = currentMainStage + 1;
                
                // ���� ���������� �ִ밪�� ������ ���� Ŭ����
                if (nextMainStage > maxMainStages)
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
        /// ���� Ŭ���� �� ȣ��˴ϴ�.
        /// </summary>
        private void OnGameComplete()
        {
            Debug.Log("���� Ŭ����!");
            
            // GameStateManager�� ���� Ŭ���� �˸�
            //if (gameStateManager != null)
            //{
            //    gameStateManager.SetGameState(GameState.StageComplete);
            //}
            
            // ��� ȭ������ �̵�
            if (GameSceneManager.Instance != null)
            {
                //GameSceneManager.Instance.LoadResultScene(1000, true); // ���� Ŭ����
            }
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

            // ���� ���������� �ʱ�ȭ
            LoadStage(1, 1);

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
            Debug.Log($"MaxMainStages: {maxMainStages}, MaxSubStages: {maxSubStages}");
            //Debug.Log($"GameStateManager: {(GameStateManager.Instance != null ? "����" : "null")}");
            Debug.Log($"PlayerPrefs - Main: {PlayerPrefs.GetInt("SelectedMainStage", 1)}, Sub: {PlayerPrefs.GetInt("SelectedSubStage", 1)}");
            Debug.Log("=== ���� ���� ���� �Ϸ� ===");
        }
    }
}