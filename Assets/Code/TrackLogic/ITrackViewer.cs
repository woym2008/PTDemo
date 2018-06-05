/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/01 13:10:18
 *	版 本：v 1.0
 *	描 述：轨道接口,负责轨道的具体逻辑
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.GameSystem
{
    public interface ITrackViewer
    {
        /// <summary>
        /// 初始化操作
        /// </summary>
        void Init();
        /// <summary>
        /// 唤醒操作
        /// </summary>
        /// <param name="isStart"></param>
        void OnAwake(bool isStart = false);
        /// <summary>
        /// 挂起操作
        /// </summary>
        void HangUp(); 
        /// <summary>
        /// 更新操作
        /// </summary>
        void OnUpdate();
        /// <summary>
        /// 轨道销销毁操作
        /// </summary>
        void OnDestroy();
        void OnClear();
        /// <summary>
        /// 设置轨道的运行速度
        /// </summary>
        /// <param name="value"></param>
        void SetSpeed(float value);
        float GetSpeed();
        /// <summary>
        /// 获取轨道的运行速度
        /// </summary>
        /// <param name="num"></param>
        void SetTracklineNum(int num);
        /// <summary>
        /// 轨道运行完成
        /// </summary>
        void OnFinished();
        /// <summary>
        /// 获取value对应的进度比率
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        float GetProgressRatio(float value);
        bool CheckPushValue();
        bool PushValue(NodeObject value);
    }


    public class TrackBase : ITrackViewer
    {
        protected trackviewData _data = new trackviewData();

        public virtual void Init()
        { }
        public virtual void OnAwake(bool isStart = false)
        {
            if(isStart)
            {
                _data.runnungTime = 0f;
                _data.isFinished = false;
            }
            _data.isRunning = true;
        }

        public virtual void HangUp()
        {
            _data.isRunning = false;
        }
        public virtual void OnUpdate()
        { }

        public virtual void SetTracklineNum(int num)
        {
            _data.lineNum = num;
        }
        public virtual void OnClear()
        { }
        public virtual void OnDestroy()
        { }
        public virtual void OnFinished()
        {
            this._data.isFinished = true;
        }
        public virtual void SetSpeed(float value)
        {
            _data.speed = value;
        }
        public virtual float GetSpeed()
        {
            return _data.speed;
        }
        public virtual float GetProgressRatio(float value)
        {
            return 0f;
        }
        public virtual bool CheckPushValue()
        { return true; }
        public virtual bool PushValue(NodeObject value)
        { return true; }


        // 轨道数据集合
        public class trackviewData{
            public bool isRunning { get; set; }
            public int lineNum { get; set; }
            public float speed { get; set; }
            public float tracklength { get; set; }      // 轨道的长度
            public float runnungTime { get; set; }
            public bool isFinished { get; set; }
        }
    }
}

