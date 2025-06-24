using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KYG
{
    public class ObjectPoolRunner : MonoBehaviour // ������Ʈ Ǯ �Ŵ����� �ʱ� Ǯ ����
    {
        public ObjectPoolManagerSO poolManager;
        public GameObject prefab;
        public string key = "Bullet";

        void Awake()
        {
            poolManager.CreatePool(key, prefab, 20);
        }
    }
}