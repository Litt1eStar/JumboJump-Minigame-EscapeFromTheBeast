using Assets.Scripts.EFTB.GameData;
using Assets.Scripts.EFTB.Manager;
using Assets.Scripts.EFTB.Utilities;
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
            if(collision.CompareTag("Player") && !isCollected)
            {
                GameContext.Instance.Get<CollectibleManager>().AddCoin(collectibleData.Value);
                isCollected = true;

                //Play Collection Effect
                //Play Collection Sound
                //Play Collection Animation
                //Destroy gameobject after animation is done
                Destroy(gameObject);
            }
        }
    }
}
