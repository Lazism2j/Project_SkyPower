using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryingTest : MonoBehaviour
{
    [SerializeField] int shield;
    [SerializeField] float invincibleTime;
    [SerializeField] float parryingRadius;
    [SerializeField] float destroyRadius;
    [SerializeField] LayerMask destroyLayer;
    [SerializeField] Collider collider;
    private Coroutine coroutine;


    public YieldInstruction coroutineDelay;

    private void Awake()
    {
        coroutineDelay = new WaitForSeconds(invincibleTime);
        collider = GetComponent<Collider>();
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
        Collider[] canParrying = Physics.OverlapCapsule(transform.position, Vector3.one * parryingRadius, 1, destroyLayer);
        Debug.Log("�и� ����");
        if (canParrying.Length > 0)
        {
            Debug.Log("���ǹ� ����");

            foreach (Collider c in canParrying)
            {
                c.gameObject.SetActive(false);
                Debug.Log("�ݺ��� ����");

            }
            coroutine = StartCoroutine(InvincibleCoroutine());

        }
        else return;

    }
    
    private IEnumerator InvincibleCoroutine()
    {
        Debug.Log("�ڷ�ƾ ����");

        collider.enabled = false;
        yield return coroutineDelay;
        collider.enabled = true;
        coroutine = null;
        yield break;

    }



}
