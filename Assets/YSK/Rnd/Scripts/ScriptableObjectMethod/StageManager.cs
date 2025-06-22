using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        
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
        
        // �ڵ� �������� �ε� ���� - GameStateManager�� �����ϵ��� ��
        // LoadStage(selectedstageID);
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
                if (spawnedMaps.Count == 0 && gameStateManager != null)
                {
                    Debug.Log("���� ����: �������� �ε�");
                    LoadStage(gameStateManager.CurrentStageID);
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
            gameStateManager = FindObjectOfType<GameStateManager>();
            if (gameStateManager == null)
            {
                Debug.LogWarning("GameStateManager�� ã�� �� �����ϴ�!");
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
            Debug.LogError("StageTransition ������Ʈ�� �ڽ� ������Ʈ���� ã�� �� �����ϴ�!");
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
                Debug.LogError("StartPoint�� ã�� �� �����ϴ�!");
            }
        }
        
        if (endPoint == null)
        {
            endPoint = transform.Find("EndPoint");
            if (endPoint == null)
            {
                Debug.LogError("EndPoint�� ã�� �� �����ϴ�!");
            }
        }
    }

    private void Update()
    {
        UpdateMovingMaps();

        CheckInput();

    }


    private void CheckInput()
    {
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1)) HandleKey(1);
            else if (Input.GetKeyDown(KeyCode.Alpha2)) HandleKey(2);
            else if (Input.GetKeyDown(KeyCode.Alpha3)) HandleKey(3);
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
        
        currentStage = stageDataList.Find(data => data.stageID == stageID);

        if (currentStage == null)
        {
            Debug.LogError($"Stage ID {stageID} not found!");
            return;
        }

        Debug.Log($"�������� {stageID} ������ �ε� �Ϸ�, �� ���� ����");
        SpawnMaps();
    }



    private void SpawnMaps()
    {
        Debug.Log($"SpawnMaps ����: ���� �� {spawnedMaps.Count}�� ����");
        spawnedMaps.Clear();
        movingMaps.Clear();

        Debug.Log($"�� �� ���� ����: {currentStage.mapPrefabs.Count}�� ������");
        for (int i = 0; i < currentStage.mapPrefabs.Count; i++)
        {
            GameObject map = Instantiate(currentStage.mapPrefabs[i]);
            // ���� �Ϸķ� ��ġ (��: Z�� ����)
            map.transform.position = startPoint.position + Vector3.back * (mapLength * i);
            spawnedMaps.Add(map);
            movingMaps.Add(map);
            Debug.Log($"�� {i + 1} ���� �Ϸ�: {map.name}");
        }
        
        Debug.Log($"SpawnMaps �Ϸ�: �� {spawnedMaps.Count}�� �� ����");
    }





    private void MoveMap(GameObject map)
    {
        map.transform.position += Vector3.back * speed * Time.deltaTime;

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
            if (map.transform.position.z > maxZ)
                maxZ = map.transform.position.z;
        }
        return maxZ;
    }

    private void UpdateMovingMaps()
    {
        foreach (var map in movingMaps)
        {
            MoveMap(map);
        }
    }

    public void ChangeStage(int newStageID)
    {
        Debug.Log($"ChangeStage ȣ��: �������� {newStageID}�� ����");
        
        // LoadStage���� �̹� ClearAllMaps�� ȣ���ϹǷ� ���⼭�� ����
        // ���ο� �������� ������ �ε� �� �� ��ġ
        LoadStage(newStageID);

        // UI �ؽ�Ʈ ������Ʈ
        GameSceneManager gameSceneManager = FindObjectOfType<GameSceneManager>();
        if (gameSceneManager != null)
        {
            gameSceneManager.UpdateStageText(newStageID);
        }

        // 3. �÷��̾�/ī�޶� ��ġ �ʱ�ȭ
        // player.transform.position = ...;
        // camera.transform.position = ...;

        // 4. UI �� ���� ���� �ʱ�ȭ
        // UpdateUI();
        // SetGameState(GameState.Ready);
    }

    /// <summary>
    /// Ư�� �������� ID�� StageData�� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="stageID">ã�� �������� ID</param>
    /// <returns>�ش� �������� ������ �Ǵ� null</returns>
    public StageData GetStageData(int stageID)
    {
        return stageDataList.Find(data => data.stageID == stageID);
    }

    /// <summary>
    /// ���� �������� �����͸� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>���� �������� ������</returns>
    public StageData GetCurrentStageData()
    {
        return currentStage;
    }

    /// <summary>
    /// ���� ������ �ʵ��� ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>���� �ʵ��� ����Ʈ</returns>
    public List<GameObject> GetSpawnedMaps()
    {
        return new List<GameObject>(spawnedMaps);
    }

    /// <summary>
    /// ���� �̵� ���� �ʵ��� ����Ʈ�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns>���� �̵� ���� �ʵ��� ����Ʈ</returns>
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

}
