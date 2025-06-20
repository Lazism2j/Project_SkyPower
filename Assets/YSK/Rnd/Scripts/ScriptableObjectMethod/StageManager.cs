using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageManager : MonoBehaviour
{
    [Header ("StageData")]
    [SerializeField] private List<StageData> stageDataList; // ��� ���������� ������
    private  StageData  currentStage;
    private List<GameObject> MapPrefabs;
    [SerializeField] int selectedstageID; // Test�� Stage ID�� ��� �ܺ� ���ÿ� ���ؼ� �������� �Ǵ� ������ �����ؾ���.

    [Header ("MoveInfo")]
    [SerializeField] private Transform endPoint;
    [SerializeField] private Transform startPoint;
    [SerializeField] private float speed = 3f;

    private List<GameObject> spawnedMaps = new(); // �������� �̿��� Stage Map ����

    private Queue<GameObject> mapQueue = new(); // �̵������� ���� ť
    private List<GameObject> movingMaps = new(); // ���� �̵����� ��.

    private void Awake()
    {


    }

    private void Start()
    {
        LoadStage(selectedstageID);
       
    }

    private void Update()
    {
        UpdateMovingMaps();
    }


    private void LoadStage(int stageID)
    {
        currentStage = stageDataList.Find(data => data.stageID == stageID);

        if (currentStage == null)
        {
            Debug.LogError($"Stage ID {stageID} not found!");
            return;
        }

        // �ʱ� ��ġ�� ��� �Ұ��ΰ�?

        SpawnMaps();

    }



    private void SpawnMaps()
    {
        spawnedMaps.Clear();
        mapQueue.Clear();
        movingMaps.Clear();

        for (int i = 0; i < currentStage.mapPrefabs.Count; i++)
        {
            GameObject map = Instantiate(currentStage.mapPrefabs[i]);

            if (i == 0)
            {
                map.transform.position = Vector3.zero;
                movingMaps.Add(map);
            }
            else if (i == 1)
            {
                map.transform.position = startPoint.position;
                movingMaps.Add(map);
            }
            else
            {
                map.transform.position = startPoint.position + Vector3.back * (10f * (i - 1));
                mapQueue.Enqueue(map);
            }

            spawnedMaps.Add(map);
        }

    }



    private void MoveMap(GameObject map, int index)
    {
        map.transform.position = Vector3.MoveTowards(
            map.transform.position,
            endPoint.position,
            speed * Time.deltaTime
        );

        // ���� ����
        if (Vector3.Distance(map.transform.position, endPoint.position) < 0.01f)
        {
            // �ش� ���� �ٽ� startPoint�� ������ ť �ڷ�
            map.transform.position = startPoint.position;
            mapQueue.Enqueue(map);

            // ���� ���� ���� �̵� ����Ʈ�� �ֱ�
            if (mapQueue.Count > 0)
            {
                movingMaps[index] = mapQueue.Dequeue();
            }
        }
    }


    private void UpdateMovingMaps()
    {
        for (int i = 0; i < movingMaps.Count; i++)
        {
            MoveMap(movingMaps[i], i);
        }
    }
}
