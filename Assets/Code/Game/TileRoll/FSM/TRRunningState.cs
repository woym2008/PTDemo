using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class TRRunningState : ITRState
    {
        TileRoll m_TR;

        float m_RunningTime = 0;
        float m_StartTime = 0;

        public float basespeed = 1.0f;

        public float m_TileSpeed = 1.0f;

        
        //CameraPlayer m_Player;


        //-----------------------------------------------
        //临时的 活动块的管理
        public List<TouchTileBase> m_RunningTiles;

        public TRRunningState(TileRoll tr)
        {
            m_TR = tr;

            //m_Player = player;

            //----------
            m_RunningTiles = new List<TouchTileBase>();
        }

        public void Reset()
        {
            m_StartTime = 0;
            m_RunningTime = 0;
        }

        public void Enter()
        {
            Reset();

            m_TR.m_track.Start();

            

            //foreach (var t in m_TR.m_Tracks)
            //{
            //    t.m_bEnableTrack = true;
            //}
        }

        public void Execute(float dt)
        {
            m_StartTime += dt;
            m_RunningTime += dt;

            if(m_TR.m_CacheSpawner.Count > 0)
            {
                TileSpawner pSpawner = m_TR.m_CacheSpawner.Peek();
                if (pSpawner.getStartTime <= m_StartTime)
                {
                    AttachBlock(pSpawner);
                    m_TR.m_CacheSpawner.Dequeue();
                }
            }

            //for (int i = 0; i < m_TR.m_Tracks.Count; ++i)
            //{
            //    m_TR.m_Tracks[i].FrameUpdate(m_RunningTime);
            //}
            m_TR.m_track.Update();

            float param = m_RunningTime / m_TR.m_MusicTime;
            
            Vector3 curpos = m_TR.m_track.GetPosition(param, 0);
            m_TR.m_Player.position = new Vector3(curpos.x, curpos.y, curpos.z);

            m_TR.m_Player.rotation = m_TR.m_track.GetRotation(param, 0);
            //-----------------------------
            for (int i= m_RunningTiles.Count-1;i>=0;--i)
            {
                m_RunningTiles[i].FrameUpdate(param);
            }
        }

        public void Exit()
        {
            //foreach (var t in m_TR.m_Tracks)
            //{
            //    t.m_bEnableTrack = false;
            //}
            m_TR.m_track.Stop();
        }
        //---------------------------------------------------
        void AttachBlock(TileSpawner bs)
        {
            float startprocess = m_TR.m_RollTime / m_TR.m_MusicTime;
            TouchTileBase pTile = bs.CreateTile(startprocess);

            if (pTile != null)
            {                
                //pTile.AttachParent(this.m_TR.GetView.transform);

                //pTile.SetSpeed(m_TileSpeed);
                
                //start pos

                //start rot
            }

            m_TR.m_track.PushValue(pTile);

            m_RunningTiles.Add(pTile);

            //float param = (m_RunningTime + m_TR.m_RollTime) / m_TR.m_MusicTime;
            ////pTile.setProcess(param);

            //Vector3 pos = m_TR.m_track.GetPosition(param, 0);
            //Debug.Log("New Tile Pos:" + pos);
            //pTile.setPosition(m_TR.m_track.GetPosition(param,0));

            //pTile.setRotation(Quaternion.AngleAxis(90, Vector3.right) * m_TR.m_track.GetRotation(param, 0));
           
        }
    }
}
