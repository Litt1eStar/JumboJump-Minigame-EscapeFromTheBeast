using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EFTB.Manager
{
    public class Input2DManager : MonoBehaviour
    {
        public float xInput { get; private set; }
        public float yInput { get; private set; }  
        
        public void UpdateLogic(float deltaTime)
        {
            MovementInputHandler();
        }

        private void MovementInputHandler()
        {
            xInput = Input.GetAxis("Horizontal");
            yInput = Input.GetAxis("Vertical");
        }
    }
}
