using JumboJumps.EFTB.GI;
using JumboJumps.EFTB.Utilities;
using UnityEngine;

namespace JumboJumps.EFTB.Visualizer
{
    public class PlayerVisualizer
    {
        private GIPlayer giPlayer;
        public GICamera giCamera { get; private set;}
        public void Initialize()
        {
            giPlayer = SceneObjectContext.Instance.Get<GIPlayer>();
            if(giPlayer == null)
            {
                DebugLogHelper.LogError("GIPlayer not found in SceneObjectContext. PlayerVisualizer initialization failed.");
            }

            giCamera = SceneObjectContext.Instance.Get<GICamera>(); 
            if(giCamera == null)
            {
                DebugLogHelper.LogError("GICamera not found in SceneObjectContext. PlayerVisualizer initialization failed.");
            }
        }
        public void Dispose()
        {
            giPlayer = null;
        }
        public void Move(float input)
        {
            giPlayer.Move(input);
        }
        public void Idle()
        {
            //Idle Logic
        }   
    }
}
