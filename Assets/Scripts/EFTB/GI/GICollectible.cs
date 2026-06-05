using JumboJumps.EFTB.GameData;
using JumboJumps.EFTB.Manager;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GICollectible : MonoBehaviour
    {
        [SerializeField]
        private CollectibleSO collectibleData;

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
                GameContext.Instance.Get<CollectibleManager>().AddValue(collectibleData.Value);
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
