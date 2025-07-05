using System;
using UnityEngine;

namespace JYL
{
    public class BulletController : MonoBehaviour
    {
        [Range(0.05f, 1f)][SerializeField] float ticTime = 0.5f;
        [SerializeField] GameObject flash; // TODO: �Ѿ� ������ �ϳ��ϳ� �� ���� �޾���� ��
        [SerializeField] GameObject hit;
        public Rigidbody rig;
        private Collider col;
        private ParticleSystem ps;
        public int attackPower;
        public bool canDeactive = true;
        private float timer;
        void Awake()
        {
            ps = GetComponent<ParticleSystem>();
            if(ps == null)
            {
                Debug.LogError("��ƼŬ �ý����� ����");
            }
            rig = GetComponent<Rigidbody>();
            col = GetComponent<Collider>();
            rig.constraints = RigidbodyConstraints.FreezeRotation;
            rig.useGravity = false;
            col.isTrigger = true;
            timer = ticTime;
        }
        private void OnEnable()
        {
            timer = ticTime;
            OnFire();
        }
        private void Update()
        {
            if (!canDeactive)
            {
                if (timer > 0)
                {
                    timer -= Time.deltaTime;
                }
                else if (timer < 0)
                {
                    timer = ticTime;
                }
            }
        }
        //private void OnTriggerStay(Collider other)
        //{
        //if(gameObject.layer == 7) // �÷��̾�
        //{
        //    //���ǿ� ���� SetActive false
        //    // ������ ����ũ ������
        //    Enemy enemy = other.GetComponent<Enemy>();
        //    if (enemy == null)
        //    {
        //        gameObject.SetActive(false);
        //        return;
        //    }
        //    if (!canDeactive && timer <= 0)
        //    {
        //        enemy.TakeDamage(attackPower);
        //    }
        //    else if (canDeactive)
        //    {
        //        enemy.TakeDamage(attackPower);
        //        gameObject.SetActive(false);
        //    }
        //}
        //else if(gameObject.layer == 9)
        //{
        //    // ���� �Ѿ� ��� ó��
        //    PlayerController enemy =other.GetComponent<PlayerController>();
        //    if (enemy == null)
        //    {
        //        gameObject.SetActive(false);
        //        return;
        //    }
        //    if (!canDeactive && timer <= 0)
        //    {
        //        //enemy.TakeDamage(attackPower); // TODO:�÷��̾� �����ʿ�
        //    }
        //    else if (canDeactive)
        //    {
        //        //enemy.TakeDamage(attackPower); // TODO:�÷��̾� �����ʿ�
        //        gameObject.SetActive(false);
        //    }
        //}

        private void OnTriggerStay(Collider other)
        {
            //if (gameObject.layer == 7) // �÷��̾��� �Ѿ��� ���
            //{
            //    Enemy enemy = other.GetComponent<Enemy>();
            //    if (enemy == null)
            //    {
            //        SpawnHitEffect(other.transform);
            //        gameObject.SetActive(false);
            //        return;
            //    }
            //    if (!canDeactive && timer <= 0)
            //    {
            //        enemy.TakeDamage(attackPower);
            //        SpawnHitEffect(other.transform);
            //    }
            //    else if (canDeactive)
            //    {
            //        enemy.TakeDamage(attackPower);
            //        SpawnHitEffect(other.transform);
            //        gameObject.SetActive(false);
            //    }
            //}
            Debug.Log("Ʈ���� �ν�");
            if (gameObject.layer == 9) // ���ʹ��� �Ѿ��� ���
            {
                Debug.Log("�Ѿ��� ���� �Ѿ��� ���.");
                PlayerController player = other.GetComponent<PlayerController>();
                if (player == null)
                {
                    SpawnHitEffect(other.transform);
                    gameObject.SetActive(false);
                    return;
                }
                if (!canDeactive && timer <= 0)
                {
                    //player.TakeDamage(attackPower); // TODO:�÷��̾� �����ʿ�
                    SpawnHitEffect(other.transform);
                }
                else if (canDeactive)
                {
                    //player.TakeDamage(attackPower); // TODO:�÷��̾� �����ʿ�
                    SpawnHitEffect(other.transform);
                    gameObject.SetActive(false);
                }
            }
        }
        private void OnDisable(){ }
        private void OnFire()
        {
            ps.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            ps.Play();
            if (flash != null)
            {
                GameObject flashInstance = Instantiate(flash, transform.position, Quaternion.identity,transform);
                flashInstance.transform.forward = gameObject.transform.forward;
                if(flashInstance == null)
                {
                    Debug.Log("�÷��� ���ӿ�����Ʈ ���� ���� NUll");
                }
                ParticleSystem flashPs = flashInstance.GetComponent<ParticleSystem>();
                if (flashPs != null)
                {
                    Destroy(flashInstance, flashPs.main.duration);
                }
                else if(flashPs == null)
                {
                    Debug.Log("�÷��� ��ƼŬ �ý��� ������Ʈ�� NUll");
                    ParticleSystem flashPsParts = flashInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                    Destroy(flashInstance, flashPsParts.main.duration);
                    if(flashPsParts == null)
                    {
                        Debug.Log("�÷����� �ڽĵ� ��ƼŬ �ý��� ������Ʈ�� Null");
                    }
                    
                }
                
            }
            else if(flash == null)
            {
                Debug.Log($"�÷��� ������ ������ NUll");
            }
        }

        private void SpawnHitEffect(Transform parent)
        {
            if (hit != null)
            {
                // �浹 ��ġ�� ���� ���
                Vector3 hitPos = transform.position;
                Quaternion hitRot = Quaternion.identity;
                if (parent != null)
                {
                    hitRot = Quaternion.LookRotation(parent.position - transform.position);
                }
                else if(parent == null)
                {
                    Debug.Log("�浹ü�� null��");
                }
                GameObject hitInstance = Instantiate(hit, hitPos, hitRot, parent);
                if(hitInstance == null)
                {
                    Debug.Log("��Ʈ ���� ������Ʈ ���� ����");
                }
                ParticleSystem hitPs = hitInstance.GetComponent<ParticleSystem>();
                if (hitPs != null)
                {
                    hitPs.Play();
                    Destroy(hitInstance, hitPs.main.duration);
                }
                else if(hitPs == null)
                {
                    Debug.Log("��Ʈ ��ƼŬ �ý����� Null��");
                    if (hitInstance.transform.childCount > 0)
                    {
                        var hitPsParts = hitInstance.transform.GetChild(0).GetComponent<ParticleSystem>();
                        if (hitPsParts == null)
                        {
                            Debug.Log("��Ʈ : �ڽĿ��Լ��� ��ƼŬ �ý����� ã�� �� ����");
                        }
                        hitPsParts.Play();
                        Destroy(hitInstance, hitPsParts.main.duration);
                    }
                }

            }
            else
            {
                Debug.Log("��Ʈ ���ӿ�����Ʈ ������ null");
            }
        }
    }
}
