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
        Vector3 m_position;        

        float m_speed = 1;
        float m_speedparam = 1.0f;

        Vector3 m_direct = new Vector3(0,0,1);
        //------------------------------------------------
        Vector3 m_start;
        Vector3 m_end;

        float m_AllTime;
        float m_PassedTime;
        //------------------------------------------------
        public Transform getPlayerRoot()
        {
            return m_CameraView.transform;
        }

        public Vector3 getPosition
        {
            get
            {
                return m_position;
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
        public CameraPlayer(float time, Vector3 start, Vector3 end, Transform playerroot = null)
        {
            m_start = start;
            m_end = end;
            m_AllTime = time;
            m_PassedTime = 0.0f;

            m_direct = (m_end - m_start).normalized;
            m_State = PlayerState.Stop;

            m_speed = (m_end - m_start).magnitude / m_AllTime;

            Debug.Log("CameraPlayer start");
            //m_CameraView = InstanceCameraPrefab();
            m_CameraView = playerroot.GetComponent<CameraView>();
            Debug.Log("m_CameraView ok");
            m_CameraView.m_Player = this;
            //m_CameraView.transform.position = m_start;
            //m_CameraView.transform.LookAt(m_end);

            m_position = m_start;
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
            switch(m_State)
            {
                case PlayerState.Move:

                    m_position = m_position + m_direct * dt * m_speed * m_speedparam;

                    break;
                case PlayerState.Stop:
                    m_position = m_start;
                    break;
            }
            
        }

        //public void setSpeed(float sp)
        //{
        //    m_speed = sp;
        //}

        //public void setDirect(Vector3 dir)
        //{
        //    m_direct = dir;
        //}
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

        public void SetSpeed(float sp)
        {
            m_speedparam = sp;
        }
    }
}
