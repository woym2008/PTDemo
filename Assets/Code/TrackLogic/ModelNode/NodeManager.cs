/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/17 10:54:02
 *	版 本：v 1.0
 *	描 述：游戏中的音乐块抽象为节点 Node,有关node的类表示的便是音乐键。
 *	    管理器作用相当于Producer,负责生成对应的音乐块，具体的生成逻辑都在这里完成，包括根据音乐节奏，生成算法等。
 *	    并赋予对应的滑块相应的属性，比如形状、时间片、分数、分数的计算公式等
 *	注：不在管理器中添加node列表，简化管理器功能，将对node的控制操作分散在轨道和node自身，管理器只承担Producer功能
 *	    该类全局只会实例化一次，因此可以设计为单例模式
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
{
    public class NodeManager: XSingleton<NodeManager>
    {       

        public class NodeData
        {
            //public float time = 0f;    // "time" 时间刻
            //public float lenght = 0f;  // "length" 时长
            //public int mtype = 0;      // "mtype" 类型
            //public int index = 0;      // 在所有列表中的位置
            //public int subIndex = 0;   // 内部的索引，比如长发音
            //public bool triggleChanging = false; // 出发变轨


            public NodeManager.eNodeType eType;
            // 节点必须的一些数据
            public float startTime = 0;      // 开始的时间刻
            public float startProgrss = 0f;  // 开始的进度
            public float length = 1.0f;      // 播放时间长度
            public int mtype = 0;            // "mtype" 类型
            public int trackIndex = 0;       // 轨道的标识
            public int index = 0;            // 在所有列表中的位置  
            public int subIndex = 0;         // 内部的索引，比如长发音
            public bool voiceable = true;
            public bool triggleChanging = false;// 出发变轨

            public bool avaliable = true;
            public bool isPressed = false;      // 是否被点击过
            public Vector3 ori_scale = Vector3.one;
            public Quaternion ori_rotation = Quaternion.identity;
            public float posProgrss = 0f;     // 位置摆放时滞后多少进度
            public float curProgress = 0f;      // 当前的进度

            public void Reset()
            {
                startTime = 1000;
                length = 1.0f;
                trackIndex = 0;
                startProgrss = 0;
            }

        }

        // 滑块节点的类型
        public enum eNodeType{
            Normal = 0,     // 普通
            Len = 1,        // 长块
            Combine = 2,    // 复合块
        }

        // 缓存池
        private Dictionary<int, GameObject> _prefabList = null;
        private Dictionary<int, List<NodeObject>> _nodePool = null;
        private List<NodeData> _dataCache = null;

        public int m_CacheDefualtNum = 5;     // 每类节点资源默认的初始数量
        private Transform _transRoot;
        public int defaultTrackNum = 2;
        public int maxTrackNum = 4;
        private int changeingSpaceNum = 8;

        // Init Operate初始化操作
        public bool Init()
        {
            // TODO: 初始化一些配置信息,下面的代码需要进一步整理和优化，需要等到资源模块完成后重新整理为异步操作
            _prefabList = new Dictionary<int, GameObject>();
            _nodePool = new Dictionary<int, List<NodeObject>>();
            _dataCache = new List<NodeData>();

            InitRootObject();            
            
            //int len = Game.Instance.blockPrefabs.Length;
            //if(len > 0)
            //{
            //    for(int i = 0; i<len; ++ i)
            //    {
            //        GameObject obj = Game.Instance.blockPrefabs[i];
            //        _prefabList.Add(i, obj);

            //        CrateModelNodePool(i, obj, m_CacheDefualtNum);
            //    }
            //}           

            return true;
        }

        private void InitRootObject()
        {
            if(_transRoot == null)
            {
                GameObject objRoot = new GameObject("BlockCache");
                _transRoot = objRoot.transform;
                _transRoot.position = new Vector3(0,500,0);
                GameObject.DontDestroyOnLoad(objRoot);
            }
        }

        public Transform cacheRoot
        {
            get { return _transRoot; }
        }
        // 创建一类节点缓存
        private void CrateModelNodePool(int mType, GameObject prefabe, int num)
        {
            if(_nodePool.ContainsKey(mType))
            {
                Debug.LogWarning("Already contain this type node cache "+mType);
                return;
            }
            if(prefabe == null)
            {
                Debug.LogError("Created failed ,node resource is a null value");
                return;
            }

            List<NodeObject> list = new List<NodeObject>();
            _nodePool.Add(mType, list);

            for(int i = 0;i< num; ++ i)
            {               
                eNodeType eType = (eNodeType)mType;

                NodeObject node = CreateEmptyNode(eType, prefabe);
               
                //list.Add(node);
                RecoverNode(node);
            }
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="eType"></param>
        /// <param name="prefab">源资源prefab</param>
        /// <returns></returns>
        private NodeObject CreateEmptyNode(eNodeType eType,UnityEngine.Object prefab)
        {
            if (prefab == null)
                return null;

            NodeObject node = null;
            GameObject go = GameObject.Instantiate(prefab) as GameObject;
            go.name = prefab.name;

            switch (eType)
            {
                case eNodeType.Normal:
                    node = go.GetComponent<NormalNode>();
                    if(node == null)
                    {
                        node = go.AddComponent<NormalNode>();
                    }
                    node.eType = eNodeType.Normal;
                    break;
                case eNodeType.Len:
                    node = go.GetComponent<NormalNode>();
                    if(node == null)
                    {
                        node = go.AddComponent<NormalNode>();
                    }
                    node.eType = eNodeType.Len;
                    break;

                case eNodeType.Combine:
                    node = go.GetComponent<NormalNode>();
                    if(node == null)
                    {
                        node = go.AddComponent<NormalNode>();
                    }
                    node.eType = eNodeType.Combine;
                    break;
                default:
                    node = go.GetComponent<NormalNode>();
                    if(node == null)
                    {
                        node = go.AddComponent<NormalNode>();
                    }
                    node.eType = eNodeType.Normal;
                    break;
            }

            node.Init();

            node.SetParent(cacheRoot);

            //node.SetActive(false);

            return node;
        }

        private NodeObject PickAvaliableNode(eNodeType eType)
        {
            int mType = (int)eType;
            List<NodeObject> list = null;
            NodeObject node = null;

            if(!_nodePool.TryGetValue(mType,out list))
            {
                GameObject prefabe = _prefabList[mType];
                if (prefabe == null)
                {
                    Debug.LogError("Not contain this type prefab "+mType);
                    return null;
                }
                CrateModelNodePool(mType, prefabe, m_CacheDefualtNum);
            }

            int count = list.Count;
            if(count <= 0)
            {
                Object prefabe = _prefabList[mType];
                if (prefabe == null)
                {
                    Debug.LogError("Not contain this type prefab@@@@@@ " + mType);
                    return null;
                }

                node = CreateEmptyNode(eType, prefabe);

                return node;
            }

            node = list[0];
            list.RemoveAt(0);

            return node;
        }

        #region Create Node Operates

        public void Restart()
        {
            // TODO 获取当前播放的歌曲
            //MusicData data = GameManager.GetInstance().getMusicData(3);
            //if (data != null)
            //{
            //    Restart(data.cells.ToArray());
            //}
        }
        //public void Restart(MusicCell[] datas)
        //{
        //    _dataCache.Clear();
        //    TrackManager.instance.Stop();
        //    TrackManager.instance.Clear();
        //    ReceiveBeatsData(datas);
        //}

        // 接收到音符数据集
        //public bool ReceiveBeatsData(BeatData[] datas)
        //public bool ReceiveBeatsData(MusicCell[] datas)
        //{
        //    if (datas.Length <= 0)
        //    {
        //        Debug.LogError("not contain beats data");
        //        return false;
        //    }

        //    int bpmValue = GameManager.GetInstance().BPM;  // 每分钟落打90节拍;
        //    //bpmValue = 500;

        //    //float unitTime = 60.0f / bpmValue; //每个节拍的所用的时间
        //    float unitTime = GameManager.GetInstance().getDropBaseTime();

        //    //float speed = GameDef.tilespace / unitTime;
        //    //TrackManager.GetInstance().Speed = speed;

        //    float unitProgress = TrackManager.GetInstance().GetProgressRatio(GameDef.tilespace);
        //    float posProgrss = unitProgress * GameDef.preTileSpace;
           
        //    int length = datas.Length;
        //    int extralAdd = 0;
        //    int deltaTrack = maxTrackNum - defaultTrackNum;
        //    int changeSubNum = (int)(length /(deltaTrack+1));
        //    int changeIndex = 0;
        //    float extralTime = 0;
        //    bool triggle = false;

        //    for (int i = 0; i < length ; ++i)
        //    {
                
        //        if(datas[i].length > 0)
        //        {
        //            for (int num = 0; num < datas[i].length; ++num)
        //            {
        //                NodeData nodeInfo = new NodeData();

        //                nodeInfo.startTime = unitTime * (i + extralAdd + num);
        //                nodeInfo.startProgrss = unitProgress * (i + extralAdd + num);
        //                nodeInfo.posProgrss = posProgrss;
        //                nodeInfo.length = datas[i].length;
        //                int mType = datas[i].mark;
        //                nodeInfo.mtype = mType;
        //                nodeInfo.index = i + extralAdd;
        //                nodeInfo.subIndex = num;

        //                nodeInfo.triggleChanging = false;
        //                //changeIndex++;
        //                //if (deltaTrack > 0 &&
        //                //    changeIndex >= changeSubNum && mType == 1)
        //                //{
        //                //    nodeInfo.triggleChanging = true;
        //                //    changeIndex = 0;
        //                //    deltaTrack--;
        //                //    triggle = true;
        //                //}
        //                //else
        //                //{
        //                //    nodeInfo.triggleChanging = false;
        //                //}

                        
        //                ProduceModelNode(mType, nodeInfo);
        //            }
        //            if (datas[i].length > 1)
        //            {
        //                extralAdd += (datas[i].length -1);
        //            }

        //            if (triggle)
        //            {
        //                extralAdd += changeingSpaceNum;
        //                triggle = false;
        //            }
                   
        //        }                         
                
        //    }

        //    Game.instance.StartCoroutine(CoroutinePrepare());
            
        //    return true;
        //}

        IEnumerator CoroutinePrepare()
        {
            yield return null;
            TrackManager.instance.ResetTracklineNum(defaultTrackNum, false);
            yield return null;

            TrackManager.instance.Start();
        }
        
        /// <summary>
        /// 根据时间片长短创建
        /// </summary>
        /// <param name="dataSet">数据集</param> dataInfo的数据结构  
        /// <returns></returns>
        public NodeObject ProduceModelNode(int mType, NodeData dataInfo)
        {
            if (!TrackManager.instance.CheckPushValue())
            {
                dataInfo.mtype = mType;
                
                _dataCache.Insert(0, dataInfo); // 时间倒叙
               
                return null;
            }

            // Creat modelnode,生成节点
            NodeObject node = PickAvaliableNode((eNodeType)mType);
            if (node == null)
                return null;
            
            // ToDo: Set node attriInfo,将数据集传给Node（钢琴块）节点
            node.ResetDataInfo(dataInfo);

            // Put on track ,将节点放入轨道
            TrackManager.instance.PushValue(node);

            return node;
        }

        #endregion

#region Recover Node Operate
        public void RecoverNode(NodeObject node)
        {
            eNodeType eType = node.eType;
            int mType = (int)eType;
            List<NodeObject> list = null;

            if(!_nodePool.TryGetValue(mType,out list))
            {
                GameObject.Destroy(node);

                Debug.LogError("RecoverNode failed not contain this type pool");                
                return;
            }         

            node.Disappear();

            node.SetParent(cacheRoot);

            node.position = Vector3.zero;
            
            list.Add(node);
        }
#endregion

        // 更新不由引擎驱动，而是由Game驱动
        public void Update()
        {
            int count = _dataCache.Count;
 
            for(int i = count- 1; i>= 0; -- i)
            {
                if (!TrackManager.instance.CheckPushValue())
                    break;

                NodeData dataInfo = _dataCache[i];
                int mType = dataInfo.mtype;

                NodeObject obj = ProduceModelNode(mType,dataInfo);
                if(obj != null)
                {
                    _dataCache.RemoveAt(i);
                }
            }
        }
    }
}

