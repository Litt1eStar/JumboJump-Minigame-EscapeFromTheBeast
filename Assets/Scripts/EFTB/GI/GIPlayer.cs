using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EFTB.GI
{
    public class GIPlayer : MonoBehaviour
    {
        [SerializeField]
        private Transform playerTransform;

        [SerializeField]
        private Animator animator;

        [SerializeField]
        private SpriteRenderer spriteRenderer;

        [SerializeField]
        private float playerMovementSpeed = 5f;

        public void Initialize()
        {

        }
        public void Dispose()
        {

        }
        public void Move(float input)
        {
            playerTransform.position += new Vector3(input * playerMovementSpeed * Time.deltaTime, 0, 0);
        }
    }
}
