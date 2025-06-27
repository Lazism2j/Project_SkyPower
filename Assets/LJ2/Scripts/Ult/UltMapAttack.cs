using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UltMapAttack : MonoBehaviour
{
    [SerializeField] LayerMask bullet;
    private Coroutine coroutine;

    private void OnEnable()
    {
        coroutine = StartCoroutine(EraseCoroutine());
    }

    private void OnDisable()
    {
        coroutine = null;
    }

    private IEnumerator EraseCoroutine()
    {
        yield return null;
        Collider[] hits = Physics.OverlapBox(transform.position, transform.localScale / 2f, Quaternion.identity, bullet);
        Debug.Log("Erase �ڷ�ƾ ����");
        foreach (Collider c in hits)
        {
            Debug.Log("�ݺ��� ����");

            c.gameObject.SetActive(false);
                
            
        }
        hits = null;
        yield break;
    }

    
}
