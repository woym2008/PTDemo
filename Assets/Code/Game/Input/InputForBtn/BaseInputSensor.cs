using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public interface BaseInputSensor : IComparable<BaseInputSensor>
    {
        //虚基础属性接口  
        int Priority { get; set; }
        int EventID { get; }
        //GameObject gb { get; }
        bool EnableSensor { get; set; }

        //消息处理接口  
        //void OnEvent<T>(T t) where T : InputEvent;
        void OnPress<T>(T t) where T : InputEvent;
        void OnEnd<T>(T t) where T : InputEvent;
        void OnPressing<T>(T t) where T : InputEvent;
    }
}
