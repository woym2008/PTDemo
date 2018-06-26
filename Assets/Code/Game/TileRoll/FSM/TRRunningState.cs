using Demo.TileTrack;
using PTAudio.Frame;
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

        float testparam = 0.0f;
        //CameraPlayer m_Player;


        //-----------------------------------------------
        //临时的 活动块的管理
        public List<TouchTileBase> m_RunningTiles;
        //-----------------------------------------------
        
        public TRRunningState(TileRoll tr)
        {
            m_TR = tr;

            //m_Player = player;

            //----------
            
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
        }

        public void Execute(float dt)
        {
            m_StartTime += dt;
            m_RunningTime += dt;

            m_TR.getTileManager.UpdateSpawner(m_RunningTime);

            m_TR.m_track.Update();


            float param = m_RunningTime / (m_TR.m_MusicTime + m_TR.m_RollTime);
            //float param = Mathf.Clamp((m_RunningTime),0,float.MaxValue) 
            //    / (m_TR.m_MusicTime + m_TR.m_RollTime);

            int playertracknum = m_TR.m_track.trackNum / 2;

            m_TR.m_Player.position = m_TR.m_track.GetPosition(param, playertracknum,true);
            
            m_TR.m_Player.rotation = m_TR.m_track.GetRotation(param, playertracknum);
            //-----------------------------
            m_TR.getTileManager.Update(dt);
            //for (int i= m_RunningTiles.Count-1;i>=0;--i)
            //{
            //    m_RunningTiles[i].FrameUpdate(dt);
            //    if(m_RunningTiles[i].m_MoveTime > m_TR.m_RollTime - 2.0f)
            //    {
            //        m_RunningTiles[i].EnableTouch();

            //        if (m_TR.m_bAutoPlay)
            //        {                        
            //            m_RunningTiles[i].OnTouchBeat(Vector3.zero);
            //        }
            //    }

            //    if (m_RunningTiles[i].m_MoveTime > m_TR.m_RollTime + m_TR.m_RollTime)
            //    {
            //        m_RunningTiles[i].ReleaseSelf();
            //        m_RunningTiles.Remove(m_RunningTiles[i]);
            //    }
            //}
        }

        public void Exit()
        {
            //foreach (var t in m_TR.m_Tracks)
            //{
            //    t.m_bEnableTrack = false;
            //}
            m_TR.m_track.Stop();
        }
    }
}
