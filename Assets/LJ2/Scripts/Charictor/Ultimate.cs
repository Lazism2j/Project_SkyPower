using JYL;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Ultimate : MonoBehaviour
{
    public Coroutine ultRoutine;
    public float setUltDelay;
    public YieldInstruction ultDelay;

    public PlayerController playerController;
    [Range(0.1f, 5)][SerializeField] float bulletReturnTimer = 5f;
    [Range(0.1f, 30)][SerializeField] float bulletSpeed = 15f;
    [Range(0.1f, 3)][SerializeField] float ultBulletTime = 5f;

    [SerializeField] float bulletUpgradeTime = 5f;

    public LayerMask enemyBullet;

    public GameObject ultLaser;
    public UltLaserController ultLaserController;

    public GameObject shield;
    public UltShieldController ultShieldController;

    public GameObject ultAll;
    public UltMapAttack ultAllController;

    public GameObject ultFire; // Fire ������ ���� (�߰��� �κ�)
    public UltLaserController ultFireController; // Fire ��Ʈ�ѷ� (�߰��� �κ�)

    public int defense = 1;

    public void Awake()
    {
        playerController = GetComponent<PlayerController>();
        ultDelay = new WaitForSeconds(setUltDelay);
        enemyBullet = LayerMask.GetMask("EnemyBullet");
        ultLaserController = ultLaser.GetComponentInChildren<UltLaserController>();
        ultShieldController = shield.GetComponentInChildren<UltShieldController>();
        ultAllController = ultAll.GetComponent<UltMapAttack>();
        ultFireController = ultFire.GetComponentInChildren<UltLaserController>(); // Fire ��Ʈ�ѷ� �ʱ�ȭ (�߰��� �κ�) 
    }

    public void Laser(float damage)
    {
        if (ultRoutine == null)
        {
            ultLaserController.AttackDamage(damage);
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
        //PlayerController.canAttack = false; // ���� �Ұ� ���·� ����
        Debug.Log("Laser Active");
        yield return ultDelay;

        ultLaser.SetActive(false);
        //PlayerController.canAttack = true; // ���� ���� ���·� ����
        Debug.Log("Laser Off");
        ultRoutine = null;
        yield break;
    }

    public void Fire(float damage)
    {
        if (ultRoutine == null)
        {
            ultFireController.AttackDamage(damage);
            ultRoutine = StartCoroutine(FireCoroutine());
        }
        else
        {
            return;
        }
    }
    private IEnumerator FireCoroutine()
    {
        ultFire.SetActive(true);
        //PlayerController.canAttack = false; // ���� �Ұ� ���·� ����
        Debug.Log("Laser Active");
        yield return ultDelay;

        ultFire.SetActive(false);
        //PlayerController.canAttack = true; // ���� ���� ���·� ����
        Debug.Log("Laser Off");
        ultRoutine = null;
        yield break;
    }

    public int Shield(float damage)
    {
        if (ultRoutine == null)
        {
            ultShieldController.AttackDamage(damage);
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

    public void AllAttack(float damage)
    {
        if (ultRoutine == null)
        {
            ultAllController.AttackDamage(damage);
            ultRoutine = StartCoroutine(EraseCoroutine());
        }
        else
        {
            return;
        }
    }

    private IEnumerator EraseCoroutine()
    {
        Collider[] hits = Physics.OverlapBox(ultAll.transform.position, ultAll.transform.localScale / 2f, Quaternion.identity, enemyBullet);

        foreach (Collider c in hits)
        {
            c.gameObject.SetActive(false);
        }
        ultAll.SetActive(true);

        yield return ultDelay;
        ultAll.SetActive(false);
        hits = null;
        ultRoutine = null;
        Debug.Log("�ڷ�ƾ ����");

        yield break;
    }

    // �ñر� ź�� 1ȸ + �ٴ���Ʈ
    public void BigBullet(float damage)
    {
        playerController.poolIndex = 1;
        BulletPrefabController bulletPrefab = playerController.curBulletPool.ObjectOut() as BulletPrefabController;
        bulletPrefab.transform.position = playerController.muzzlePoint.position;
        bulletPrefab.ReturnToPool(bulletReturnTimer);

        if (bulletPrefab.bulletInfo[0].rig != null)
        {
            bulletPrefab.bulletInfo[0].trans.gameObject.SetActive(true);
            bulletPrefab.bulletInfo[0].trans.localPosition = bulletPrefab.bulletInfo[0].originPos;
            bulletPrefab.bulletInfo[0].rig.velocity = Vector3.zero;
            bulletPrefab.bulletInfo[0].bulletController.attackPower = (int)damage;
            bulletPrefab.bulletInfo[0].rig.AddForce(bulletSpeed * bulletPrefab.bulletInfo[0].trans.forward, ForceMode.Impulse); // �� �κ��� Ŀ�����ϸ� ��
            bulletPrefab.bulletInfo[0].canDeactive = false; // �ٴ���Ʈ�̹Ƿ� false�� ����
        }

        playerController.poolIndex = 0; // �ٽ� �⺻ �Ѿ˷� ����

    }

    // ź�� ���� + ������ ����
    public void BulletUpgrade()
    {
        if(ultRoutine == null)
        {
            ultRoutine = StartCoroutine(UpgradeRoutine());
        }
        else
        {
            return;
        }

    }

    public IEnumerator UpgradeRoutine()
    {
        playerController.poolIndex = 1;
        Debug.Log("Upgrade Bullet Shot");
        yield return ultDelay;

        playerController.poolIndex = 0;
        Debug.Log("Normal Bullet Shot");
        ultRoutine = null;
        yield break;
    }
}
