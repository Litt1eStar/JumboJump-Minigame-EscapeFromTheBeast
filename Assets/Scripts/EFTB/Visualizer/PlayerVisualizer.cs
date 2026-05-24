using Assets.Scripts.EFTB.GI;
using Assets.Scripts.EFTB.Utilities;
using UnityEngine;

namespace Assets.Scripts.EFTB.Visualizer
{
    public class PlayerVisualizer
    {
        private GIPlayer giPlayer;
        public void Initialize()
        {
            giPlayer = SceneObjectContext.Instance.Get<GIPlayer>();

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
