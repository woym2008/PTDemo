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

        public Vector3 m_CameraOffset = Vector3.zero;

        private void Start()
        {
            
        }

        private void Update()
        {
            if(m_Player == null)
            {
                return;
            }

            Vector3 l = m_Player.rotation * m_CameraOffset;

            this.transform.position = m_Player.position + l;

            this.transform.rotation = m_Player.rotation;
        }
    }
}
