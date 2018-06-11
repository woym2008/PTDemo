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

        public List<TouchTileBase> m_RunningTiles;

        private float m_TileLength = 0.1f;

        private float m_DelayProcess = 0.0f;
        private float m_DelayTime = 0.0f;

        private float m_CurMusicTime = 0.0f;

        private Dictionary<int, float> m_LastTrackProcesses;
        private int lasttrack = -1;

        //private int maxtrack = 1;

        public bool m_bAutoPlay = false;

        public TileManager()
        {
            m_RunningTiles = new List<TouchTileBase>();

            m_LastTrackProcesses = new Dictionary<int, float>();
        }

        public void CreateSpawners(
            MidiTile[] tiles, 
            float onetiletime,
            int numdelaytile,
            float musictime, 
            float baselength)
        {
            m_TileLength = baselength;

            float delaytime = numdelaytile * onetiletime;
            m_DelayTime = delaytime;

            m_CurMusicTime = musictime;
            m_DelayProcess = delaytime / musictime;

            if (m_CacheSpawner == null)
            {
                m_CacheSpawner = new Queue<TileSpawner>();
            }
            m_CacheSpawner.Clear();

            //float basttilelength = m_TileRollLength / m_MaxTile;
            for (int i = 0; i < tiles.Length; ++i)
            {
                float scale = (float)(tiles[i].EndTime - tiles[i].StartTime) / onetiletime;
                if(scale <=1)
                {
                    scale = 1;
                }
                m_CacheSpawner.Enqueue(new TileSpawner(tiles[i], scale * m_TileLength));
            }
        }
        
        public void UpdateSpawner(float passedtime)
        {
            if (m_CacheSpawner.Count > 0)
            {
                TileSpawner pSpawner = m_CacheSpawner.Peek();
                if (pSpawner.getStartTime <= passedtime)
                {
                    float passedprocess = m_DelayProcess + (passedtime) / m_CurMusicTime;
                    AttachBlock(pSpawner, passedprocess);
                    m_CacheSpawner.Dequeue();
                }
            }
        }

        public void Update(float dt)
        {
            for (int i = m_RunningTiles.Count - 1; i >= 0; --i)
            {
                m_RunningTiles[i].FrameUpdate(dt);

                if (m_bAutoPlay)
                {
                    m_RunningTiles[i].AutoUpdate();
                }

                if (m_RunningTiles[i].IsUseless())
                {
                    m_RunningTiles[i].ReleaseSelf();
                    m_RunningTiles.Remove(m_RunningTiles[i]);
                }
            }
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
                        //֤�������������˿��Ѿ�������
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

            //����
            //for(int i=0;i< usefultracknum.Count;++i)
            int usetrack = -1;
            if(usefultracknum.Count > 0)
            {
                int index = UnityEngine.Random.Range(0, usefultracknum.Count);
                usetrack = usefultracknum[index];
            }
            else
            {
                usetrack = UnityEngine.Random.Range(0, TrackManager.instance.trackNum);
            }

            if (bs.getLength > 1)
            {
                int numpoints = 16;
                Vector3[] curvepoints = new Vector3[numpoints];
                float endtime = (float)bs.getEndTime;
                float everytime = (endtime - (float)bs.getStartTime) / numpoints;
                for (int i = 0; i < curvepoints.Length; ++i)
                {
                    float process = m_DelayProcess + ((float)bs.getStartTime + everytime * i) / m_CurMusicTime;
                    curvepoints[i] = TrackManager.instance.GetPosition(process, 0);
                }

                float endprocess = m_DelayProcess + ((float)bs.getEndTime) / m_CurMusicTime;
                pTile = bs.CreateTile(m_DelayProcess, endprocess, m_DelayTime, CurverTouchTile.m_TileName);

                TrackManager.instance.PushValue(pTile, usetrack);

                m_LastTrackProcesses[usetrack] = endprocess;

                bs.CreateTileMesh(pTile, curvepoints);

                lasttrack = usetrack;
            }
            else
            {
                float endprocess = m_DelayProcess + ((float)bs.getEndTime) / m_CurMusicTime;
                pTile = bs.CreateTile(m_DelayProcess, endprocess, m_DelayTime, NormalTouchTile.m_TileName);

                TrackManager.instance.PushValue(pTile, usetrack);

                m_LastTrackProcesses[usetrack] = endprocess;

                bs.CreateTileMesh(pTile);

                lasttrack = usetrack;
            }



            m_RunningTiles.Add(pTile);
            
        }
    }
}