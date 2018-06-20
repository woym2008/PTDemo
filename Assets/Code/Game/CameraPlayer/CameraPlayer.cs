using Demo.UIKeyboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class CameraPlayer
    {
        //------------------------------------------------
        CameraView m_CameraView;

        public KeyboardMark m_ClickMark;
        public GameObject m_PressMark;

        //------------------------------------------------
        //public Vector3 m_CameraOffset;
        //------------------------------------------------
        Vector3 m_position;
        Quaternion m_rotation;

        //------------------------------------------------
        public Transform getPlayerRoot()
        {
            return m_CameraView.transform;
        }

        public Vector3 position
        {
            get
            {
                return m_position;
            }
            set
            {
                m_position = value;
            }
        }

        public Quaternion rotation
        {
            set
            {
                m_rotation = value;
            }
            get
            {
                return m_rotation;
            }
        }
        //------------------------------------------------
        public enum PlayerState
        {
            Stop,
            Move,
            Pause,
        }
        PlayerState m_State;
        //------------------------------------------------
        public CameraPlayer(Transform playerroot = null)
        {
            Debug.Log("CameraPlayer start");

            m_CameraView = playerroot.GetComponent<CameraView>();
            Debug.Log("m_CameraView ok");
            m_CameraView.m_Player = this;

            m_ClickMark = playerroot.GetComponentInChildren<KeyboardMark>();
            for(int i=0;i< playerroot.childCount;++i)
            {
                if(playerroot.GetChild(i).name == "StartPress")
                {
                    m_PressMark = playerroot.GetChild(i).gameObject;
                    break;
                }
            }
        }

        CameraView InstanceCameraPrefab()
        {
            string prefabName = "Player/Player";
            GameObject prefab = Resources.Load<GameObject>(prefabName);

            GameObject go = GameObject.Instantiate(prefab);
            CameraView instance = go.GetComponent<CameraView>();

            if (instance == null)
            {
                Debug.LogError("null player obj" + prefabName);
            }

            return instance;
        }

        public void MoveUpdate(float dt)
        {
            
        }

        public void CorrectPos(float cameradis)
        {
            m_CameraView.UpdatePos();

            m_CameraView.SetCameraPos(cameradis);
        }
        //------------------------------------------------------
        public void startMove()
        {
            m_State = PlayerState.Move;
        }

        public void stopMove()
        {
            m_State = PlayerState.Stop;

        }

        public void pauseMove()
        {
            m_State = PlayerState.Pause;
        }

        public void SetMarkPos(Vector3 point, float delaymarkdis)
        {
            m_ClickMark.transform.position = point;

            m_PressMark.transform.position = point;
            m_PressMark.transform.localPosition = new Vector3(
                m_PressMark.transform.localPosition.x,
                m_PressMark.transform.localPosition.y,
                m_PressMark.transform.localPosition.z + delaymarkdis);
        }

        //public void SetStartPressPos(Vector3 point)
        //{
        //    m_PressMark.transform.position = point;
        //}

        public void SetClickPoint(int index, Vector3 point)
        {
            //test
            //m_CameraView.transform.position = m_position;
            //Vector3 locpos = point - (m_position + m_CameraView.m_CameraOffset);

            Vector3 locpos = point - (m_position + m_CameraView.m_CameraOffset);

            m_ClickMark.AddMarkPoint(index, locpos);
        }

        
    }
}
