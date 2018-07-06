using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo.TrackNormal
{
    public class TileTrack
    {
        int m_TrackID;
        //y
        float m_Length;
        //x
        float m_Width;

        float m_Thickness;

        //local
        Vector3 m_Offset;

        float m_RollTime;

        public bool m_bEnableTrack = false;

        Transform m_TrackRoot = null;
        public Transform Root
        {
            get
            {
                return m_TrackRoot;
            }
        }

        ITileTrackView m_TrackView;

        public List<TouchTileBase> m_RunningTiles;

        public bool IsUseing = false;


        public float Length
        {
            get
            {
                return m_Length;
            }
        }


        public float Width
        {
            get
            {
                return m_Width;
            }
        }

        public float Thickness
        {
            get
            {
                return m_Thickness;
            }
        }

        public Vector3 Offset
        {
            get
            {
                return m_Offset;
            }
        }

        public float RollTime
        {
            get
            {
                return m_RollTime;
            }
        }

        //EndingArea
        public TileTrack(int id, Transform root = null)
        {
            m_TrackID = id;

            m_RunningTiles = new List<TouchTileBase>();

            m_TrackRoot = root;

            m_TrackView = InstanceTrackView();
            m_TrackView.SetTrack(this);
        }

        //void SetStartPos(Vector3 locpos)
        //{
        //    StartPos = locpos;
        //}
        public void SetLength(float length)
        {
            m_Length = length;
        }

        public void SetWidth(float width)
        {
            m_Width = width;
        }

        public void SetThickness(float t)
        {
            m_Thickness = t;
        }

        public void SetOffset(Vector3 offset)
        {
            m_Offset = offset;
        }

        public void SetTime(float t)
        {
            m_RollTime = t;
        }

        public void FrameUpdate(float realpassedtime)
        {
            //for(int i= m_RunningTiles.Count-1; i>=0 ; --i)
            //{
            //    m_RunningTiles[i].FrameUpdate(realpassedtime);
            //    if(!m_RunningTiles[i].IsUseing)
            //    {
            //        m_RunningTiles.Remove(m_RunningTiles[i]);
            //    }
            //}
        }

        public void AddTile(TouchTileBase ptile)
        {
            m_RunningTiles.Add(ptile);
        }

        public Vector3 GetPos(float passedtime)
        {
            if(m_TrackView != null)
            {
                Vector3 worldpos = m_TrackView.GetPosition(passedtime);
                //if(m_TrackRoot != null)
                //{
                //    return m_TrackRoot.TransformVector(locpos);
                //}
                return worldpos;
            }

            return Vector3.zero;
        }

        public Quaternion GetRot(float passedtime)
        {
            if (m_TrackView != null)
            {
                Quaternion rot = m_TrackView.GetRotate(passedtime);
                //if(m_TrackRoot != null)
                //{
                //    return m_TrackRoot.TransformVector(locpos);
                //}
                return rot;
            }

            return Quaternion.identity;
        }

        private ITileTrackView InstanceTrackView()
        {
            string prefabName = "TrackRes/TrackView_0";
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            GameObject go = GameObject.Instantiate(prefab);
            ITileTrackView instance = go.GetComponent<ITileTrackView>();

            if (instance == null)
            {
                Debug.LogError("null track view" + prefabName);
            }

            return instance;
        }
        //----------------------------------------------------------

        private void PressTile()
        {
            Debug.Log("PressTile");
        }

        private void ReleaseTile()
        {
            Debug.Log("ReleaseTile");
        }
    }
}
