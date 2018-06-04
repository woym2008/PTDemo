using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class TRStopState : ITRState
    {
        TileRoll m_TR;
        public TRStopState(TileRoll tr)
        {
            m_TR = tr;
        }

        public void Enter()
        {
        }

        public void Execute(float dt)
        {
        }

        public void Exit()
        {
        }
    }
}
