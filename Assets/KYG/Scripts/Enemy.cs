using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.KYG.Scripts
{
    public class Enemy : MonoBehaviour
    {
        [SerializeField] private float speed;

        public GameManagerSO gameManager;
        public ObjectPoolManagerSO poolManager;
        public string poolKey = "Enemy"; // Ǯ Ű

        void Update()
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);

            if (transform.position.y < -6f)
            {
                poolManager.Despawn(poolKey, gameObject);
            }
        }

        private void OnTriggerEnter(Collider collision) // 3D�� �ݶ��̴� �̺�Ʈ
        {
            if (collision.CompareTag("Bullet"))
            {
                // �Ѿ� ��Ȱ��ȭ
                poolManager.Despawn("Bullet", collision.gameObject);

                gameManager.AddScore(100);

                // �� ��Ȱ��ȭ (Ǯ ��ȯ)
                poolManager.Despawn(poolKey, gameObject);
            }
        }
    }
}