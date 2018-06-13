using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo.UIKeyboard
{
    public class KeyboardMark : MonoBehaviour
    {
        public Dictionary<int, KeyboradPressPoint> m_Mark;

        private void Start()
        {
            m_Mark = new Dictionary<int, KeyboradPressPoint>();
        }

        private void Update()
        {
            
        }

        public void AddMarkPoint(int mark, Vector3 pos)
        {
            GameObject pobj = new GameObject();
            KeyboradPressPoint markpoint = pobj.AddComponent<KeyboradPressPoint>();
            //markpoint.Pos = pos - this.transform.position;
            markpoint.transform.position = pos;
            markpoint.transform.parent = this.transform;
            m_Mark.Add(mark, markpoint);
        }

        public void Press(int index)
        {
            if(m_Mark.ContainsKey(index))
            {
                m_Mark[index].Press();
            }
        }
    }
}
