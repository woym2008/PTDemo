using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TouchTileEvent : InputEvent
    {
        public override int EventID
        {
            get
            {
                return 10;
            }
        }        
    }
}