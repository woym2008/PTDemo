/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/02 11:08:30
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Demo.FrameWork
{
    #region 事件接口
    public delegate void OnEvent(int eventKey, params object[] param);
    #endregion

    public class GameEventSystem :XSingleton<GameEventSystem>
    {

        private readonly Dictionary<int, ListenerWrap> mAllListenerMap = new Dictionary<int, ListenerWrap>();

        public override void Init()
        {
            base.Init();
        }

        #region 功能函数

        // 注册监听事件
        public bool Register<T>(T key, OnEvent func) where T:IConvertible
        {
            var kv = key.ToInt32(null);
            ListenerWrap wrap;

            if(!mAllListenerMap.TryGetValue(kv, out wrap))
            {
                wrap = new ListenerWrap();
                mAllListenerMap.Add(kv, wrap);
            }

            if(wrap.Add(func))
            {
                return true;
            }

            UnityEngine.Debug.LogWarning("Already Register Same Event:" + key);
            return false;
        }

        // 移除监听事件
        public void UnRegister<T>(T key, OnEvent func) where T:IConvertible
        {
            ListenerWrap wrap;
            var kv = key.ToInt32(null);
            if(mAllListenerMap.TryGetValue(kv, out wrap))
            {
                wrap.Remove(func);
            }
        }
        // 移除某一类监听事件
        public void UnRegister<T>(T key) where T:IConvertible
        {
            ListenerWrap wrap;
            var kv = key.ToInt32(null);
            if(mAllListenerMap.TryGetValue(kv, out wrap))
            {
                wrap.RemoveAll();
                wrap = null;
                mAllListenerMap.Remove(kv);
            }
        }

        public bool Send<T> (T key, params object[] param) where T:IConvertible
        {
            int kv = key.ToInt32(null);
            ListenerWrap wrap;
            if(mAllListenerMap.TryGetValue(kv, out wrap))
            {
                return wrap.Send(kv,param);
            }

            return false;
        }
        #endregion

        # region 外部调用API
        
        // 广播事件消息
        public static bool SendEvnet<T>(T key, params object[] param) where T:IConvertible
        {
            return GameEventSystem.GetInstance().Send(key, param);
        }

        // 注册监听事件方法
        public static bool RegisterEvent<T>(T key, OnEvent func) where T:IConvertible
        {
            return GameEventSystem.GetInstance().Register(key, func);
        }

        // 取消监听事件方法
        public static void UnRegisterEvent<T>(T key, OnEvent func) where T:IConvertible
        {
            GameEventSystem.GetInstance().UnRegister(key, func);
        }

        #endregion


        # region 内部监听结构
        private class ListenerWrap
        {
            private LinkedList<OnEvent> mEventList;

            // 出发事件广播
            public bool Send(int key, params object[] param)
            {
                if(mEventList == null)
                {
                    return false;
                }

                var next = mEventList.First;
                OnEvent call = null;
                LinkedListNode<OnEvent> nextCache = null;

                while (next != null)
                {
                    call = next.Value;
                    nextCache = next.Next;

                    call(key, param);
                    next = next.Next ?? nextCache;
                }
                return true;
            }

            // 添加监听事件
            public bool Add(OnEvent listener)
            {
                if(mEventList == null)
                {
                    mEventList = new LinkedList<OnEvent>();
                }
                if(mEventList.Contains(listener))
                {
                    return false;
                }
                mEventList.AddLast(listener);
                return true;
            }

            // 移除监听事件
            public void Remove(OnEvent listener)
            {
                if(mEventList ==null)
                {
                    return;
                }
                mEventList.Remove(listener);
            }

            // 移除所有的监听事件
            public void RemoveAll()
            {
                if(mEventList == null)
                {
                    return;
                }
                mEventList.Clear();
            }
        }
                
        #endregion
    }
}

