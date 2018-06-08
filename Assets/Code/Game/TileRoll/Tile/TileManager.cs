using Demo.TileTrack;
using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public class TileManager
    {
        public Queue<TileSpawner> m_CacheSpawner;

        public List<TouchTileBase> m_RunningTiles;

        private float m_TileLength = 0.1f;

        private float m_DelayProcess = 0.0f;

        public TileManager()
        {
            m_RunningTiles = new List<TouchTileBase>();
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
                    AttachBlock(pSpawner);
                    m_CacheSpawner.Dequeue();
                }
            }
        }

        public void Update(float dt)
        {
            for (int i = m_RunningTiles.Count - 1; i >= 0; --i)
            {
                m_RunningTiles[i].FrameUpdate(dt);

                if(m_RunningTiles[i].IsUseless())
                {
                    m_RunningTiles[i].ReleaseSelf();
                    m_RunningTiles.Remove(m_RunningTiles[i]);
                }
            }
        }

        void AttachBlock(TileSpawner bs)
        {
            //test for curve
            TouchTileBase pTile = null;

            //if (bs.getScale > 1)
            //{
            //    int numpoints = 16;
            //    Vector3[] curvepoints = new Vector3[numpoints];
            //    float endtime = (float)bs.getEndTime;
            //    float everytime = (endtime - (float)bs.getStartTime) / numpoints;
            //    for (int i = 0; i < curvepoints.Length; ++i)
            //    {
            //        float process = startprocess + ((float)bs.getStartTime + everytime * i) / m_TR.m_MusicTime;
            //        curvepoints[i] = m_TR.m_track.GetPosition(process, 0);
            //    }
            //    pTile = bs.CreateTile(startprocess, CurverTouchTile.m_TileName, curvepoints);
            //}
            //else
            {
                pTile = bs.CreateTile(m_DelayProcess, NormalTouchTile.m_TileName);
            }

            TrackManager.instance.PushValue(pTile);

            m_RunningTiles.Add(pTile);
            
        }
    }
}
