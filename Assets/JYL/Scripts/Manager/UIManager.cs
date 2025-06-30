using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class UIManager : Singleton<UIManager>
    {
        [SerializeField] string popUpPath = "JYL/UI/Canvas_PopUp";
        [SerializeField] string prefabPath = "JYL/UI";

        private PopUpUI popUp;
        public PopUpUI PopUp
        {
            get 
            {
                if (popUp == null)
                {
                    popUp = FindObjectOfType<PopUpUI>();
                    if (popUp != null) return popUp;
                    
                    PopUpUI prefab = Resources.Load<PopUpUI>(popUpPath);
                    if (prefab == null)
                    { 
                        Debug.LogWarning($"�ش� ��ο� �˾� �������� ����: {popUpPath}");
                        return null;
                    }
                    return Instantiate(prefab);
                }
                return popUp;
            }
        }

        protected override void Awake() => base.Awake();
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && PopUpUI.IsPopUpActive && !Util.escPressed)
            {
                Instance.ClosePopUp();
                Util.ConsumeESC();
            }
        }

        // �˾� UI�� ������
        public T ShowPopUp<T>() where T : BaseUI
        {
            string path = $"{prefabPath}/{typeof(T).Name}";
            T prefab = Resources.Load<T>(path);
            if (prefab == null)
            {
                Debug.LogWarning($"�ش� ��ο� �˾� ������ ����: {path}");
                return null;
            }
            T instance = Instantiate(prefab, PopUp.transform);
            PopUp.PushUIStack(instance);
            return instance;
        }
        public void ClosePopUp()
        {
            PopUp.PopUIStack();
        }
        public void CleanPopUp()
        {
            while(PopUp.StackCount() == 0)
            {
                PopUp.PopUIStack();
            }
        }
    }
}