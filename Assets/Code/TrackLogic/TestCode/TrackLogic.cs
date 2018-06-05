/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/05 15:58:32
 *	版 本：v 1.0
 *	描 述：轨道逻辑测试入口逻辑
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.GameSystem
{
    public class TrackLogic : MonoBehaviour
    {
        
        public ITrackViewer trackViewer;



        // Use this for initialization
        void Start()
        {
            trackViewer = new CurveTrackRollViewer();


        }

        // Update is called once per frame
        void Update()
        {

        }


    }
}

