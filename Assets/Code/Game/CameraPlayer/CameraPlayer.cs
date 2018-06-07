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
    }
}
