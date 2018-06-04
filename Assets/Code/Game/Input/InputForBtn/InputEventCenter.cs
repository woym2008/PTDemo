using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class EventCenter
    {
        static EventCenter m_Instance = null;
        public static EventCenter getInstance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = new EventCenter();
                }

                return m_Instance;
            }
        }
        Dictionary<int, List<BaseSensor>> _eventMap = new Dictionary<int, List<BaseSensor>>();

        //注册 如果已经注册返回FALSE  
        public bool RigisterSensor<T>(Sensor<T> s) where T : InputEvent, new()
        {
            if (s != null)
            {
                if (_eventMap.ContainsKey(s.EventID))
                {
                    List<BaseSensor> bs = _eventMap[s.EventID];
                    if (bs.Contains(s))
                        return false;
                    else
                    {
                        bs.Add(s);
                        bs.Sort();
                        Debug.Log("add list " + bs.Count + "  priority is " + s.Priority);
                        return true;
                    }
                }
                else
                {
                    List<BaseSensor> bs = new List<BaseSensor>();
                    _eventMap.Add(s.EventID, bs);
                    Debug.Log("add map " + s.EventID);
                    bs.Add(s);
                    bs.Sort();
                    Debug.Log("add list " + bs.Count + "  priority is " + s.Priority);
                    return true;
                }
            }
            return false;
        }
        //反注册 如果没有注册返回false  
        public bool UnRigisterSensor<T>(Sensor<T> s) where T : InputEvent, new()
        {
            if (s != null)
            {
                if (_eventMap.ContainsKey(s.EventID))
                {
                    List<BaseSensor> bs = _eventMap[s.EventID];
                    return bs.Remove(s);
                }
            }
            return false;
        }

        public void OnEvent<T>(T t) where T : InputEvent
        {
            Debug.Log("on event  " + t.EventID);
            if (_eventMap.ContainsKey(t.EventID))
            {
                Debug.Log("has eventid");
                List<BaseSensor> bs = _eventMap[t.EventID];
                foreach (var item in bs)
                {
                    //Debug.Log("deal sensor" + item.gb.GetHashCode() + " enablesensor  " + item.EnableSensor);
                    if (t.BroadCarst && item.EnableSensor)
                    {
                        item.OnEvent<T>(t);
                    }
                }
            }
        }

        ~EventCenter()
        {
            _eventMap = null;
        }
    }
}
