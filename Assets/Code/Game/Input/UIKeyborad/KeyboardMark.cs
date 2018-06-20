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

        public GameObject UIKey;

        private void Start()
        {
            m_Mark = new Dictionary<int, KeyboradPressPoint>();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Press(0);
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Press(1);
            }
            else if (Input.GetKeyDown(KeyCode.L))
            {
                Press(2);
            }
        }

        public void AddMarkPoint(int mark, Vector3 pos)
        {
            GameObject pobj = new GameObject();
            KeyboradPressPoint markpoint = pobj.AddComponent<KeyboradPressPoint>();
            //markpoint.Pos = pos - this.transform.position;
            
            markpoint.transform.parent = this.transform.parent;
            markpoint.transform.localPosition = pos;
            m_Mark.Add(mark, markpoint);
        }

        public void Press(int index)
        {
            if(m_Mark.ContainsKey(index))
            {
                m_Mark[index].Press(index);
            }
        }
    }
}
