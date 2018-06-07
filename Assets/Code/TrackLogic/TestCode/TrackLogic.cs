/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/05 15:58:32
 *	版 本：v 1.0
 *	描 述：轨道逻辑测试入口逻辑
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
{
    public class TrackLogic : MonoBehaviour
    {
        public GameObject trackPrefab;


        // Use this for initialization
        void Start()
        {
            TrackManager.GetInstance().Init(TrackNumDef.enTrackType.Curve);

            //SetupPath();
        }

        // Update is called once per frame
        //void Update()
        //{

        //}

        void SetupPath()
        {
            GameObject pathRoot = GameObject.Find("RootPath");
            List<CurveNodeData> pathDataList = new List<CurveNodeData>();



            Transform pathTrans = pathRoot.transform;
            for (int i =0; i< pathTrans.childCount; ++ i )
            {
                Transform child = pathTrans.GetChild(i);
                CurveNodeData data = new CurveNodeData();
                data.position = child.position;
                data.rotation = child.rotation;
                pathDataList.Add(data);
            }

            TrackManager.GetInstance().GenerateTrack(trackPrefab, pathDataList.ToArray());
        }
    }
}

