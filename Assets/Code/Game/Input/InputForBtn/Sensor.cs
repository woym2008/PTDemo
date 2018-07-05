using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Demo
{
    public abstract class Sensor<T> : BaseInputSensor where T : InputEvent, new()
    {
        virtual public void Init()
        {
            //EventCenter.getInstance.RigisterSensor<T>(this);
        }

        virtual public void Uninit()
        {
            //EventCenter.getInstance.UnRigisterSensor<T>(this);
        }

        /////////////////BaseSensor interface imp  

        //是否激活  
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

        public int CompareTo(BaseInputSensor other) { return other.Priority - priority; }

        /// ///////////////////////////////  

        abstract public void OnEvent<T>(T t) where T : InputEvent;
        abstract public void OnPress<T>(T t) where T : InputEvent;
        abstract public void OnEnd<T>(T t) where T : InputEvent;
    }
}
