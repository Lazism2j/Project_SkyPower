using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ultimate : MonoBehaviour
{
    public Coroutine ultRoutine;
    public float setUltDelay;
    public YieldInstruction ultDelay;
    public LayerMask enemyBullet;

    public GameObject ultLaser; // UltLaserController component�� ��������
    public GameObject shield;   // ShieldController component�� ��������
    public GameObject ultAll;   // AllAttackController component�� ��������

    public int defense = 1;

    public void Awake()
    {
        ultDelay = new WaitForSeconds(setUltDelay);
        enemyBullet = LayerMask.GetMask("EnemyBullet");
    }

    public void Laser()
    {
        if (ultRoutine == null)
        {
            ultRoutine = StartCoroutine(LaserCoroutine());
        }
        else
        {
            return;
        }
    }
    private IEnumerator LaserCoroutine()
    {
        ultLaser.SetActive(true);
        Debug.Log("Laser Active");
        yield return ultDelay;

        ultLaser.SetActive(false);
        Debug.Log("Laser Off");
        ultRoutine = null;
        yield break;
    }

    public int Shield()
    {
        if (ultRoutine == null)
        {
            ultRoutine = StartCoroutine(ShieldCoroutine());
        }
        return defense;
    }
    private IEnumerator ShieldCoroutine()
    {
        shield.SetActive(true);
        Debug.Log("Shield Active");
        yield return ultDelay;

        shield.SetActive(false);
        Debug.Log("Shield Off");
        ultRoutine = null;
        yield break;
    }

    public void AllAttack()
    {
        Collider[] hits = Physics.OverlapBox(ultAll.transform.position, ultAll.transform.localScale / 2f, Quaternion.identity, enemyBullet);

        foreach (Collider c in hits)
        {
            c.gameObject.SetActive(false);
        }
        ultAll.SetActive(true);
        hits = null;
        ultAll.SetActive(false);
    }

    // ����ź

    // �ñر� ź�� 1ȸ + �ٴ���Ʈ

    // ź�� ���� + ������ ����
}
