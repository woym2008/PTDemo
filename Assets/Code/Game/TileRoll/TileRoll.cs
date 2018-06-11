using Demo.TileTrack;
using PTAudio.Frame;
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

        //public List<TileTrack> m_Tracks;

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

        float m_curBPM;
        float m_speedratio = 1.0f;

        //根据bpm算出的结果，一个object从轨道的一端走到另一端的时间
        //也即是生成tile后 轨道延迟时间
        public float m_RollTime = 0.0f;

        public float m_MusicTime = 0.0f;

        public float m_IntoTouchProportion = 0.1f;
        public float m_IntoTouchAreaTime = 0;

        //---------------------------------------------------
        public Vector3 m_TileOffset = Vector3.zero;
        //---------------------------------------------------
        public CameraPlayer m_Player;
        //---------------------------------------------------
        //track
        public TrackManager m_track;
        //------------------------------------
        public GameObject m_trackPrefab;
        //---------------------------------------------------
        //public bool m_bAutoPlay = false;
        //---------------------------------------------------
        public float m_TileLenght = 0.4f;
        TileManager m_TileMgr;
        public TileManager getTileManager
        {
            get
            {
                return m_TileMgr;
            }
        }
        //---------------------------------------------------
        //cache Audio System
        AudioSystem m_system;
        //---------------------------------------------------
        public TileRoll()
        {

        }
        public void Init(MidiTile[] tiles, int bpm, float basebeat,
            float musictime,
            Transform root = null
            )
        {
            m_Player = new CameraPlayer(root);

            m_TileMgr = new TileManager();

            //fsm
            m_FSM = new TileRollFSM();
            m_FSM.SetState(new TRStopState(this));

            m_MusicTime = musictime;

            //m_Tracks = new List<TileTrack>();

            //init tile spawner
            m_BPM = bpm;
            m_curBPM = m_BPM;
            //basebeat = m_BaseBeat;
            float basetiletime = 60.0f * basebeat / bpm;
            m_RollTime = basetiletime * m_MaxTile;
            //m_RollTime = 10.0f;

            m_track = TrackManager.instance;

            //float waitprocess = m_RollTime / musictime;
            //CreateSpawners(tiles, basebeat);
            //tilelenght = 
            m_TileMgr.CreateSpawners(tiles, basetiletime,m_MaxTile,
                musictime, m_TileLenght);
            //-----------------------------------------------------------
            //track
            TrackManager.instance.Init(TrackNumDef.enTrackType.Curve);
            SetupPath(TrackManager.instance);

            float length = TrackManager.instance.trackViewer.GetTrackLength();

            float speed = length / musictime;
            TrackManager.instance.Speed = speed;
            TrackManager.instance.trackViewer.SetTrackHeight(0.001f);
            //-----------------------------------------------------------
            m_Player.position = TrackManager.instance.GetPosition(0, 0);
        }

        public void FrameUpdate(float dt)
        {
            if(m_FSM != null)
            {
                m_FSM.FrameUpdate(dt * m_speedratio);
            }

            // Test code
            //TrackManager.GetInstance().Update();
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

        public void EnableGame(AudioSystem system)
        {
            m_system = system;

            m_FSM.SetState(new TRRunningState(this));
        }

        public void StopGame()
        {
            m_FSM.SetState(new TRStopState(this));
        }

        public void SetRot(Vector3 localeur)
        {
            //if(m_TileRollView != null)
            //{
            //    m_TileRollView.transform.localEulerAngles = localeur;
            //}
        }

        void SetupPath(TrackManager track)
        {
            GameObject pathRoot = GameObject.Find("RootPath");
            List<CurveNodeData> pathDataList = new List<CurveNodeData>();

            Transform pathTrans = pathRoot.transform;
            for (int i = 0; i < pathTrans.childCount; ++i)
            {
                Transform child = pathTrans.GetChild(i);
                CurveNodeData data = new CurveNodeData();
                data.position = child.position;
                data.rotation = child.rotation;
                pathDataList.Add(data);
            }

            track.GenerateTrack(m_trackPrefab, pathDataList.ToArray(),400);
        }

        public void AddSpeed()
        {
            m_curBPM += 10;
            m_speedratio = m_curBPM/(float)m_BPM;

            float length = m_track.trackViewer.GetTrackLength();
            float speed = length / m_MusicTime;
            m_track.Speed = speed * m_speedratio;

            m_system.setMusicSpeed(m_speedratio);
        }

        public void ReduceSpeed()
        {
            m_curBPM -= 10;
            m_speedratio = m_curBPM / (float)m_BPM;

            float length = m_track.trackViewer.GetTrackLength();
            float speed = length / m_MusicTime;
            m_track.Speed = speed * m_speedratio;

            m_system.setMusicSpeed(m_speedratio);
        }

        public void SetAutoPlay()
        {
            m_TileMgr.m_bAutoPlay = !m_TileMgr.m_bAutoPlay;
        }
    }
}
