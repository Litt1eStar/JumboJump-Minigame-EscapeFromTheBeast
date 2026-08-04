using System.Collections.Generic;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIAggressiveCat : GICat
    {
        [Header("Aggressive Cat Visual Reference")]
        [SerializeField]
        private Transform catHand;

        [SerializeField]
        private Collider2D smashCollider;

        public Transform CatHand => catHand;
        public Collider2D SmashCollider => smashCollider;

        public Vector3? TargetSmashPosition { get; private set; }

        public void SetTargetSmashPosition(Vector3 position)
        {
            TargetSmashPosition = position;
        }

        public void ClearTargetSmashPosition()
        {
            TargetSmashPosition = null;
        }

        private void OnDisable()
        {
            ClearTargetSmashPosition();
        }

        public void SetHandActive(bool active)
        {
            if (catHand != null)
            {
                catHand.gameObject.SetActive(active);
            }
        }

        public void SetHandPosition(Vector3 position)
        {
            if (catHand != null)
            {
                catHand.position = position;
            }
        }

        public void SetHandRotation(Quaternion rotation) 
        {
            if (catHand != null)
            {
                catHand.rotation = rotation;
            }
        }

        public bool CheckPlayerCollision()
        {
            ContactFilter2D filter = ContactFilter2D.noFilter;
            List<Collider2D> results = new List<Collider2D>();
            smashCollider.Overlap(filter, results);

            foreach (var col in results)
            {
                if (col != null && col.GetComponent<GIPlayer>() != null)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
