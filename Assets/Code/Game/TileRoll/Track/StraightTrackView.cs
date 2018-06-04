using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class StraightTrackView : MonoBehaviour, ITileTrackView
    {
        MorphCube m_MorphCube;

        TileTrack m_Track;

        [SerializeField]
        Vector3 m_StartPosition;
        [SerializeField]
        Vector3 m_EndPosition;

        LinkedList<GroundRow> m_Grounds;

        public List<GameObject> m_GroundPrefab = new List<GameObject>();

        public Transform m_RotObj;
        //------------------------------------------------------------
        private void Awake()
        {
            m_StartPosition = new Vector3(0, 0, 0);

            m_EndPosition = new Vector3(0, 0, 0);

            m_Grounds = new LinkedList<GroundRow>();
        }

        private void Start()
        {
            this.transform.parent = m_Track.Root;

            this.transform.localPosition = new Vector3(m_Track.Offset.x, m_Track.Offset.y, m_Track.Offset.z);

            this.transform.transform.localEulerAngles = new Vector3(0,0,0);

            //m_MorphCube = this.gameObject.GetComponent<MorphCube>();

            //m_MorphCube.SetLength(m_Track.Length);
            //m_MorphCube.SetWidth(m_Track.Width);
            //m_MorphCube.SetDepth(m_Track.Thickness);

            //m_MorphCube.CreateMesh();

            //m_MorphCube.SetMaterial();

            m_EndPosition = new Vector3(0, m_Track.Length, 0);

            int num = 30;
            //LinkedListNode<GroundRow> row = new LinkedListNode<GroundRow>(new GroundRow(1));
            //m_Grounds.AddFirst(row);
            for (int i=0;i< num; ++i)
            {
                int index = UnityEngine.Random.Range(0, m_GroundPrefab.Count);
                GameObject obj = Instantiate(m_GroundPrefab[index]);
                obj.SetActive(true);
                GroundRow r = new GroundRow(obj);
                r.m_CurTime = (float)(num - i-1) * m_Track.RollTime/ (float)(num-1);
                m_Grounds.AddLast(r);

                Vector3 pos = GetPosition(r.m_CurTime);
                r.SetBornHeight(pos.y);
            }
        }

        private void Update()
        {
            //if(m_Track != null)
            //{
            //    m_Track.Root.TransformPoint
            //}

            if(m_Track.m_bEnableTrack)
            {
                foreach (var row in m_Grounds)
                {
                    Vector3 pos = GetPosition(row.m_CurTime);
                    row.UpdateSpeed(pos);
                    row.m_CurTime += Time.deltaTime;
                }
                GroundRow cacherow = m_Grounds.First.Value;
                while (cacherow.m_CurTime >= m_Track.RollTime)
                {
                    Vector3 pos = GetPosition(cacherow.m_CurTime);
                    //cacherow.UpdateSpeed(new Vector3(pos.x, pos.y, pos.z-10.0f));
                    cacherow.SetBornHeight(pos.y - 1.0f);

                    cacherow.m_CurTime = cacherow.m_CurTime - m_Track.RollTime;
                    m_Grounds.RemoveFirst();
                    m_Grounds.AddLast(cacherow);

                    cacherow = m_Grounds.First.Value;
                }
            }
            
        }
        //------------------------------------------------------------
        public Vector3 GetPosition(float t)
        {
            float l = (m_EndPosition - m_StartPosition).magnitude;
            Vector3 dir = (m_EndPosition - m_StartPosition).normalized;
            Vector3 localpos = m_StartPosition + (t / m_Track.RollTime) * l * dir;
            return this.transform.TransformPoint(localpos);
        }

        public Quaternion GetRotate(float t)
        {
            return m_RotObj.rotation ;
        }

        public void SetTrack(TileTrack track)
        {
            m_Track = track;
        }

        
    }
}
