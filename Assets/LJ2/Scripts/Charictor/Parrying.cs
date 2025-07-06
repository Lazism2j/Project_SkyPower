using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parrying : MonoBehaviour
{
    [SerializeField] bool isParrying = false;
    [SerializeField] float parryRadius = 1.1f;
    [SerializeField] LayerMask enemyBullet;
    [SerializeField] Collider characterCollider;

    private Coroutine parryCoroutine;
    public YieldInstruction coroutineDelay;
    [SerializeField] float invincibleTime = 1;

    [SerializeField] int shield = 3;

    public void Awake()
    {
        characterCollider = GetComponent<Collider>();
        coroutineDelay = new WaitForSeconds(invincibleTime);
        enemyBullet = LayerMask.GetMask("EnemyBullet");
    }

    public void Parry()
    {
        // enemyBullet ���̾ �����ϵ��� ����
        Collider[] canParry = Physics.OverlapSphere(transform.position, parryRadius*10f, 1<<9);
        Debug.Log($"�и��ؼ� ���� ������� ����{canParry.Length}");
        if (canParry.Length > 0)
        {
            isParrying = true;

            foreach (Collider c in canParry)
            {
                Debug.Log($"{c.gameObject.name} �и� �浹ü �̸�");
                c.gameObject.SetActive(false); // Deactivate enemy bullets
            }
        }
        else
        {
            isParrying = false; // No bullets to parry
        }
    }

    public void Invicible()
    {   
        if (isParrying)
        {
           parryCoroutine = StartCoroutine(InvincibleCoroutine());
        }
        isParrying = false; // Reset parrying state
    }


    private IEnumerator InvincibleCoroutine()
    {
        Debug.Log("�ڷ�ƾ ����");

        characterCollider.enabled = false;
        yield return coroutineDelay;
        characterCollider.enabled = true;
        parryCoroutine = null;
        yield break;

    }

    public int Shield()
    {
        int getShield = 0; // Default shield value
        if (isParrying)
        {
            getShield = shield;
        }
        isParrying = false; // Reset parrying state after getting shield
        return getShield;
    }

    // �ݻ�� �Ѿ��� ������ �߻��ϴ� ����� ���� �����Ǿ� ���� �ʽ��ϴ�.    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, parryRadius);
    }
}
