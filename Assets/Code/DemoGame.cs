//using Demo.TileTrack;
using PTAudio.Frame;
using PTAudio.Midi.Builder;
using PTAudio.Midi.Data.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class DemoGame : MonoBehaviour
    {
        AudioSystem m_system;

        //[SerializeField] StreamingAssetResouce midiSource;

        public string path = "";

        //public string maintrack = "Piano";

        public int maintrackIndex = 0;

        public TileRoll m_Roll;

        public Transform m_RotObj;

        //public CameraPlayer m_Player;

        public Transform m_StartTrans;
        public Transform m_EndTrans;

        public Transform m_PlayerObj;

        //------------------------------------
        public Vector3 m_TileOffset = Vector3.zero;
        //------------------------------------
        public Transform m_RollRoot;
        public Vector3 m_LocalOffset;
        public int m_MaxTile = 20;
        public int TrackNum = 4;
        public float m_TrackThickness = 0.1f;
        public float m_TrackWidth = 5.0f;
        public float m_TileRollLength = 10.0f;

        public float m_CameraSpeed = 1.0f;
        //------------------------------------
        public AudioClip m_Accompaniment = null;
        public float m_AccompVolume = 0.5f;
        //------------------------------------
        bool m_bEnableCamera = false;

        //------------------------------------
        public GameObject m_trackPrefab;
        //------------------------------------
        //public float AutoPlay = 1.0f;
        //------------------------------------
        private void Start()
        {
            m_system = new AudioSystem();
            m_system.init();

#if UNITY_ANDROID && !UNITY_EDITOR
            Debug.Log("Plat is Android");
            m_system.SetPlatform(SystemInPlatform.Android);            
#else
            Debug.Log("Plat is PC");
            m_system.SetPlatform(SystemInPlatform.PC);
#endif
            m_system.load(path, maintrackIndex);

            //m_Player = new CameraPlayer(m_PlayerObj);

            //--------------------------------------
            m_Roll = new TileRoll();
            //m_Roll.m_RollRoot = m_RollRoot;
            //m_Roll.m_LocalOffset = m_LocalOffset;
            m_Roll.m_MaxTile = m_MaxTile;
            m_Roll.TrackNum = TrackNum;
            m_Roll.m_TileOffset = m_TileOffset;
            //m_Roll.m_TrackThickness = m_TrackThickness;
            //m_Roll.m_TrackWidth = m_TrackWidth;
            //m_Roll.m_TileRollLength = m_TileRollLength;
            m_Roll.m_trackPrefab = m_trackPrefab;            
            //--------------------------------------
            MidiTile[] m_Tiles;
            if(m_system.getTileDatas(out m_Tiles))
            {
                int bpm = m_system.getBPM();
                float basebeat = m_system.getBaseBeat();
                m_Roll.Init(m_Tiles, bpm, basebeat, (float)m_Tiles[m_Tiles.Length-1].EndTime, m_PlayerObj);

                //if(m_RotObj != null)
                //{
                //    m_Roll.SetRot(m_RotObj.localEulerAngles);
                //}
            }
            //--------------------------------------
            //accomp
            m_system.SetAccompaniment(m_Accompaniment.name, m_Accompaniment);
            m_system.SetAccompanimentVolume(m_Accompaniment.name, m_AccompVolume);


            //--------------------------------------

            TouchTileFactory.Init();
        }

        private void Update()
        {
            //Debug.LogWarning("m_system.update");
            m_system.update(Time.deltaTime);

            //Debug.LogWarning("FrameUpdate");
            m_Roll.FrameUpdate(Time.deltaTime);

            //Debug.LogWarning("MoveUpdate");
            //if(m_Player == null)
            //{
            //    Debug.LogWarning("m_Player is null");
            //}

            //m_Player.MoveUpdate(Time.deltaTime);

            //Debug.LogWarning("MoveUpdate ok");
            //test
            //if (Input.GetKeyDown(KeyCode.T))
            //{
            //    m_Roll.EnableGame(m_Player);
            //    m_Player.startMove();
            //}
            //if (Input.GetKeyDown(KeyCode.S))
            //{
            //    m_Roll.StopGame();
            //    m_Player.stopMove();
            //}
            //end
        }

        public void StartDemo()
        {
            m_Roll.EnableGame(m_system);
            //m_Player.startMove();
            //m_Player.SetSpeed(5);
            m_system.PlayAccompaniment(m_Roll.m_RollTime);
        }

        public void AddCamSpeed()
        {
            //m_CameraSpeed += 0.5f;
            //m_Player.SetSpeed(m_CameraSpeed);

            //m_Roll.m_track.Speed = m_CameraSpeed;

            m_Roll.AddSpeed();
        }

        public void ReduceCamSpeed()
        {
            //m_CameraSpeed -= 0.5f;
            //m_Player.SetSpeed(m_CameraSpeed);

            //m_Roll.m_track.Speed = m_CameraSpeed;
            m_Roll.ReduceSpeed();
        }

        public void AutoPlay()
        {
            m_Roll.m_bAutoPlay = !m_Roll.m_bAutoPlay;
        }
    }
}
