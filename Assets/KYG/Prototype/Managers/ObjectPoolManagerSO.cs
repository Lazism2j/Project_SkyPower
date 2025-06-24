using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG
{
    [CreateAssetMenu(fileName = "ObjectPoolManager", menuName = "Managers/ObjectPoolManager")]
    public class ObjectPoolManagerSO : ScriptableObject
    {
        private Dictionary<string, Queue<GameObject>> pool = new();
        private Dictionary<string, GameObject> prefabDictionary = new(); // ���� ������ ����

        // ������Ʈ Ǯ ����
        public void CreatePool(string key, GameObject prefab, int count)
        {
            if (!pool.ContainsKey(key))
                pool[key] = new Queue<GameObject>();

            if (!prefabDictionary.ContainsKey(key))
                prefabDictionary[key] = prefab;

            for (int i = 0; i < count; i++)
            {
                GameObject obj = Instantiate(prefab);
                obj.SetActive(false);
                pool[key].Enqueue(obj);
            }
        }

        // Ǯ���� ������ (�����ϸ� �ڵ� ����)
        public GameObject Spawn(string key, Vector3 pos, Quaternion rot)
        {
            if (!pool.ContainsKey(key))
            {
                Debug.LogWarning($"[Pool] Key '{key}' not found!");
                return null;
            }

            if (pool[key].Count == 0)
            {
                // �ڵ� Ȯ��
                if (prefabDictionary.ContainsKey(key))
                {
                    Debug.LogWarning($"[Pool] Pool '{key}' empty. Instantiating additional object.");
                    GameObject extra = Instantiate(prefabDictionary[key]);
                    extra.SetActive(false);
                    pool[key].Enqueue(extra);
                }
                else
                {
                    Debug.LogError($"[Pool] No prefab registered for key: {key}");
                    return null;
                }
            }

            GameObject obj = pool[key].Dequeue();
            obj.transform.SetPositionAndRotation(pos, rot);
            obj.SetActive(true);

            return obj;
        }

        // Ǯ�� ��ȯ
        public void Despawn(string key, GameObject obj)
        {
            obj.SetActive(false);
            if (!pool.ContainsKey(key))
                pool[key] = new Queue<GameObject>();

            pool[key].Enqueue(obj);
        }

        // ���� ���� Ȯ�ο� (����� �Ǵ� �ΰ��� ���� üũ)
        public int GetRemainingCount(string key)
        {
            return pool.ContainsKey(key) ? pool[key].Count : 0;
        }
    }
}