using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public abstract class TouchTile<T> : TouchTileBase
        , BaseInputSensor where T : InputEvent, new()
    {
        //---------------------------------------------------------
        bool _enableSensor = true;
        public bool EnableSensor
        {
            get { return _enableSensor; }
            set { _enableSensor = value; }
        }

        int priority = 0;
        public int Priority
        {
            get { return priority; }
            set { priority = value; }
        }

        int _eventid = 0;
        public int EventID
        {
            get
            {
                if (_eventid == 0)
                {
                    T t = new T();
                    _eventid = t.EventID;
                }
                return _eventid;
            }
        }

        private void OnEnable()
        {
            EventCenter.getInstance.RigisterSensor<T>(this);
        }

        private void OnDisable()
        {
            EventCenter.getInstance.UnRigisterSensor<T>(this);
        }

        public int CompareTo(BaseInputSensor other) { return other.Priority - priority; }

        abstract public void OnPress<T1>(T1 t) where T1 : InputEvent;

        abstract public void OnEnd<T1>(T1 t) where T1 : InputEvent;

        abstract public void OnPressing<T1>(T1 t) where T1 : InputEvent;
    }
}
