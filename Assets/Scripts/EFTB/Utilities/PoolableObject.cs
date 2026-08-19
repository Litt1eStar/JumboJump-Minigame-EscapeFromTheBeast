using UnityEngine;

namespace JumboJumps.EFTB.Utilities
{
    public class PoolableObject : MonoBehaviour
    {
        /// <summary>
        /// Attached this script to prefab that working with Object Pool
        /// </summary>

        public string PoolKey { get; set; }
        
        public virtual void OnSpawn()
        {
            gameObject.SetActive(true);
        }

        public virtual void OnRecycle()
        {
            gameObject.SetActive(false);
        }
    }
}
