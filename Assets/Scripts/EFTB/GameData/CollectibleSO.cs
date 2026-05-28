using UnityEngine;

namespace JumboJumps.EFTB.GameData
{
    [CreateAssetMenu(fileName = "CollectibleData", menuName = "CollectibleSO")]
    public class CollectibleSO : ScriptableObject
    {
        [SerializeField]
        private Sprite collectibleSprite;
        [SerializeField]
        private int value;

        public Sprite CollectibleSprite => collectibleSprite;
        public int Value => value;  
    }
}
