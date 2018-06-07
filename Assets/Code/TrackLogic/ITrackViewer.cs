/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/01 13:10:18
 *	版 本：v 1.0
 *	描 述：轨道接口,负责轨道的具体逻辑
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
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
        /// 获取轨道长度
        /// </summary>
        float GetTrackLength();
        /// <summary>
        /// 获取轨道的运行速度
        /// </summary>
        /// <param name="num"></param>
        void SetTracklineNum(int num, float lineSpace);
        int GetTracklineNum();
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
        bool PushValue(IPTTile value, int lineIndex);

        void GenerateTrack(GameObject obj, CurveNodeData[] pathDataArray);
        /// <summary>
        /// 获取轨道上的某点的位置
        /// </summary>
        /// <param name="paramater">表示所取点在整个轨道中的比率 0-1</param>
        /// <param name="lineIndex">表示的轨道线索引 左边第一条索引为0 </param>
        /// <returns></returns>
        Vector3 GetPosition(float paramater, int lineIndex);

        Quaternion GetRotation(float paramater, int lineIndex);

        void SetTrackWidth(float width);
        void SetTrackHeight(float height);
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

        public virtual void SetTracklineNum(int num, float lineSpace)
        {
            _data.lineNum = num;
            _data.lineSpace = lineSpace;
        }
        public virtual int GetTracklineNum()
        {
            return (_data.lineNum > 0) ?_data.lineNum : 1;
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
        public virtual bool PushValue(IPTTile value,int lineIndex)
        { return true; }
        public virtual void GenerateTrack(GameObject obj, CurveNodeData[] pathDataArray)
        { }
        public virtual Vector3 GetPosition(float parameter, int lineIndex) { return Vector3.zero; }

        public virtual Quaternion GetRotation(float paramater, int lineIndex) { return Quaternion.identity; }

        public float GetTrackLength()
        {
            return _data.tracklength;
        }
        public virtual void SetTrackWidth(float width)
        {

        }
        public virtual void SetTrackHeight(float height)
        { }
        public virtual float GetSpacingProgress()
        { return _data.spacingProgress; }
        // 轨道数据集合
        public class trackviewData{
            public bool isRunning { get; set; }
            public int lineNum { get; set; }
            public float speed { get; set; }
            public float tracklength { get; set; }      // 轨道的长度
            public float runnungTime { get; set; }
            public bool isFinished { get; set; }
            public float lineSpace { get; set; }
            public float width { get; set; }
            public float height { get; set; }

            public float spacingProgress = 0f;
            public float spacingTime = 0;
        }
    }
}

