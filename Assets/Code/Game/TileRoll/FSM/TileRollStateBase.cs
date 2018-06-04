using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public interface ITRState
    {
        void Enter();

        void Execute(float dt);

        void Exit();
    }
}
