using PTAudio.Frame;
using PTAudio.Midi.Data.Util;
using PTAudio.Midi.Builder;
using PTAudio.MidiPlayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class Entry : MonoBehaviour
    {
        AudioSystem m_system;

        //[SerializeField] StreamingAssetResouce midiSource;

        public string path = "";

        //public string maintrack = "Piano";

        public int maintrackIndex = 0;

        public MidiPlayer m_Player;

        private void Start()
        {
            m_system = new AudioSystem();
            m_system.init();

#if UNITY_ANDROID && !UNITY_EDITOR
            //Debug.Log("Plat is Android");
            m_system.SetPlatform(SystemInPlatform.Android);            
#else
            Debug.Log("Plat is PC");
            m_system.SetPlatform(SystemInPlatform.PC);
#endif
            m_system.load(path, maintrackIndex);

            m_Player = new MidiPlayer();
            LoadPlayer();
        }

        private void OnGUI()
        {
            GUIStyle style = new GUIStyle();
            style.fontSize = 64;
            style.fontStyle = FontStyle.Bold;
            style.alignment = TextAnchor.MiddleLeft;

            //Debug.Log("NumCurAudioSourceCtrl" + m_system.NumCurAudioSourceCtrl());
            GUI.TextArea(new Rect(400, 200, 200, 100), "" + m_system.getNumCurAudioSourceCtrl(), style);
        }

        private void Update()
        {
            m_system.update(Time.deltaTime);

            m_Player.Update(Time.deltaTime);

            if (Input.GetKeyDown(KeyCode.W))
            {
                m_Player.Play();
            }
            if (Input.GetKeyDown(KeyCode.P))
            {
                m_Player.Pause();
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                m_Player.Stop();
            }
        }

        public void CreateDatas()
        {
            //m_system.GetDatas(out );
            //m_Roll;
        }

        public void LoadPlayer()
        {
            //MidiTile[] datas = null;
            //if (m_system.GetTileDatas(out datas))
            //{
            //    m_Player.LoadTile(datas);
            //}
            m_Player.LoadTile(m_system);
        }
    }
}
