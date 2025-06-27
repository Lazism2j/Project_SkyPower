using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public static class Util
    {
        public static T GetOrAddComponent<T>(this GameObject go) where T : Component
        {
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            return comp;
        }

        public static bool ExtractTrailNumber(in string input, out int number)
        {
            number = -1;
            if(string.IsNullOrEmpty(input))
            {
                Debug.LogWarning("�Է��� �����ϴ�");
                return false;
            }
            
            int i = input.Length - 1;
            while (i >= 0 && char.IsDigit(input[i])) i--;
            if(i == input.Length - 1)
            {
                Debug.LogWarning("�Է¿� ���ڰ� �����ϴ�");
                return false;
            }

            return int.TryParse(input[(i + 1)..], out number);

        }
    }
}

