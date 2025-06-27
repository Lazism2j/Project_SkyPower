using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace KYG_skyPower
{

    // �߻�ȭ �� Ŀ���� Init Ȯ�强�� ����
    public abstract class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<T>();
                    if (instance != null) return instance;

                    GameObject go = new GameObject(typeof(T).Name); // �̸� ���� ����!
                    instance = go.AddComponent<T>();
                    DontDestroyOnLoad(go);
                }
                return instance;
            }
        }

        protected virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
                DontDestroyOnLoad(gameObject);
                OnAwake(); // Ŀ���� Ȯ�� ����
            }
            else if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        // �ڽĿ��� Ŀ���� Awake
        protected virtual void OnAwake() { }

        // �Ŵ��� �ϰ� Init�� ���� �߻�ȭ (Manager���� ���� ȣ�� ����)
        public virtual void Init() { }

        private void OnApplicationQuit()
        {
            instance = null;
        }

        public void DestroyManager()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
        }
    }
}
