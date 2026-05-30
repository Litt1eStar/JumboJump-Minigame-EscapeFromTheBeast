using JumboJumps.EFTB.State;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JumboJumps.EFTB.Manager
{
    public class GameManager
    {
        private GameStateController stateController;

        public void Initialize()
        {
            stateController = new GameStateController();
            stateController?.Initialize();
            stateController?.StartStateController();
        }

        public void Dispose()
        {
            stateController?.Dispose();
            stateController = null;
        }
    }
}
