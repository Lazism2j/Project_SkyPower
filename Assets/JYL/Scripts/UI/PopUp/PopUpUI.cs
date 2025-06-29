using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL 
{
    public class PopUpUI : MonoBehaviour
    {
        // TODO: �˾� �Ǹ� �˾� �ǳ� ���� �ٱ��� 1. Ŭ������ ���ϰ� ���ų�, 2. Ŭ�� �� �˾� �������� ���ư��� �Ѵ�.
        private Stack<BaseUI> stack = new Stack<BaseUI>();
        [SerializeField] GameObject blocker;
        
        public void PushUIStack(BaseUI ui)
        {
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
            if(stack.Count<=0) return;

            
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
    }
}

