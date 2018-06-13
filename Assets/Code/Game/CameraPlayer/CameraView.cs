using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class CameraView : MonoBehaviour
    {
        public CameraPlayer m_Player;
        public Camera m_Camera;

        public Vector3 m_CameraOffset = Vector3.zero;

        private void Start()
        {
            m_CameraOffset = m_Camera.transform.localPosition;
            m_Camera.transform.localPosition = new Vector3(0, 0, 0);
        }

        private void Update()
        {
            if(m_Player == null)
            {
                return;
            }

            Vector3 l = m_Player.rotation * m_CameraOffset;

            //this.transform.position = m_Player.position - m_Camera.transform.localPosition;
            //this.transform.position = m_Player.position + l;
            this.transform.position = m_Player.position + l;

            this.transform.rotation = m_Player.rotation;
        }
    }
}
