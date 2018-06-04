using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TileRollView : MonoBehaviour
    {
        TileRoll m_RollEntity;

        public void SetEntity(TileRoll tr)
        {
            m_RollEntity = tr;
        }
    }
}
