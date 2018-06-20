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
        private int m_TrackLineNum = 3;             // 轨道数量
        //private  float m_speed = 3;               // 轨道运动的速度
        public int maxTrackNum = 4;
        //private float m_fSpace = 2f;              // 音符间距(m)
        private bool m_isMoveable = false;          // 是否可以运动
        private float m_runningTime = 0;           // 轨道运行的时间

        /// /////////////////////////////////////////
        
        public static float changingTime = 3f;        
        
        //private List<TrackLine> _trackLineList;
        
        //private float m_fTimeCircle = 6;        // 循环周期(轨道运行一个循环的时间)，用来表示运动的速度
        //private float m_fTrackLength = 20f;      // 轨道的长度
        
        private float m_fTrackWidth = 5;        // 轨道宽5米
        private float m_minLineSpace = 1.0f;    // 最宽的轨道间距
        public float[] m_lineSpaces = { 0f, 2.0f, 1.2f, 1.0f };
        public float m_adjustTime = 0.9f;
        private int m_lastTrackIndex = 0;        // 最近分配的轨道编号


        //private List<NodeObject> _prepareList;   // 预备队列，等待加入轨道的节点列表
        //private int m_maxCacheNum = 25;        // 最大的缓存数量，当超过这个数值后将拒绝接受数据


        /////////////// 临时数据，以后再详细设计整理
        //protected Transform m_trackParent;        // 父节点，其坐标表示轨道终点
        //protected GameObject m_trackPanel;      // 轨道面板
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

            this.ResetTracklineNum(TrackNumDef.defaultLineNum, false);

            //trackViewer.SetSpeed(Game.instance.m_CameraSpeed);

            return true;
        }

        /// <summary>
        /// 生成轨道面
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="pathDataArray"></param>
        /// <param name="meshSegment">mesh分段数目，数目越少性能越高</param>
        public void GenerateTrack(GameObject obj, CurveNodeData[] pathDataArray,int meshSegment = -1)
        {
            if(meshSegment <= 0)
            {
                meshSegment = TrackNumDef.CurveMeshSegment;
            }
            trackViewer.GenerateTrack(obj, pathDataArray, meshSegment);
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

        public void ActionTrack(int lineIndex)
        {
            trackViewer.ActionTrack(lineIndex);
        }
        // 设置轨道的运动速度
        public float Speed
        {
            get { return trackViewer.GetSpeed(); }
            set
            {
                trackViewer.SetSpeed(value);
            }
        }

        public float runningTime
        {
            get { return m_runningTime; }
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

        // 设置轨道线的数目
        public void ResetTracklineNum(int num, bool bDisplayEffect)
        {
            float linespace = TrackNumDef.defaultlineSpace;
            this.ResetTracklineNum(num, linespace);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="num"></param>
        /// <param name="lineSpace">每根轨道间距</param>
        public void ResetTracklineNum(int num,float lineSpace)
        {
            this.trackViewer.SetTracklineNum(num,lineSpace);
        }
        
        /// <summary>
        /// 检测是否可以放置到轨道上
        /// </summary>
        /// <param name="trackline"></param>
        /// <returns></returns>
        public bool CheckPushValue()
        {
            return this.trackViewer.CheckPushValue();
        }

        public bool CheckPushValue(IPTTile tile, int lineIndex)
        {
            return this.trackViewer.CheckPushValue(tile, lineIndex);
        }

        // 压入节点数据
        //public bool PushValue(NodeObject node)
        public bool PushValue(IPTTile node,int lineIndex = -1)
        {
            if(lineIndex < 0)
            {
                // 采用默认的随机分配
                int lineNum = this.trackViewer.GetTracklineNum();

                int randValue = UnityEngine.Random.Range((int)0, (int)(lineNum * 100));
                if (randValue * 0.01f > (lineNum * 0.5f))
                {
                    lineIndex = m_lastTrackIndex + 1;
                }
                else
                {
                    lineIndex = m_lastTrackIndex - 1;
                }
                lineIndex = (lineIndex < 0) ? 0 : ((lineIndex >= lineNum) ? lineNum - 1 : lineIndex);

                if (!this.trackViewer.CheckPushValue(node,lineIndex))
                {
                    for(int i = 1; i< lineNum; ++i)
                    {
                        int newIndex = (lineIndex + i) % lineNum;
                        if(this.trackViewer.CheckPushValue(node,newIndex))
                        {
                            lineIndex = newIndex;
                            break;
                        }
                    }
                }
            }

            m_lastTrackIndex = lineIndex;

            return this.trackViewer.PushValue(node, lineIndex);
        }



        public void Start()
        {
            m_isMoveable = true;
            m_runningTime = 0;
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

            // Test code 
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    if (!m_isMoveable)
            //    {
            //        this.Start();
            //    }

            //    Object obj = Resources.Load("TileRes/NormalTile");
            //    if (obj != null)
            //    {
            //        GameObject go = GameObject.Instantiate(obj) as GameObject;
            //        NodeObject node = go.AddComponent<NodeObject>();

            //        this.PushValue(node);
            //    }
            //}
            ///////////////////////

            if (!m_isMoveable)
                return;

            m_runningTime += Time.deltaTime;

            trackViewer.OnUpdate();
            
        }        
    }
}

