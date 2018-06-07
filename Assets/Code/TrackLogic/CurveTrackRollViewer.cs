/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/01 13:15:48
 *	版 本：v 1.0
 *	描 述：按照样条曲线进行运动的轨道,通过路径点循环生成轨道路面
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
{
    public class CurveTrackRollViewer : TrackBase
    {
        public class CacheData
        {
            public int lineIndex = 0;
            public IPTTile node;
        }


        public GameObject trackRoot;

        public Spline _spline;
        SplineMesh _splineMesh;
        MeshRenderer _meshRender;
       
        //public Transform _player;

        private float runnedLength = 0f;
        public float progress = 0f;

        private List<CacheData> _prepareList;
        private int m_maxCacheNum = 20;
        //private List<IPTTile> _operateList;

        private List<CurveTrackLine> _lineList = new List<CurveTrackLine>();
        public override void Init()
        {
            Debug.Log("SplineTrackViewer Init");

            if (_prepareList == null)
            {
                _prepareList = new List<CacheData>();
            }
            else
            {
                RecoveryNode(ref _prepareList);
            }

            trackRoot = GameObject.Find("TrackRoot");
            if (trackRoot == null)
            {
                trackRoot = new GameObject("TrackRoot");
                trackRoot.transform.position = Vector3.zero;
            }
                        
            _spline = trackRoot.GetComponentInChildren<Spline>();
            _splineMesh = trackRoot.GetComponentInChildren<SplineMesh>();
            if(_spline == null)
            {
                GameObject objSpline = new GameObject("SplineRoot");
                objSpline.transform.parent = trackRoot.transform;
                objSpline.transform.localPosition = Vector3.zero;
                _spline = objSpline.AddComponent<Spline>();
            }
            if (_splineMesh == null)
            {
                GameObject objSplineMesh = new GameObject("SplineMeshRoot");
                objSplineMesh.transform.parent = trackRoot.transform;
                objSplineMesh.transform.localPosition = Vector3.zero;
                _splineMesh = objSplineMesh.AddComponent<SplineMesh>();
            }

            _spline.interpolationMode = Spline.InterpolationMode.Hermite;
            _spline.rotationMode = Spline.RotationMode.Tangent;
            _spline.tangentMode = Spline.TangentMode.UseNormalizedTangents;
            _spline.perNodeTension = false;
            _spline.tension = 3.0f;
            _spline.updateMode = Spline.UpdateMode.DontUpdate;

            _splineMesh.spline = _spline;
            _splineMesh.updateMode = SplineMesh.UpdateMode.WhenSplineChanged;
            _splineMesh.uvMode = SplineMesh.UVMode.InterpolateV;
            _splineMesh.uvScale = Vector2.one;
            _splineMesh.xyScale = Vector2.one;
            _splineMesh.segmentCount = 100;
            _splineMesh.splitMode = SplineMesh.SplitMode.DontSplit;

            _meshRender = _splineMesh.gameObject.GetComponent<MeshRenderer>();
            if (_meshRender == null)
            {
                _meshRender = _splineMesh.gameObject.AddComponent<MeshRenderer>();
            }

            return;
        }

        public override Vector3 GetPosition(float parameter, int lineIndex)
        {
            Vector3 pos = _spline.GetPositionOnSpline(parameter);
            return pos;
        }
        public override Quaternion GetRotation(float paramater, int lineIndex) 
        {
            Quaternion rotation = _spline.GetOrientationOnSpline(paramater);
            return rotation;
        }

        public override void SetTracklineNum(int num, float lineSpace)
        {
            base.SetTracklineNum(num, lineSpace);
                        
            int midIndex = 0;
            float offsetX = 0f;
            if (num % 2 == 0)
            {
                midIndex = (int)(num * 0.5f);
                for (int i = 0; i < num; ++i)
                {
                    if (i < midIndex)
                    {
                        offsetX = (i - midIndex + 0.5f) * _data.lineSpace;
                    }
                    else
                    {
                        offsetX = (i - midIndex + 0.5f) * _data.lineSpace;
                    }

                    if (_lineList.Count <= i)
                    {
                        CurveTrackLine line = new CurveTrackLine(this);
                        line.lineIndex = i;
                        _lineList.Add(line);
                    }
                    _lineList[i].OffsetX = offsetX;
                }
            }
            else
            {
                midIndex = (int)(num * 0.5f);
                for (int i = 0; i < num; ++i)
                {
                    offsetX = (i - midIndex) * _data.lineSpace;

                    if (_lineList.Count <= i)
                    {
                        CurveTrackLine line = new CurveTrackLine(this);
                        line.lineIndex = i;
                        _lineList.Add(line);
                    }

                    _lineList[i].OffsetX = offsetX;
                }
            }
        }

        public override void SetTrackWidth(float width)
        {
            this._data.width = width;
            Vector2 scale = _splineMesh.xyScale;
            scale.x = width;
            this._splineMesh.xyScale = scale;
            this._splineMesh.UpdateMesh();
        }

        public override void SetTrackHeight(float height)
        {
            this._data.height = height;
            Vector2 scale = _splineMesh.xyScale;
            scale.y = height;
            this._splineMesh.xyScale = scale;
            this._splineMesh.UpdateMesh();
        }
        public override void OnAwake(bool isStart)
        {
            base.OnAwake(isStart);
            if (isStart)
            {
                this.runnedLength = 0f;
                this.progress = 0f;
            }
            if (this._data.lineNum <= 0)
            {
                this._data.lineNum = 1;
            }
        }

        public override void HangUp()
        {
            base.HangUp();
        }
        public override void OnClear()
        {
            RecoveryNode(ref _prepareList);
            _prepareList.Clear();

            for (int i = 0; i < _lineList.Count; ++i )
            {
                _lineList[i].Clear();
            }
            //RecoveryNode(ref _operateList);            
            //_operateList.Clear();
        }

        public override void OnDestroy()
        {
            base.OnDestroy();
        }

        public override void SetSpeed(float value)
        {
            base.SetSpeed(value);
        }
        public override float GetProgressRatio(float value)
        {
            if (this._data.tracklength <= 0)
            {
                return 0;
            }
            return (value / this._data.tracklength);
        }
        public override bool CheckPushValue()
        {
            if (_prepareList.Count >= m_maxCacheNum)
            {
                return false;
            }
            return true;
        }
        public override bool PushValue(IPTTile node, int lineIndex)
        {
            CacheData data = new CacheData();
            data.lineIndex = lineIndex;
            data.node = node;

            _prepareList.Add(data);
            return true;
        }

        private void calculateTrackLength()
        {
            if (_spline == null)
            {
                _data.tracklength = 1;
                return;
            }
            _data.tracklength = 1;

            SplineNode[] nodes = _spline.SplineNodes;
            if (nodes == null)
            { return; }

            Vector3 lastPos = nodes[0].Position;
            float length = 0f;
            for (int i = 0; i < nodes.Length; ++i)
            {
                length += Vector3.Magnitude(nodes[i].Position - lastPos);
                lastPos = nodes[i].Position;
            }

            if (length <= 0f)
            {
                _data.tracklength = 1;
            }
            else
            {
                _data.tracklength = length;
            }


        }

        public override void GenerateTrack(GameObject obj, CurveNodeData[] pathDataArray)
        {
            SplineNode[] preNodeArr = _spline.SplineNodes;
            if(preNodeArr != null && preNodeArr.Length > 0)
            {
                for (int i = 0; i < preNodeArr.Length; ++ i )
                {
                    GameObject.Destroy(preNodeArr[i].gameObject);
                }
            }            
            
            List<SplineNode> nodelist = new List<SplineNode>();
            for (int index = 0; index < pathDataArray.Length; ++ index )
            {
                GameObject objNode = new GameObject("node" + index);
                objNode.transform.parent = _spline.transform;

                SplineNode node = objNode.AddComponent<SplineNode>();
                nodelist.Add(node);
            }
            _spline.splineNodesArray = nodelist;
            for(int j = 0; j< pathDataArray.Length; ++ j)
            {
                _spline.splineNodesArray[j].Position = pathDataArray[j].position;
                _spline.splineNodesArray[j].Rotation = pathDataArray[j].rotation;
            }
            _spline.UpdateSpline();

            _splineMesh.baseMesh = obj.GetComponent<MeshFilter>().mesh;

            _meshRender.material = obj.GetComponent<Renderer>().material;

            _splineMesh.UpdateMesh();

            calculateTrackLength();
        }



        public override void OnUpdate()
        {

            if (!_data.isRunning || _data.isFinished)
            {
                return;
            }

            _data.runnungTime += Time.deltaTime;

            UpdateTrackRunning();

            UpdatePrepareList();

            UpdateOperateList();
        }

        private void UpdateTrackRunning()
        {
            float deltaLen = this._data.speed * Time.deltaTime;
            float deltaProgress = deltaLen / this._data.tracklength;

            this.progress += deltaProgress;

            Vector3 position = this._spline.GetPositionOnSpline(this.progress);
            Quaternion rotation = this._spline.GetOrientationOnSpline(this.progress);

            //_player.position = position;
            //_player.rotation = rotation;

            if (this.progress >= 1.0f)
            {
                OnFinished();
            }
        }

        // 更新预备队列中的数据，当有节点满足进入轨道条件则安排其进入轨道
        private void UpdatePrepareList()
        {
            if (_prepareList.Count > 0)
            {
                CacheData data = _prepareList[0];

                if (data.node.getStartProcess() <= this.progress)
                {
                    _prepareList.RemoveAt(0);

                    PushIntoTackline(0, data.node);
                }
            }

        }

        private void PushIntoTackline(int lineIndex, IPTTile node)
        {
            CurveTrackLine line = _lineList[lineIndex];
            if(line == null)
            {
                Debug.LogError("not contain this line index " + lineIndex);
            }

            line.PushValue(node);

            //node.Appear(lineIndex);

            ////node.SetParent(transformTrackRoot);

            //lineIndex %= this._data.lineNum;
            //// 倒叙
            //_operateList.Insert(0, node);

            //float progress = this.progress + node.getPositionProgress();

            //Vector3 position = this._spline.GetPositionOnSpline(progress);
            //Quaternion rotation = this._spline.GetOrientationOnSpline(progress);

            ////node.progress = progress;
            //node.setProcess(progress);

            ////node.transform.position = position;
            ////node.transform.rotation = rotation;
            //node.setPosition(position);
            //node.setRotation(rotation);
        }

        private void UpdateOperateList()
        {
            for(int i = 0; i< _lineList.Count; ++ i)
            {
                _lineList[i].OnUpdate();
            }
            
        }

        public void RecoveryNode(IPTTile node)
        {
            //NodeManager.instance.RecoverNode(node);
            Debug.LogWarning("未实现节点回收");
        }

        public void RecoveryNode(ref List<IPTTile> list)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                IPTTile node = list[i];
                list.RemoveAt(i);
                RecoveryNode(node);
            }
            list.Clear();
        }
        public void RecoveryNode(ref List<CacheData> list)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                IPTTile node = list[i].node;
                list.RemoveAt(i);
                RecoveryNode(node);
            }
            list.Clear();
        }
    }



    ///////////////////////////////////////////////////////////
    //// 曲面轨道线类

    public class CurveTrackLine:TileLineBase
    {
        private CurveTrackRollViewer _trackViewer;
        
        public float OffsetX;
        public float OffsetY;

        private List<IPTTile> _operateList = new List<IPTTile>();

        public CurveTrackLine(CurveTrackRollViewer trackViewer)
        {
            this._trackViewer = trackViewer;
        }

        public override void PushValue(IPTTile node)
        {
            base.PushValue(node);

            node.Appear(lineIndex);

            // 倒叙
            _operateList.Insert(0, node);

            float progress = this._trackViewer.progress + node.getPositionProgress();

            Vector3 position = this._trackViewer._spline.GetPositionOnSpline(progress);
            Quaternion rotation = this._trackViewer._spline.GetOrientationOnSpline(progress);

            node.setProcess(progress);
            position.x += OffsetX;
            node.setPosition(position);
            node.setRotation(rotation);
        }

        public override void OnUpdate()
        {
            int count = _operateList.Count;

            for (int i = count - 1; i >= 0; --i)
            {
                IPTTile node = _operateList[i];

                UpdateTilePosition(node);

                if (node.getStartProcess() + TrackNumDef.tileLifeProgress <= this._trackViewer.progress)
                {
                    _operateList.RemoveAt(i);
                    this._trackViewer.RecoveryNode(node);
                }
            }
        }

        public override void Clear()
        {
            this._trackViewer.RecoveryNode(ref this._operateList);
            this._operateList.Clear();

        }

        private void UpdateTilePosition(IPTTile node)
        {
            //node.progress = node.progress - (Time.deltaTime * Game.instance.tileSpeed);
            Vector3 position = this._trackViewer._spline.GetPositionOnSpline(node.getProcess());
            Quaternion rotation = this._trackViewer._spline.GetOrientationOnSpline(node.getProcess());

            position.x += OffsetX;

            node.setPosition(position);

            node.setRotation(rotation);
        }
    }
}


