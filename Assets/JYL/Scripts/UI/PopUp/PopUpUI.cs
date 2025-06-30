using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL 
{
    public class PopUpUI : MonoBehaviour
    {
        // TODO: �˾� �Ǹ� �˾� �ǳ� ���� �ٱ��� 1. Ŭ������ ���ϰ� ���ų�, 2. Ŭ�� �� �˾� �������� ���ư��� �Ѵ�.
        private Stack<BaseUI> stack = new Stack<BaseUI>();
        public static bool IsPopUpActive { get; private set; }

        [SerializeField] GameObject blocker;
        
        public void PushUIStack(BaseUI ui)
        {
            IsPopUpActive = true;
            if (stack.Count > 0)
            {
                BaseUI top = stack.Peek();
                top.gameObject.SetActive(false);
            }
            stack.Push(ui);

            blocker.SetActive(true);
        }
        public void PopUIStack()
        {
            if(stack.Count ==1)
            {
                IsPopUpActive = false;
            }
            if(stack.Count<=0)
            {
                IsPopUpActive = false;
                return;
            }

            
            Destroy(stack.Pop().gameObject);

            if(stack.Count >0)
            {
                BaseUI top = stack.Peek();
                top.gameObject.SetActive(true);
            }
            else
            {
                blocker.SetActive(false);
            }

        }
        public int StackCount()
        {
            return stack.Count;
        }
    }
}

