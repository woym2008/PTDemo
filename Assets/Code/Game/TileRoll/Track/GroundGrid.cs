using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class GroundRow
    {
        public List<GroundGrid> m_Grids;

        Vector3 m_Pos;

        public float m_CurTime;

        public GroundRow(GameObject obj)
        {
            m_Grids = new List<GroundGrid>();
            for(int i=0;i< 1; ++i)
            {
                m_Grids.Add(new GroundGrid(obj));
            }

            m_CurTime = 0;
        }

        public void UpdateSpeed(Vector3 pos)
        {
            //m_Pos += offset;
            for (int i = 0; i < m_Grids.Count; ++i)
            {
                m_Grids[i].UpdatePos(pos);
            }
        }

        public void SetBornHeight(float h)
        {
            //m_Pos += offset;
            for (int i = 0; i < m_Grids.Count; ++i)
            {
                m_Grids[i].SetBornHeight(h);
            }
        }
    }
    public class GroundGrid
    {
        GameObject m_Obj;
        public GroundGrid(GameObject obj)
        {
            m_Obj = obj;
        }

        //public void AttachObject(GameObject obj)
        //{
        //    m_Obj = obj;
        //}

        //public void RemoveObject()
        //{
        //    m_Obj = null;
        //}

        public void UpdatePos(Vector3 pos)
        {
            if(m_Obj != null)
            {
                float offset = pos.y - m_BornHeight;
                if (Mathf.Abs(offset) > minoffset)
                {
                    m_BornHeight = m_BornHeight + offset * 0.1f;
                }

                m_Obj.transform.position = new Vector3(pos.x, m_BornHeight - 0.1f, pos.z);
            }
        }

        float m_BornHeight = 0;
        float minoffset = 0.01f;
        public void SetBornHeight(float h)
        {
            m_BornHeight = h;
        }
    }
}
