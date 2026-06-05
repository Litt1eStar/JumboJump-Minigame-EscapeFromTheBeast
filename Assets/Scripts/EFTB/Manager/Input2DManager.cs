using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Manager
{
    public class Input2DManager : MonoBehaviour
    {
        [SerializeField]
        private KeyCode changeStateKey = KeyCode.Space; // For testing purpose, can be removed later

        public float XInput { get; private set; }
        public float YInput { get; private set; }  
        
        public void Initialize()
        {
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
        }

        public void UpdateLogic(float deltaTime)
        {
            MovementInputHandler();
        }

        private void MovementInputHandler()
        {
            XInput = Input.GetAxis("Horizontal");
            YInput = Input.GetAxis("Vertical");
        }

        /// <summary>
        /// For testing purpose
        /// </summary>
#if UNITY_EDITOR
        public bool IsChangeState()
        {
            return Input.GetKeyDown(changeStateKey);
        }
#endif
    }
}
