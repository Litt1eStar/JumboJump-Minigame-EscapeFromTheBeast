using JumboJumps.EFTB.Utilities;

namespace JumboJump.Assets.Scripts.EFTB.Manager
{
    public class GameplayStateManager
    {
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

        }
    }
}
