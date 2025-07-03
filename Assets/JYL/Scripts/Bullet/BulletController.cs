using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class BulletController : MonoBehaviour
    {
        [Range(0.05f, 1f)][SerializeField] float ticTime = 0.5f;
        [SerializeField] ParticleSystem destroyParticle; // TODO: �Ѿ� ������ �ϳ��ϳ� �� ���� �޾���� ��
        public Rigidbody rig;
        private Collider col;
        public int attackPower;
        public bool canDeactive = true;
        private float timer;
        void Awake()
        {
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
        }
        private void Update()
        {
            if(!canDeactive)
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
        private void OnTriggerStay(Collider other)
        {
            //���ǿ� ���� SetActive false
            // ������ ����ũ ������
            Enemy enemy = other.GetComponent<Enemy>();
            
            if (enemy == null)
            {
                gameObject.SetActive(false);
                return;
            }
            if(!canDeactive && timer<=0)
            {
                enemy.TakeDamage(attackPower);
            }
            else if(canDeactive)
            {
                enemy.TakeDamage(attackPower);
                gameObject.SetActive(false);
            }
            
        }
        private void OnDisable()
        {
            if(destroyParticle != null)
            {
                destroyParticle.Play(true);
            }
            canDeactive = true;
            timer = ticTime;
        }
    }
}
