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

        private void Start()
        {
            
        }

        private void Update()
        {
            this.transform.position = m_Player.getPosition;
        }
    }
}
