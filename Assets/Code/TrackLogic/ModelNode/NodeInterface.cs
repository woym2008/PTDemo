/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/15 17:20:57
 *	版 本：v 1.0
 *	描 述：钢琴键节点接口类，提供节点所需的对外接口
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
{
    public class NodeObject : MonoBehaviour, IPTTile
    {
        protected GameObject _gameObject;
        protected Transform _trans;
        protected NodeManager.NodeData _data = new NodeManager.NodeData();
             
      
        protected virtual void Awake()
        {
            _gameObject = gameObject;
            _trans = transform;

            _data.ori_scale = _trans.localScale;
            _data.ori_rotation = _trans.localRotation;
        }

        public virtual void Init()
        {
            // 数据初始化操作
        }
       

        public NodeManager.eNodeType eType
        {
            get { return _data.eType; }
            set { _data.eType = value; }
        }
        public float startTime
        {
           get { return _data.startTime; }
        }
        public float startProgress
        {
            get { return _data.startProgress; }
        }
        public float positionProgress{
            get { return _data.posProgrss; }
        }
        public Vector3 position
        {
            get { return _trans.localPosition; }
            set { _trans.localPosition = value; }
        }
        public float progress
        {
            get { return _data.curProgress; }
            set { _data.curProgress = value; }
        }
        public Quaternion rotation
        {
            get { return _trans.localRotation; }
            set { _trans.localRotation = value; }
        }
        public Vector3 scale
        {
            get { return _trans.localScale; }
            set { _trans.localScale = value; }
        }

        public bool voiceable
        {
            get { return _data.voiceable ; }
            set { _data.voiceable = value; }
        }
        public bool isSubNote
        {// 是否是子节点
            get { return _data.subIndex > 0; }
        }

        public virtual void SetActive(bool value)
        {
            if (_gameObject != null)
            {
                _gameObject.SetActive(value);
            }            
        }

        public virtual void SetParent(Transform parent)
        {
            if(_gameObject != null)
            {
                _gameObject.transform.parent = parent;
            }
        }

        public virtual void ResetDataInfo(NodeManager.NodeData dataInfo)
        {
            _data.startTime = dataInfo.startTime;
            _data.startProgress = dataInfo.startProgress;
            _data.length = (float)dataInfo.length;
            _data.index = dataInfo.index;
            _data.subIndex = dataInfo.subIndex;
            _data.triggleChanging = dataInfo.triggleChanging;
            _data.posProgrss = dataInfo.posProgrss;
        }
        public virtual bool IsAlive(float runningTime,float lifeTime)
        {
            if (!_data.avaliable)
                return false;

            // 运行时间超过周期
            //if (_data.startTime + _data.length + lifeTime < runningTime)
            if (_data.startTime + lifeTime < runningTime)
            {
                return false;
            }
            return true;
        }
        // 出现，复活
        public virtual void Appear(int trackIndex)
        {
            _data.avaliable = true;
            _data.isPressed = false;
            SetActive(true);
        }
        public virtual void Disappear()
        {
            _data.avaliable = false;

            _data.Reset();

            SetActive(false);
        }

        // 消失，死亡
        public bool IsPressed
        {
            get
            {
                return _data.isPressed;
            }
            set
            {
                _data.isPressed = value;
            }
        }

        public void DebugTrggerEnter()
        {
            if (!TrackManager.instance.isDebug)
                return;
            OnTriggerEnter(null);

            if (!TrackManager.instance.isRunning)
            {
                return;
            }
            _data.isPressed = true;

            PlayNote();
            AddScore();
            DisplayClickEffect();
            Disappear();

        }
        protected virtual void OnTriggerEnter(Collider collider)
        {
            if (TrackManager.instance.isDebug)
                return;

            if (!TrackManager.instance.isRunning)
            {
                return;
            }
            _data.isPressed = true;

            PlayNote();
            AddScore();
            DisplayClickEffect();
            Disappear();
        }
        //protected virtual void OnTriggerExit(Collider collider)
        //{ }

        // 播放音符
        protected virtual void PlayNote()
        {
            if (voiceable)
            {
                // 播放音符
                //Game.instance.m_center.Play();
            }
        }
        // 分数的控制
        protected virtual void AddScore()
        { }
        // 音符被点击后的效果播放
        protected virtual void DisplayClickEffect()
        {

        }
        // 死亡特效
        public virtual void DisappearEffect()
        { }
        //----------------------------------------------------------------
        public float getStartTime()
        {
            return startTime;
        }

        public float getProcess()
        {
            return progress;
        }

        public float getStartProcess()
        {
            return startProgress;
        }

        public float getEndProcess()
        {
            return startProgress;
        }

        public void setPosition(Vector3 pos)
        {
            this.transform.position = pos;
        }

        public void setRotation(Quaternion rot)
        {
            this.transform.rotation = rot;
        }

        public void setScale(Vector3 scale)
        {
            this.transform.localScale = scale;
        }

        public void setProcess(float pro)
        {
            progress = pro;
        }

        public float getPositionProgress()
        {
            return positionProgress;
        }

        public void onUpdate()
        {

        }

        //void Appear()
        //{

        //}

        //void Recover();
    }
}
