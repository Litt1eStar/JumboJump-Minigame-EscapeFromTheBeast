using Assets.Scripts.EFTB.GameData;
using UnityEngine;

namespace Assets.Scripts.EFTB.GI
{
    public class GICollectible : MonoBehaviour
    {
        [SerializeField]
        private CollectibleSO collectibleData;

        private SpriteRenderer spriteRenderer;
        private bool isCollected = false;   

        public void Initialize()
        {

        }

        public void Dispose()
        {

        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            
        }
    }
}
