using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    [Serializable]
    public class TileRoll
    {
        //public enum RollState
        //{
        //    Stop,
        //    Running,
        //}

        public List<TileTrack> m_Tracks;

        public Queue<TileSpawner> m_CacheSpawner;

        private TileRollFSM m_FSM;

        private TileRollView m_TileRollView;
        public TileRollView GetView
        {
            get
            {
                return m_TileRollView;
            }
        }

        [SerializeField]
        public int m_MaxTile = 10;

        [SerializeField]
        public float m_TileRollLength = 10.0f;

        //[SerializeField]
        public float m_TrackWidth = 5.0f;

        //[SerializeField]
        public float m_TrackThickness = 0.1f;
        
        //[SerializeField]
        public int TrackNum = 4;

        //[SerializeField]
        public Transform m_RollRoot;

        //[SerializeField]
        public Vector3 m_LocalOffset;

        public float m_BPM = 90;
        public float m_BaseBeat = 1.0f;

        //根据bpm算出的结果，一个object从轨道的一端走到另一端的时间
        public float m_RollTime = 0.0f;

        public TileRoll()
        {

        }
        public void Init(MidiTile[] tiles, int bpm, float basebeat, Transform root = null)
        {            
            m_FSM = new TileRollFSM();
            m_FSM.SetState(new TRStopState(this));

            m_Tracks = new List<TileTrack>();

            m_BPM = bpm;
            //basebeat = m_BaseBeat;
            float basetiletime = 60.0f * basebeat / bpm;

            CreateSpawners(tiles, basebeat); 
            if(m_TileRollView == null)
            {
                m_TileRollView = InstanceTillRollView();
            }
            m_TileRollView.SetEntity(this);

            
            m_RollTime = basetiletime * m_MaxTile;

            if(root != null)
            {
                m_RollRoot = null;
            }
            m_TileRollView.transform.parent = m_RollRoot;
            m_TileRollView.transform.localPosition = m_LocalOffset;

            for (int i=0;i< TrackNum; ++i)
            {
                TileTrack track = new TileTrack(i,m_TileRollView.transform);                

                float offsetx = (i+0.5f) * m_TrackWidth - m_TrackWidth * TrackNum * 0.5f;
                float offsety = -m_TileRollLength;

                Vector3 offset = new Vector3(offsetx, offsety, 0);

                track.SetLength(m_TileRollLength);
                track.SetWidth(m_TrackWidth);
                track.SetThickness(m_TrackThickness);

                track.SetOffset(offset);

                track.SetTime(m_RollTime);

                m_Tracks.Add(track);
            }
        }

        public void FrameUpdate(float dt)
        {
            if(m_FSM != null)
            {
                m_FSM.FrameUpdate(dt);
            }
        }

        private TileRollView InstanceTillRollView()
        {
            string prefabName = "Roll/TillRoll";
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            GameObject go = GameObject.Instantiate(prefab);
            TileRollView instance = go.GetComponent<TileRollView>();

            if (instance == null)
            {
                Debug.LogError("null tile obj" + prefabName);
            }

            return instance;
        }

        private void CreateSpawners(MidiTile[] tiles, float onetiletime)
        {
            if(m_CacheSpawner == null)
            {
                m_CacheSpawner = new Queue<TileSpawner>();
            }
            m_CacheSpawner.Clear();

            float basttilelength = m_TileRollLength / m_MaxTile;
            for (int i=0;i< tiles.Length; ++i)
            {
                float scale = (float)(tiles[i].EndTime - tiles[i].StartTime) * basttilelength / onetiletime;
                m_CacheSpawner.Enqueue(new TileSpawner(tiles[i], scale));
            }
        }

        public void EnableGame()
        {
            m_FSM.SetState(new TRRunningState(this));
        }

        public void StopGame()
        {
            m_FSM.SetState(new TRStopState(this));
        }

        public void SetRot(Vector3 localeur)
        {
            if(m_TileRollView != null)
            {
                m_TileRollView.transform.localEulerAngles = localeur;
            }
        }
    }
}
