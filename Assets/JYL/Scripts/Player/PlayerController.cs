using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JYL
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] PlayerModel playerModel;
        [SerializeField] Transform muzzlePoint;
        ObjectPool objectPool;

        private void Awake()
        {
            objectPool = GetComponent<ObjectPool>();
        }
        private void Update()
        {
            
        }

        private void FireBullet()
        {
            // ���⼭ ������Ʈ Ǯ���� ���� ����, �߻�
        }
    }
}

