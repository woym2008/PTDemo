///* ========================================================
// *	作 者：ZhangShouYang 
// *	创建时间：2018/05/17 10:54:02
// *	版 本：v 1.0
// *	描 述：
//* ========================================================*/

//#define DEBUGTEST

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace PianoMini
//{
//    public class TrackLine:MonoBehaviour
//    {
//        public float width = 0.4f;                  
//        public GameObject highlightLine;             // 高亮的线
//        private MeshRenderer highlightRender = null;

//        private Transform _trans;
//        private List<NodeObject> _nodeList;              // 轨道上的滑块
//        private Vector3 _startPosition = Vector3.zero;   // 轨道起点
//        private Vector3 _endPosition = Vector3.zero;
//        private float halfWidth = 0f;
//        private bool isHovered = false;

//        void Awake()
//        {
//            _nodeList = new List<NodeObject>();
//            _trans = transform;

//            if(highlightLine != null)
//            {
//                highlightRender = highlightLine.GetComponent<MeshRenderer>();
//                highlightRender.enabled = false;
//            }
//            isHovered = false;
//        }

//        void Start()
//        {
//            halfWidth = width * 0.5f;
//        }
//        //public TrackLine()
//        //{ }

//        public void PushValue(NodeObject node)
//        {
//            node.position = _startPosition;
//            _nodeList.Add(node);
//        }
//        public void SetEndPosition(Vector3 pos)
//        {
//            pos.y += 0.1f;
//            _endPosition = _startPosition = pos;
//            _startPosition.z = pos.z + TrackManager.instance.trackLength;
//            if(_trans == null)
//            {
//                _trans = transform;
//            }
//            _trans.position = _endPosition;
//        }

//        public void Clear()
//        {
//            int count = _nodeList.Count;
//            for (int i = count - 1; i >= 0; --i)
//            {
//                NodeObject node = _nodeList[i];

//                NodeManager.instance.RecoverNode(node);                
//            }
//            _nodeList.Clear();
//        }

//        public void OnUpdate()
//        {
//            UpdatePosition();
//            UpdateAliveTime();

//            UpdateMouseHover();
//        }

//        private void UpdatePosition()
//        {
//            int count = _nodeList.Count;
//            NodeObject node = null;
//            for (int i = count - 1; i >= 0; --i)
//            {
//                node = _nodeList[i];
//                {
//                    float progress = (node.startTime - TrackManager.instance.runningTime) / TrackManager.instance.circleTime;
                  
//                    Vector3 position = _startPosition;

//                    position.z += progress * TrackManager.instance.trackLength;
//                    node.position = position;
//                }
//            }


//        }
//        private void UpdateAliveTime()
//        { 
//            int count = _nodeList.Count;
//            for (int i = count - 1; i >= 0; --i)
//            {
//                NodeObject node = _nodeList[i];

//                if (!node.IsAlive(TrackManager.instance.runningTime, (TrackManager.instance.circleTime*TrackManager.instance.m_adjustTime)))
//                {
//                    // 判断是否触碰过，没有的话游戏结束
//                    if (!node.IsPressed)
//                    {
//                        if (TrackManager.instance.isDebug)
//                        {
//                            node.DebugTrggerEnter();
//                        }
//                        else
//                        {
//                            TrackManager.instance.Stop();

//                            GameManager.GetInstance().endGame();

//                            node.IsPressed = true;
//                            node.DisappearEffect();
//                        }
                        
//                    }
//                    else
//                    {
//                        // 回收node 节点
//                        _nodeList.RemoveAt(i);
//                        NodeManager.instance.RecoverNode(node); 
//                    }
                                       
//                }
//            }
//        }

//        Vector3 touchPos;
//        void UpdateMouseHover()
//        {
//            touchPos = TrackManager.instance.GetTouchPosition();
            
//            if (Mathf.Abs(_trans.position.x - touchPos.x)<= halfWidth)
//            {
//                if(!isHovered)
//                {
//                    isHovered = true;
//                    highlightRender.enabled = true;
//                }
//            }
//            else
//            {
//                if(isHovered)
//                {
//                    isHovered = false;
//                    highlightRender.enabled = false;
//                }
//            }
//        }
//    }
//}

