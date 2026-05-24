using Assets.Scripts.EFTB.Visualizer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.EFTB.State.Player
{
    public class PlayerMovementController
    {
        private PlayerVisualizer visualizer;
        public void Initialize()
        {
            visualizer = new PlayerVisualizer();
        }

        public void Dispose()
        {

        }
    }
}
