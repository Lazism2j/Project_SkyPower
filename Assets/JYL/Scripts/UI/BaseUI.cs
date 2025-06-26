using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    // Ư���� ����� �ִ� UI�� ����ϰ��� �� ���, �ĺ��Ǵ� �̸��� ����Ѵ�.
    public class BaseUI : MonoBehaviour
    {
        private Dictionary<string, GameObject> goDict;
        private Dictionary<string, Component> compDict;
        private void Awake()
        {
            RectTransform[] transforms = GetComponentsInChildren<RectTransform>();
            goDict = new Dictionary<string, GameObject>(transforms.Length<<2);
            foreach (Transform t in transforms)
            {
                goDict.TryAdd(t.gameObject.name, t.gameObject);
            }

            Component[] components = GetComponentsInChildren<Component>();
            compDict = new Dictionary<string, Component>(components.Length << 2);
            foreach (Component comp in components)
            {
                compDict.TryAdd($"{comp.gameObject.name}_{comp.GetType().Name}",comp);
            }
        }

        // string���� Ư�� UI ���ӿ�����Ʈ ã��
        public GameObject GetUI(in string name)
        {
            goDict.TryGetValue(name, out GameObject gameObject);
            if(gameObject == null)
            {
                Debug.LogError($"���� UI ������Ʈ�� �����ϴ�: {name}");
            }
            return gameObject; // ������� Null
        }

        // string���� Ư�� UI ������Ʈ ã��
        public T GetUI<T>(in string name) where T : Component
        {
            compDict.TryGetValue(name, out Component comp);
            if(comp != null)
            {
                return comp as T;
            }
            GameObject go = GetUI(name);
            if(go == null)
            {
                return null;
            }
            comp = go.GetComponent<T>();
            if (comp == null) return null;
            compDict.TryAdd($"{name}_{typeof(T).Name}", comp);
            return comp as T;
        }

        //  UI�� �������ڵ鷯�� ���� ��, ��������
        public PointerHandler GetEvent(in string name)
        {
            GameObject go = GetUI(name);
            PointerHandler temp = go.GetOrAddComponent<PointerHandler>();

            return temp;
        }
    }
}

