using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class PressTrack : InputEvent
    {
        public override int EventID
        {
            get
            {
                return 1;
            }
        }
    }
}
