using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class GameTrack
    {
        //is Local Position
        //point need >=2 
        //first is startpoint last is endpoint
        List<GameTrackFragment> m_FragmentList;

        //
        //JudgmentArea

        //public TrackPath GetPath(float movetime, Vector3 endpos)
        //{
        //    int length = m_PointList.Count + 1;
        //    Vector3[] p = new Vector3[length];
        //    for(int i=0;i< m_PointList.Count; ++i)
        //    {
        //        p[i] = m_PointList[i].m_Point;
        //    }

        //    TrackPath path = new TrackPath(p);
        //    path.AddPoint(endpos);
        //}

        public void AddTrackFragment()
        {

        }

        public void RemoveTrackFragment()
        {

        }

        public void RemovePoint()
        {
            //m_PointList.Remove();
        }
    }
}
