using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Set Scriptable Object")]
        [SerializeField] PlayerModel playerModel;

        [Header("Set References")]
        [SerializeField] Transform muzzlePoint;

        [Header("Set Value")]
        [SerializeField] float bulletReturnTimer = 2f;
        [SerializeField] List<ObjectPool> bulletPools;

        private int level;
        private int hp;

        private int poolIndex = 0;
        private ObjectPool curBulletPool => bulletPools[poolIndex];

        private void OnEnable()
        {
            //CreatePlayer();
        }
        private void Update()
        {
            PlayerHandler();
            Debug.Log($"{curBulletPool}");
        }

        private void FixedUpdate()
        {
            PlayerMove();
        }

        private void LateUpdate()
        {
            // �ִϸ��̼� - �ñر� ��
        }

        private void OnCollisionEnter(Collision collision)
        {
            
        }
        //private void CreatePlayer(CharacterController character)
        //{
        // �÷��̾� ����
        //}


        private void PlayerHandler()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Fire();
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                poolIndex = 0;
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                poolIndex = 1;
            }
            if(Input.GetKeyDown(KeyCode.Alpha3))
            {
                poolIndex = 2;
            }
            //UseUlt
            //Parry
        }

        private void PlayerMove()
        {

        }

        private void Fire()
        {
            BulletPrefabController bullet = curBulletPool.ObjectOut() as BulletPrefabController;
            bullet.transform.position = muzzlePoint.position;
            bullet.ReturnToPool(bulletReturnTimer);
            foreach(BulletInfo info in bullet.bullet)
            {
                if(info.rig == null)
                {
                    continue;
                }
                info.trans.gameObject.SetActive(true);
                info.trans.position = info.originPos;
                info.rig.velocity = Vector3.zero;
                info.rig.AddForce(playerModel.fireSpeed * info.trans.forward, ForceMode.Impulse);
            }
        }
        //private void UseUlt()
        //{
             // �ñر�
        //}
        //private void Parry(CharacterController character)
        //{
              // ���� ĳ���Ϳ� ���� �и���ų ���
        //}
    }
}