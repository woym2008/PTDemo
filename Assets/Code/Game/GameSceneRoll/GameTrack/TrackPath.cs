using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TrackPath
    {
        List<Vector3> m_Points;

        public TrackPath(Vector3[] p)
        {
            m_Points = new List<Vector3>();

            for (int i=0;i<p.Length;++i)
            {
                m_Points.Add(p[i]);
            }
        }

        public void AddPoint(Vector3 pos)
        {
            m_Points.Add(pos);
        }
    }
}
