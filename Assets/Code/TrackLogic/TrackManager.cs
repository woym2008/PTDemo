/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/17 10:54:01
 *	版 本：v 1.0
 *	描 述：轨道管理器
* ========================================================*/

#define DEBUGTEST


using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
{
        ///////////////////////////////////
#region Curve Node Data
    public class CurveNodeData
    {
        public Vector3 position{get;set;}
        public Quaternion rotation { get; set; }
    }

#endregion

    public class TrackManager: XSingleton<TrackManager>
    {
        public bool isDebug = false;

        public ITrackViewer trackViewer;
        private int m_TrackLineNum = 2;             // 轨道数量
        //private  float m_speed = 3;               // 轨道运动的速度
        public int maxTrackNum = 4;
        //private float m_fSpace = 2f;              // 音符间距(m)
        private bool m_isMoveable = false;          // 是否可以运动
        private float m_fRunningTime = 0;           // 轨道运行的时间

        /// /////////////////////////////////////////


        public static float changingTime = 3f;        
        
        //private List<TrackLine> _trackLineList;
        
        //private float m_fTimeCircle = 6;        // 循环周期(轨道运行一个循环的时间)，用来表示运动的速度
        //private float m_fTrackLength = 20f;      // 轨道的长度
        
        private float m_fTrackWidth = 5;        // 轨道宽5米
        private float m_minLineSpace = 1.0f;    // 最宽的轨道间距
        public float[] m_lineSpaces = { 0f, 2.0f, 1.2f, 1.0f };
        public float m_adjustTime = 0.9f;
        private int _lastTrackIndex = 0;        // 最近分配的轨道编号


        //private List<NodeObject> _prepareList;   // 预备队列，等待加入轨道的节点列表
        //private int m_maxCacheNum = 25;        // 最大的缓存数量，当超过这个数值后将拒绝接受数据


        /////////////// 临时数据，以后再详细设计整理
        protected Transform m_trackParent;        // 父节点，其坐标表示轨道终点
        //protected GameObject m_trackPanel;        // 轨道面板
        //protected Transform m_trackTrans;
        protected Vector3 m_trackPosition;        // 轨道的坐标位置，计算终点坐标
        protected Vector3 m_trackEndPos;

        //protected MouseTouchArea m_touchArea;
        protected NodeObject m_lastNote;
        //protected TrackPanel trackPanel;
        float timeSpace = 0;



        public bool Init(TrackNumDef.enTrackType trackType = TrackNumDef.enTrackType.Curve)
        {
            if (trackViewer != null)
            {
                trackViewer.OnDestroy();
                trackViewer = null;
            }

            switch (trackType)
            {
                case TrackNumDef.enTrackType.Linear:
                    trackViewer = new LinearTrackViewer();
                    break;
                case TrackNumDef.enTrackType.Curve:
                    trackViewer = new CurveTrackRollViewer();
                    break;
            }

            trackViewer.Init();

            trackViewer.SetTracklineNum(m_TrackLineNum);

            //trackViewer.SetSpeed(Game.instance.m_CameraSpeed);

            return true;
        }

        public void GenerateTrack(GameObject obj, CurveNodeData[] pathDataArray)
        {
            trackViewer.GenerateTrack(obj, pathDataArray);
        }

        /// <summary>
        /// 获取轨道上某点的坐标位置
        /// </summary>
        /// <param name="paramater"> 0-1 索取的点在轨道中的百分比率</param>
        /// <returns></returns>
        public Vector3 GetPosition(float paramater, int lineIndex)
        {
            return trackViewer.GetPosition(paramater, lineIndex);
        }
        public Quaternion GetRotation(float paramater, int lineIndex)
        {
            return trackViewer.GetRotation(paramater, lineIndex);
        }

        public float Speed
        {
            get { return 0; }
            set
            {
                Debug.LogWarning("@@@@@@@@@@ 未实现 Speed");
            }
        }

        public float runningTime
        {
            get { return m_fRunningTime; }
        }
        
        public int trackNum
        {
            get { return m_TrackLineNum; }
        }
        public float GetProgressRatio(float value)
        {
            return this.trackViewer.GetProgressRatio(value);
        }

        public bool isRunning
        {
            get { return m_isMoveable; }
        }

        public void ResetTracklineNum(int num, bool displayUI)
        {
            Debug.LogWarning("没有实现变轨操作");
            //Game.instance.StartCoroutine(SetTracklineNum(num, 0f, displayUI));
        }

       
        public bool CheckPushValue()
        {
            return this.trackViewer.CheckPushValue();
        }



        // 压入节点数据
        //public bool PushValue(NodeObject node)
        public bool PushValue(IPTTile node)
        {
            return this.trackViewer.PushValue(node);
        }



        public void Start()
        {
            m_isMoveable = true;
            m_fRunningTime = 0;
            timeSpace = 0;

            trackViewer.OnAwake(true);
        }
        public void Stop()
        {
            m_isMoveable = false;

            trackViewer.HangUp();
        }
        public void Continue()
        {
            m_isMoveable = true;
            trackViewer.OnAwake(false);
        }

        public NodeObject lastNote
        {
            get { return m_lastNote; }
        }

        public void Clear()
        {
            this.trackViewer.OnClear();
        }


        // 由Game驱动
        public void Update()
        {
            if (!m_isMoveable)
                return;
            trackViewer.OnUpdate();
        }        
    }
}

