using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Demo
{
    public class InputSystem : MonoBehaviour
    {
        static InputSystem m_Instance = null;
        static public InputSystem getInstance
        {
            get
            {
                return m_Instance;
            }
        }

        public List<InputPanel> m_Panels;

        public void InitInput(int numtrack, float width)
        {

        }

        void Awake()
        {
            m_Instance = this;
        }
        // Use this for initialization
        void Start()
        {

        }
    }
}
