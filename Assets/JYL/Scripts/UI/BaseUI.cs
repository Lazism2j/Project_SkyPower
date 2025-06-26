using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class BaseUI : MonoBehaviour
    {
        private Dictionary<string, GameObject> goDict;
        private Dictionary<string, GameObject> compDict;
        private void Awake()
        {
        }
        void Start()
        {

        }

        void Update()
        {

        }

        public GameObject GetUI(in string name)
        {
            goDict.TryGetValue(name, out GameObject gameObject);
            if(gameObject == null)
            {
                Debug.LogError($"���� UI ������Ʈ�� �����ϴ�: {name}");
            }
            return gameObject; // ������� Null
        }
    }
}

