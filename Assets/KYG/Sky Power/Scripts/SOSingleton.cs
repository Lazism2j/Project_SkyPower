using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KYG_skyPower
{


    public class SOSingleton<T> : ScriptableObject where T : ScriptableObject
    {
        private static T instance;
        public static T Instance
        {
            get
            {
                if (instance == null)
                    instance = Resources.Load<T>(typeof(T).Name); // ���ϸ�/���ҽ��� ��ġ
                return instance;
            }
        }

        public virtual void Init() { }
    }
}
