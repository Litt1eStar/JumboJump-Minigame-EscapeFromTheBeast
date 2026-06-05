using JumboJumps.EFTB.UI;
using JumboJumps.EFTB.Utilities;

namespace JumboJumps.EFTB.Visualizer
{
    public class GameVisualizer
    {
        public void Initialize()
        {   
            GameContext.Instance.Add(this);
        }

        public void Dispose()
        {
            GameContext.Instance.Remove(this);
        }
    }
}
