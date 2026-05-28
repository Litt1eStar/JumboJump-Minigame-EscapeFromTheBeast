using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class Input2DManager : MonoBehaviour
    {
        public float xInput { get; private set; }
        public float yInput { get; private set; }  
        
        public void Initialize()
        {
            GameContext.Instance.Add(this);
        }

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
