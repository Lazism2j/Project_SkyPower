using UnityEngine;

namespace JYL
{
    public class PooledObject : MonoBehaviour
    {
        public ObjectPool returnPool;
        public void ReturnToPool()
        {
            returnPool.ReturnToPool(this);
        }
        public void ReturnToPool(float returnTime)
        {
            returnPool.ReturnToPool(this, returnTime);
        }
    }
}

