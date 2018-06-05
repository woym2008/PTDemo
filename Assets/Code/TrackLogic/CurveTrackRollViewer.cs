///* ========================================================
// *	作 者：ZhangShouYang 
// *	创建时间：2018/06/01 13:15:48
// *	版 本：v 1.0
// *	描 述：按照样条曲线进行运动的轨道,通过路径点循环生成轨道路面
//* ========================================================*/

//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//namespace Demo.TileTrack
//{
//    public class CurveTrackRollViewer : TrackBase
//    {
//        public GameObject trackRoot;
//        Transform transformTrackRoot;

//        Spline _spline;
//        SplineMesh _splineMesh;
//        public Transform _player;

//        private float runnedLength = 0f;
//        private float progress = 0f;

//        private List<NodeObject> _prepareList;
//        private int m_maxCacheNum = 6;
//        private List<NodeObject> _operateList;
       

//        public override void Init()
//        {
//            Debug.Log("SplineTrackViewer Init");
//            if(_prepareList == null)
//            {
//                _prepareList = new List<NodeObject>();
//            }else
//            {
//                RecoveryNode(ref _prepareList);
//            }

//            if (_operateList == null)
//            {
//                _operateList = new List<NodeObject>();
//            }
//            else
//            {
//                RecoveryNode(ref _operateList);
//            }
            

//            trackRoot = GameObject.Find("TrackRoot");
//            if(trackRoot == null)
//            {
//                Debug.LogError("not find TrackRoot Gameobject");
//                return;
//            }
//            transformTrackRoot = trackRoot.transform;

//            GameObject objPlayer = GameObject.FindGameObjectWithTag("Player");
//            _player = objPlayer.transform;

//            _spline = trackRoot.GetComponentInChildren<Spline>();
//            _splineMesh = trackRoot.GetComponentInChildren<SplineMesh>();

//            if(_spline == null || _splineMesh == null)
//            {
//                Debug.LogError("not find Spline or SplineMesh Component");
//                return;
//            }

//            _spline.updateMode = Spline.UpdateMode.DontUpdate;
//            _splineMesh.updateMode = SplineMesh.UpdateMode.DontUpdate;

//            calculateTrackLength();

//            return;
//        }

//        public override void SetTracklineNum(int num)
//        {
//            base.SetTracklineNum(num);
//            Debug.Log("Set Track Line num operate");
//        }

//        public override void OnAwake(bool isStart)
//        {
//            base.OnAwake(isStart);
//            if(isStart)
//            {
//                this.runnedLength = 0f;
//                this.progress = 0f;
//            }
//            if(this._data.lineNum <= 0)
//            {
//                this._data.lineNum = 1;
//            }
//        }

//        public override void HangUp()
//        {
//            base.HangUp();
//        }
//        public override void OnClear()
//        {
//            RecoveryNode(ref _prepareList);
//            RecoveryNode(ref _operateList);

//            _prepareList.Clear();
//            _operateList.Clear();
//        }

//        public override void OnDestroy()
//        {
//            base.OnDestroy();
//        }
                
//        public override void SetSpeed(float value)
//        {
//            base.SetSpeed(value);
//        }
//        public override float GetProgressRatio(float value)
//        {
//            if(this._data.tracklength <= 0)
//            {
//                return 0;
//            }
//            return (value / this._data.tracklength);
//        }
//        public override bool CheckPushValue()
//        {
//            if(_prepareList.Count >= m_maxCacheNum)
//            {
//                return false;
//            }
//            return true;
//        }
//        public override bool PushValue(NodeObject node)
//        {
//            _prepareList.Add(node);
//            return true;
//        }

//        private void calculateTrackLength()
//        {
//            if(_spline == null)
//            {
//                _data.tracklength = 1;
//                return;
//            }
//            _data.tracklength = 1;

//            SplineNode[] nodes = _spline.SplineNodes;
//            if(nodes == null)
//            {return;}

//            Vector3 lastPos = nodes[0].Position;
//            float length = 0f;
//            for(int i = 0; i< nodes.Length; ++ i)
//            {
//                length += Vector3.Magnitude(nodes[i].Position - lastPos);
//                lastPos = nodes[i].Position;
//            }

//            if(length <= 0f)
//            {
//                _data.tracklength = 1;
//            }
//            else
//            {
//                _data.tracklength = length;
//            }
            

//        }



//        public override void OnUpdate()
//        {

//            if(!_data.isRunning || _data.isFinished)
//            {
//                return;
//            }

//            _data.runnungTime += Time.deltaTime;

//            UpdateTrackRunning();

//            UpdatePrepareList();

//            UpdateOperateList();
//        }

//        private void UpdateTrackRunning()
//        {
//            float deltaLen = this._data.speed * Time.deltaTime;
//            float deltaProgress = deltaLen / this._data.tracklength;

//            this.progress += deltaProgress;

//            Vector3 position = this._spline.GetPositionOnSpline(this.progress);
//            Quaternion rotation = this._spline.GetOrientationOnSpline(this.progress);

//            _player.position = position;
//            _player.rotation = rotation;

//            if(this.progress >= 1.0f)
//            {
//                OnFinished();
//            }
//        }

//        // 更新预备队列中的数据，当有节点满足进入轨道条件则安排其进入轨道
//        private void UpdatePrepareList()
//        {
//            if (_prepareList.Count > 0)
//            {
//                NodeObject node = _prepareList[0];
//                if(node.startProgress <= this.progress)
//                {
//                    _prepareList.RemoveAt(0);

//                    PushIntoTackline(0, node);
//                }                
//            }

//        }

//        private void PushIntoTackline(int lineIndex, NodeObject node)
//        {
//            node.Appear(lineIndex);

//            node.SetParent(transformTrackRoot);

//            lineIndex %= this._data.lineNum;
//            // 倒叙
//            _operateList.Insert(0, node);            

//            float progress = this.progress + node.positionProgress;

//            Vector3 position = this._spline.GetPositionOnSpline(progress);
//            Quaternion rotation = this._spline.GetOrientationOnSpline(progress);

//            node.progress = progress;

//            node.transform.position = position;
//            node.transform.rotation = rotation;
//        }

//        private void UpdateOperateList()
//        {
//            int count = _operateList.Count;

//            for (int i = count - 1; i >= 0; --i)
//            {
//                NodeObject node = _operateList[i];

//                UpdateTilePosition(node);

//                if (node.startProgress + TrackNumDef.tileLifeProgress <= this.progress)
//                {
//                    _operateList.RemoveAt(i);
//                    NodeManager.instance.RecoverNode(node);
//                }
//            }
//        }

//        private void UpdateTilePosition(NodeObject node)
//        {
//            //node.progress = node.progress - (Time.deltaTime * Game.instance.tileSpeed);

//            Vector3 position = this._spline.GetPositionOnSpline(node.progress);
//            Quaternion rotation = this._spline.GetOrientationOnSpline(node.progress);
//            node.transform.position = position;
//            node.transform.rotation = rotation;
//        }

//        private void RecoveryNode(NodeObject node)
//        {
//            NodeManager.instance.RecoverNode(node);
//        }
//        private void RecoveryNode(ref List<NodeObject> list)
//        {
//            for (int i = list.Count - 1; i >= 0; --i )
//            {
//                NodeObject node = list[i];
//                list.RemoveAt(i);
//                RecoveryNode(node);
//            }
//            list.Clear();
//        }
                
//    }
//}


