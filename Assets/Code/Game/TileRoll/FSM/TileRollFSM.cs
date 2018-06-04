using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class TileRollFSM
    {
        ITRState m_CurState;
        public TileRollFSM()
        {
            m_CurState = null;
        }

        public ITRState GetState
        {
            get
            {
                return m_CurState;
            }
        }


        public void SetState(ITRState state)
        {
            if(m_CurState != null)
            {
                m_CurState.Exit();
            }

            m_CurState = state;

            m_CurState.Enter();
        }

        public void FrameUpdate(float dt)
        {
            if(m_CurState != null)
            {
                m_CurState.Execute(dt);
            }
        }
    }
}
