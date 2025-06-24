using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectPoolManager", menuName = "Managers/ObjectPoolManager")]
public class ObjectPoolManagerSO : ScriptableObject
{
    // Ǯ ����� (key: ������ �̸�)
    private Dictionary<string, Queue<GameObject>> pool = new();

    // ������Ʈ Ǯ ����
    public void CreatePool(string key, GameObject prefab, int count)
    {
        if (!pool.ContainsKey(key))
            pool[key] = new Queue<GameObject>();

        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.SetActive(false);
            pool[key].Enqueue(obj);
        }
    }

    // Ǯ���� ������
    public GameObject Spawn(string key, Vector3 pos, Quaternion rot)
    {
        if (!pool.ContainsKey(key) || pool[key].Count == 0) return null;

        GameObject obj = pool[key].Dequeue();
        obj.transform.SetPositionAndRotation(pos, rot);
        obj.SetActive(true);
        return obj;
    }

    // Ǯ�� ��ȯ
    public void Despawn(string key, GameObject obj)
    {
        obj.SetActive(false);
        pool[key].Enqueue(obj);
    }
}
