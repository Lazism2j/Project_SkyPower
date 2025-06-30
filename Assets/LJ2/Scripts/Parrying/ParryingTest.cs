using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingTest : MonoBehaviour
{
    [SerializeField] int shield;
    [SerializeField] float invincibleTime;
    [SerializeField] float parryingRadius;
    [SerializeField] float destroyRadius;
    [SerializeField] LayerMask enemyBullet;
    [SerializeField] LayerMask destroyLayer;
    [SerializeField] SphereCollider charactorCollider;
    private Coroutine coroutine;


    public YieldInstruction coroutineDelay;

    private void Awake()
    {
        coroutineDelay = new WaitForSeconds(invincibleTime);
        charactorCollider = GetComponent<SphereCollider>();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Parrying();
        }
    }

    public void Parrying()
    {
        Collider[] canParrying = Physics.OverlapSphere(transform.position, parryingRadius, enemyBullet);
        Debug.Log("�и� ����");
        if (canParrying.Length > 0)
        {
            Debug.Log("���ǹ� ����");

            foreach (Collider c in canParrying)
            {
                c.gameObject.SetActive(false);
                Debug.Log("�ݺ��� ����");

            }
            coroutine = StartCoroutine(EraseCoroutine());

        }
        else return;

    }
    
    private IEnumerator InvincibleCoroutine()
    {
        Debug.Log("�ڷ�ƾ ����");

        charactorCollider.enabled = false;
        yield return coroutineDelay;
        charactorCollider.enabled = true;
        coroutine = null;
        yield break;

    }

    private IEnumerator EraseCoroutine()
    {
        yield return null;

        Collider[] hits = Physics.OverlapSphere(transform.position, destroyRadius, destroyLayer);

        Debug.Log("Erase �ڷ�ƾ ����");
        foreach (Collider c in hits)
        {
            Debug.Log("�ݺ��� ����");

            c.gameObject.SetActive(false);

        }
        
        hits = null;
        coroutine = null;
        Debug.Log("�ڷ�ƾ ����");

        yield break;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, destroyRadius);
    }
}
