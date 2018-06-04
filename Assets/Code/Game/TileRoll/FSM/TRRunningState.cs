using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class TRRunningState : ITRState
    {
        TileRoll m_TR;

        float m_RunningTime = 0;
        float m_StartTime = 0;

        public float basespeed = 1.0f;

        public float m_TileSpeed = 1.0f;

        public TRRunningState(TileRoll tr)
        {
            m_TR = tr;
        }

        public void Reset()
        {
            m_StartTime = 0;
            m_RunningTime = -m_TR.m_RollTime;
        }

        public void Enter()
        {
            Reset();

            foreach (var t in m_TR.m_Tracks)
            {
                t.m_bEnableTrack = true;
            }
        }

        public void Execute(float dt)
        {
            m_StartTime += dt * basespeed;
            m_RunningTime += dt * basespeed;

            if(m_TR.m_CacheSpawner.Count > 0)
            {
                TileSpawner pSpawner = m_TR.m_CacheSpawner.Peek();
                if (pSpawner.getStartTime <= m_StartTime)
                {
                    AttachBlock(pSpawner);
                    m_TR.m_CacheSpawner.Dequeue();
                }
            }            

            for (int i = 0; i < m_TR.m_Tracks.Count; ++i)
            {
                m_TR.m_Tracks[i].FrameUpdate(m_RunningTime);
            }


        }

        public void Exit()
        {
            foreach (var t in m_TR.m_Tracks)
            {
                t.m_bEnableTrack = false;
            }
        }
        //---------------------------------------------------
        void AttachBlock(TileSpawner bs)
        {
            TouchTileBase pTile = bs.CreateTile();

            if (pTile != null)
            {                
                //pTile.AttachParent(this.m_TR.GetView.transform);

                pTile.SetSpeed(m_TileSpeed);
                
                //start pos

                //start rot
            }

            //if (m_bEnableAuto)
            //{
            //    pBeat.AutoTouch();
            //}

            if (m_TR.m_Tracks.Count > 0)
            {
                List<TileTrack> tracks = new List<TileTrack>();
                for (int i = 0; i < m_TR.m_Tracks.Count; ++i)
                {
                    if (!m_TR.m_Tracks[i].IsUseing)
                    {
                        tracks.Add(m_TR.m_Tracks[i]);
                    }
                }

                int index = UnityEngine.Random.Range(0, tracks.Count);
                for (int i = 0; i < m_TR.m_Tracks.Count; ++i)
                {
                    m_TR.m_Tracks[i].IsUseing = false;
                }
                tracks[index].IsUseing = true;

                tracks[index].AddTile(pTile);

                pTile.AttachParent(tracks[index]);
            }
        }
    }
}
