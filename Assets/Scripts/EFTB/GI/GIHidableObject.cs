using System;
using UnityEngine;

namespace JumboJumps.EFTB.GI
{
    public class GIHidableObject : MonoBehaviour
    {
        public event Action<GIPlayer> EventPlayerEntered;
        public event Action<GIPlayer> EventPlayerExited;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var player = collision.GetComponent<GIPlayer>();
            if (player != null)
            {
                EventPlayerEntered?.Invoke(player);
            }   
        }

        private void OnTriggerExit2D(Collider2D collision)
        {    
            var player = collision.GetComponent<GIPlayer>();
            if (player != null)
            {
                EventPlayerExited?.Invoke(player);
            }
        }
    }
}
