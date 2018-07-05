using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public abstract class InputEvent
    {
        abstract public int EventID
        {
            get;
        }

        GameObject _sender;

        public GameObject Sender
        {
            get { return _sender; }
            set { _sender = value; }
        }

        System.Object[] _param;
        public void SetParam(params System.Object[] prams)
        {
            //get { return _param; }
            //set { _param = value; }
            if (prams != null)
            {
                _param = new System.Object[prams.Length];
                for (int i = 0; i < prams.Length; ++i)
                {
                    _param[i] = prams[i];
                }
            }
        }

        public System.Object[] GetParam()
        {
            return _param;
        }

        GameObject _target;
        public GameObject Target
        {
            get { return _target; }
            set { _target = value; }
        }

        bool _need_broadCast = true;
        public bool BroadCarst
        {
            get { return _need_broadCast; }
            set { _need_broadCast = value; }
        }

        public Vector3 m_TouchPos;
        public GameObject m_TouchObj;

        //重置属性  可覆写该函数以初始化自定义的事件数据  
        virtual public void reset()
        {
            _sender = null;
            _target = null;
            _need_broadCast = true;
            _param = null;
        }
    }
}
