using Demo.TileTrack;
using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    //public class LastTrackData
    //{
    //    int tracknum;
    //    float process;
    //}
    public class TileManager
    {
        public Queue<TileSpawner> m_CacheSpawner;

        //public List<TouchTileBase> m_RunningTiles;

        private float m_TileLength = 0.1f;

        private float m_DelayProcess = 0.0f;
        private float m_DelayTime = 0.0f;

        private float m_MusicTime = 0.0f;

        private float m_StartPressTime = 0.0f;

        //private float m_DisappearProcess = 0;
        //private float m_DisappearTime = 2.0f;

        private Dictionary<int, float> m_LastTrackProcesses;
        private int lasttrack = -1;

        //private float m_DelayTimeDisappear = 1.0f;

        //private int maxtrack = 1;

        public static bool s_AutoPlay = false;

        public TileManager()
        {
            //m_RunningTiles = new List<TouchTileBase>();

            m_LastTrackProcesses = new Dictionary<int, float>();
        }

        public void CreateSpawners(
            MidiTile[] tiles, 
            float onetiletime,
            int numdelaytile,
            float musictime, 
            float baselength,
            float startpresstime)
        {
            m_TileLength = baselength;

            float delaytime = numdelaytile * onetiletime;
            m_DelayTime = delaytime;

            m_MusicTime = musictime;
            m_DelayProcess = delaytime / (musictime + delaytime);

            //m_DisappearProcess = m_DisappearTime / (musictime + delaytime);

            if (m_CacheSpawner == null)
            {
                m_CacheSpawner = new Queue<TileSpawner>();
            }
            m_CacheSpawner.Clear();

            //float basttilelength = m_TileRollLength / m_MaxTile;
            for (int i = 0; i < tiles.Length; ++i)
            {
                //把一个块主音和其他音的所有长度加起来的最长长度 建立块
                //float scale = (float)(tiles[i].EndTime - tiles[i].StartTime) / onetiletime;
                //只使用主音轨的长度建立块
                float scale = (float)(tiles[i].EndTime - tiles[i].StartTime) / onetiletime;
                if (scale <=1)
                {
                    scale = 1;
                }
                m_CacheSpawner.Enqueue(new TileSpawner(tiles[i], scale * m_TileLength));
            }

            m_StartPressTime = startpresstime;
        }
        
        public void UpdateSpawner(float passedtime)
        {
            if (m_CacheSpawner.Count > 0)
            {
                TileSpawner pSpawner = m_CacheSpawner.Peek();
                if (pSpawner.getStartTime <= passedtime)
                {
                    float passedprocess = 
                        ((float)pSpawner.getStartTime + m_DelayTime) 
                        / (m_MusicTime + m_DelayTime);
                    AttachBlock(pSpawner, passedprocess);
                    m_CacheSpawner.Dequeue();
                }
            }
        }

        public void Update(float dt)
        {
            //for (int i = m_RunningTiles.Count - 1; i >= 0; --i)
            //{
            //    m_RunningTiles[i].FrameUpdate(dt);

            //    if (m_bAutoPlay)
            //    {
            //        m_RunningTiles[i].AutoUpdate();
            //    }

            //    if (m_RunningTiles[i].IsUseless())
            //    {
            //        m_RunningTiles[i].ReleaseSelf();
            //        m_RunningTiles.Remove(m_RunningTiles[i]);
            //    }
            //}
        }

        void AttachBlock(TileSpawner bs, float passedprocess)
        {
            //test for curve
            TouchTileBase pTile = null;

            List<int> usefultracknum = new List<int>();

            if(m_LastTrackProcesses.Count > 0)
            {
                for(int i=0;i< TrackManager.instance.trackNum; ++i)
                {
                    if(i == lasttrack)
                    {
                        continue;
                    }
                    if(m_LastTrackProcesses.ContainsKey(i))
                    {
                        //证明这个轨道的音此刻已经播完了
                        if (m_LastTrackProcesses[i] <= passedprocess)
                        {
                            usefultracknum.Add(i);
                        }
                    }
                    else
                    {
                        usefultracknum.Add(i);
                    }
                }
            }

            //求轨道
            //for(int i=0;i< usefultracknum.Count;++i)
            int usetrack = -1;
            if(usefultracknum.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, usefultracknum.Count);
                
                usetrack = usefultracknum[index];

                //Debug.Log("lasttrack: " + lasttrack);
                //Debug.Log("usefultracknum: " + usetrack);
            }
            else
            {
                //Debug.LogError("no find usetrack ");
                usetrack = UnityEngine.Random.Range(0, TrackManager.instance.trackNum);
            }

            //Debug.Log("track index: " + usetrack + "lasttrack: " + lasttrack);
            if(usetrack == lasttrack)
            {
                Debug.LogError("error " + usetrack);
            }
            if (bs.getLength > 1)
            //if (true)
            {
                int numpoints = 16;
                Vector3[] curvepoints = new Vector3[numpoints];
                float endtime = (float)bs.getEndTime;
                float everytime = (endtime - (float)bs.getStartTime) / numpoints;
                Vector3[] normals = new Vector3[numpoints];

                for (int i = 0; i < curvepoints.Length; ++i)
                {
                    //选取点计算的问题 每个采样点为延迟进度+花费时间/歌曲时间
                    //float process = m_DelayProcess + 
                    //    ((float)bs.getStartTime + everytime * i) 
                    //    / (m_MusicTime + m_DelayTime);
                    float process = m_DelayProcess +
                        ((float)bs.getStartTime + everytime * i)
                        / (m_MusicTime);
                    curvepoints[i] = TrackManager.instance.GetPosition(process, 0);

                    normals[i] = TrackManager.instance.GetRotation(process, 0) * new Vector3(0,1,0);
                }

                float endprocess = m_DelayProcess + 
                    ((float)bs.getEndTime) / (m_MusicTime + m_DelayTime);
                pTile = bs.CreateTile(m_MusicTime, m_DelayTime, m_StartPressTime
                    , CurverTouchTile.m_TileName);

                TrackManager.instance.PushValue(pTile, usetrack);

                m_LastTrackProcesses[usetrack] = endprocess;

                bs.CreateTileMesh(pTile, curvepoints, normals);

                lasttrack = usetrack;
            }
            else
            {
                float endprocess = m_DelayProcess + ((float)bs.getEndTime) / (m_MusicTime + m_DelayTime);

                pTile = bs.CreateTile(
                    m_MusicTime, m_DelayTime, m_StartPressTime, NormalTouchTile.m_TileName);

                TrackManager.instance.PushValue(pTile, usetrack);

                m_LastTrackProcesses[usetrack] = endprocess;

                bs.CreateTileMesh(pTile);

                //test
                //if(pTile.transform.childCount==0)
                //{
                //    GameObject testobj = new GameObject();
                //    testobj.transform.parent = pTile.transform;
                //    testobj.name = "track:" + usetrack;
                //}
                //else
                //{
                //    pTile.transform.GetChild(0).name = "track:" + usetrack;
                //}
                

                lasttrack = usetrack;
            }



            //m_RunningTiles.Add(pTile);
            
        }
    }
}
