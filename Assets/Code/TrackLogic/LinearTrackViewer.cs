/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/01 13:26:28
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack 
{
    public class LinearTrackViewer : TrackBase
    {
        /*

        public bool isDebug = false;

        public ITrackViewer trackViewer;

        public static float changingTime = 3f;

        private int m_TrackNum = 2;             // 轨道数量
        public int maxTrackNum = 4;
        private List<TrackLine> _trackLineList;
        private bool m_isMoveable = false;      // 是否可以运动
        private float m_fRunningTime = 0;       // 轨道运行的时间
        private float m_fTimeCircle = 6;        // 循环周期(轨道运行一个循环的时间)，用来表示运动的速度
        private float m_fTrackLength = 20f;      // 轨道的长度
        private float m_fSpace = 2f;            // 音符间距(m)
        private float m_fTrackWidth = 5;        // 轨道宽5米
        private float m_minLineSpace = 1.0f;    // 最宽的轨道间距
        public float[] m_lineSpaces = { 0f, 2.0f, 1.2f, 1.0f };
        public float m_adjustTime = 0.9f;
        private int _lastTrackIndex = 0;        // 最近分配的轨道编号


        private List<NodeObject> _prepareList;   // 预备队列，等待加入轨道的节点列表
        //private List<ModelNode> _operateList;   // 操控队列，进入并受轨道控制的节点列表
        private int m_maxCacheNum = 15;        // 最大的缓存数量，当超过这个数值后将拒绝接受数据


        /////////////// 临时数据，以后再详细设计整理
        protected Transform m_trackParent;        // 父节点，其坐标表示轨道终点
        //protected GameObject m_trackPanel;        // 轨道面板
        //protected Transform m_trackTrans;
        protected Vector3 m_trackPosition;        // 轨道的坐标位置，计算终点坐标
        protected Vector3 m_trackEndPos;

        protected MouseTouchArea m_touchArea;
        protected NodeObject m_lastNote;
        protected TrackPanel trackPanel;
        float timeSpace = 0;



        public bool Init(enTrackType trackType = enTrackType.Linear)
        {

            if (_trackLineList == null)
                _trackLineList = new List<TrackLine>();
            if (_prepareList == null)
                _prepareList = new List<NodeObject>();
            //if(_operateList == null)
            //    _operateList = new List<ModelNode>();

            m_isMoveable = false;
            m_fRunningTime = 0;

#if DEBUGTEST
                        // 得到轨道面板，后面修改为轨道实例实现
                        GameObject objParent = GameObject.FindGameObjectWithTag("TrackPanel");
                        if (objParent == null)
                        {
                            Debug.LogError("Error not find TrackPanel");
                        }
                        else
                        {
                            trackPanel = objParent.GetComponent<TrackPanel>();
                            if(trackPanel == null)
                            {
                                trackPanel = objParent.AddComponent<TrackPanel>();
                            }

                            m_trackParent = objParent.transform;
                            //m_trackPanel = m_trackParent.Find("TrackPanel").gameObject;
                            //m_trackTrans = m_trackPanel.transform;
                            if (trackPanel.transPanel)
                                m_trackPosition = trackPanel.transPanel.localPosition;
                            else
                                m_trackPosition = m_trackParent.localPosition;

                            m_trackEndPos = m_trackParent.localPosition;

                        }


#endif
            m_touchArea = GameObject.FindObjectOfType<MouseTouchArea>();
            if (m_touchArea == null)
            {
                Debug.LogError("@@@@@ not MouseTouchArea");
            }

            //// 设置轨道数量
            //ResetTracklineNum(m_TrackNum, true);
            return true;
        }

        public float runningTime
        {
            get { return m_fRunningTime; }
        }
        public float circleTime
        {
            get { return m_fTimeCircle; }
            set
            {
                m_fTimeCircle = value;
                //m_adjustTime = m_fAdjustStand /m_fTimeCircle;
            }
        }
        public float trackLength
        {
            get { return m_fTrackLength; }

            set
            { // 设置轨道的长度参数
                m_fTrackLength = value;
            }
        }
        public int trackNum
        {
            get { return m_TrackNum; }
        }
        public float space
        {
            get { return m_fSpace; }
            set { m_fSpace = value; }
        }

        public bool isRunning
        {
            get { return m_isMoveable; }
        }

        public void ResetTracklineNum(int num, bool displayUI)
        {
            Game.instance.StartCoroutine(SetTracklineNum(num, 0f, displayUI));
        }

        protected IEnumerator SetTracklineNum(int num, float fdelay, bool displayUI)
        {
            XSingleton<TrackManager>.GetInstance().Stop();

            if (fdelay > 0)
            {
                yield return new WaitForSeconds(fdelay);
            }

            if (displayUI)
            {
                GameData data = new GameData();
                data.state = GameState.Change;
                Sender.handlePostData("GameDataUpdate", data);
                yield return new WaitForSeconds(changingTime);
            }

            GameData data2 = new GameData();
            data2.state = GameState.GAMING;
            data2.score = GameManager.GetInstance().Score;
            Sender.handlePostData("GameDataUpdate", data2);

            m_TrackNum = num;
            int count = _trackLineList.Count;
            //for(int i = 0; i< count ;++ i)
            //{
            //    _trackLineList[i].Clear();
            //}            
            int sub = num - count;
            for (int i = 0; i < count; ++i)
            {
                TrackLine line = _trackLineList[i];
                //line.enabled = false;
                line.gameObject.SetActive(false);
            }

            trackPanel.PlayStartEffect(num);
            yield return new WaitForSeconds(1.0f);

            for (int j = 0; j < sub; ++j)
            {
                //TrackLine line = new TrackLine();

                // 实例化轨道实例
                GameObject objLine = GameObject.Instantiate(Game.instance.trackPrefab);
                TrackLine line = objLine.GetComponent<TrackLine>();
                if (line == null)
                {
                    line = objLine.AddComponent<TrackLine>();
                    // TODO: ....做轨道的初始化操作
                }

                _trackLineList.Add(line);
            }
            count = _trackLineList.Count;

            // 计算每一根轨道的位置
            float space = 0f;   // 间隔距离
            float offsetX = 0;  // 与中间线距离
            int midIndex = 0;
            Vector3 endPos = m_trackEndPos;

            //space = m_fTrackWidth / (num);
            //space = (space > m_minLineSpace) ? m_minLineSpace : space;
            if (m_lineSpaces != null && (num <= m_lineSpaces.Length))
            {
                space = m_lineSpaces[num - 1];
            }
            else
            {
                space = m_minLineSpace;
            }

            if (num % 2 == 0)
            {

                midIndex = (int)(num * 0.5f);
                for (int i = 0; i < num; ++i)
                {
                    if (i < midIndex)
                    {
                        offsetX = (i - midIndex + 0.5f) * space;
                        endPos.x = m_trackEndPos.x + offsetX;
                    }
                    else
                    {
                        offsetX = (i - midIndex + 0.5f) * space;
                        endPos.x = m_trackEndPos.x + offsetX;
                    }

                    _trackLineList[i].enabled = true;
                    _trackLineList[i].gameObject.SetActive(true);
                    _trackLineList[i].SetEndPosition(endPos);
                }
            }
            else
            {
                midIndex = (int)(num * 0.5f);
                for (int i = 0; i < num; ++i)
                {
                    offsetX = (i - midIndex) * space;
                    endPos.x = m_trackEndPos.x + offsetX;

                    _trackLineList[i].enabled = true;
                    _trackLineList[i].gameObject.SetActive(true);
                    _trackLineList[i].SetEndPosition(endPos);
                }
            }

            yield return null;

            XSingleton<TrackManager>.GetInstance().Continue();
        }
        public bool CheckPushValue()
        {
            if (_prepareList.Count >= m_maxCacheNum)
                return false;
            return true;
        }

        // 压入节点数据
        public bool PushValue(NodeObject node)
        {
            _prepareList.Add(node);

            return true;
        }

        public Vector3 GetTouchPosition()
        {
            if (m_touchArea == null)
            {
                return Vector3.zero;
            }
            return m_touchArea.touchTrigglePos;
        }

        public void Start()
        {
            //m_isMoveable = true;
            m_fRunningTime = 0;
            timeSpace = 0;
        }
        public void Stop()
        {
            m_isMoveable = false;
        }
        public void Continue()
        {
            m_isMoveable = true;
        }
        //public void Reset()
        //{
        //    Stop();
        //    Clear();
        //    Start();
        //}

        public NodeObject lastNote
        {
            get { return m_lastNote; }
        }

        public void Clear()
        {
            // 清空预备队列中的所有数据
            int count = _prepareList.Count;
            for (int i = count - 1; i >= 0; --i)
            {
                NodeObject node = _prepareList[i];
                _prepareList.RemoveAt(i);
                NodeManager.instance.RecoverNode(node);
            }
            _prepareList.Clear();

            // 清空轨道上的所有数据

            for (int i = 0; i < _trackLineList.Count; ++i)
            {
                _trackLineList[i].Clear();
            }
        }


        // 由Game驱动
        public void Update()
        {
            if (!m_isMoveable)
                return;

            UpdateOffset();

            //UpdateTrackNum();
        }

        // 更新运动偏移
        private void UpdateOffset()
        {
            if (!m_isMoveable)
            {
                return;
            }

            m_fRunningTime += Time.deltaTime;

            UpdateTrackOffset();

            UpdatePrepareList();

            for (int i = 0; i < m_TrackNum; ++i)
            {
                _trackLineList[i].OnUpdate();
                //_trackLineList[i].Update();
            }
        }


        private void UpdateTrackOffset()
        {

            // TODO 实现轨道的运动,后面会将其移到轨道实例中实现
            //Material mat = m_trackPanel.GetComponent<Renderer>().material;
            //if (mat == null)
            //    return;

            //Vector2 offset = mat.GetTextureOffset("_MainTex");
            //offset.y = 1 - (m_fRunningTime - 0.5f) / m_fTimeCircle;
            //mat.SetTextureOffset("_MainTex", offset);
        }

        // 更新预备队列中的数据，当有节点满足进入轨道条件则安排其进入轨道
        private void UpdatePrepareList()
        {
            if (_prepareList.Count > 0)
            {
                NodeObject node = _prepareList[0];
                if (node.startTime <= m_fRunningTime)
                {
                    _prepareList.RemoveAt(0);
                    //_operateList.Add(node);
                    int index = 0;
                    int randValue = UnityEngine.Random.Range((int)0, (int)(m_TrackNum * 100));
                    if (!node.isSubNote)
                    {
                        index = (int)(randValue * 0.01f);
                    }
                    else
                    {
                        if (randValue * 0.01f > (m_TrackNum * 0.5f))
                        {
                            index = _lastTrackIndex + 1;
                        }
                        else
                        {
                            index = _lastTrackIndex - 1;
                        }
                        index = (index < 0) ? 0 : ((index >= m_TrackNum) ? m_TrackNum - 1 : index);
                    }

                    PushIntoTackline(index, node);
                    _lastTrackIndex = index;
                }
            }
        }

        private void PushIntoTackline(int lineIndex, NodeObject node)
        {
            node.SetParent(m_trackParent);

            //node.SetActive(true);
            lineIndex %= m_TrackNum;

            if (_trackLineList[lineIndex] == null)
            {
                return;
            }
            _trackLineList[lineIndex].PushValue(node);

            node.Appear(lineIndex);

            m_lastNote = node;
        }


        protected void UpdateTrackNum()
        {
            //timeSpace += Time.deltaTime;
            //if(timeSpace > 10 )
            //{
            //    timeSpace = 0;
            //    if(trackNum < maxTrackNum)
            //    {
            //        ResetTracklineNum(trackNum + 1, true);
            //    }
            //}

        }
         * 
         * */
    }
}

