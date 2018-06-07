/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/17 10:54:02
 *	版 本：v 1.0
 *	描 述：轨道线接口类
* ========================================================*/

#define DEBUGTEST

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Demo.TileTrack
{
    public interface ITrackLine
    {       
        void PushValue(IPTTile node);

        void Clear();

        void OnUpdate();         
    }


    public class TileLineBase:ITrackLine
    {
        public int lineIndex = 0;

        protected List<IPTTile> _tileList = new List<IPTTile>();

        public virtual void PushValue(IPTTile node)
        {
            _tileList.Add(node);
        }

        public virtual void OnUpdate()
        { }

        public virtual void Clear()
        {
            Debug.LogWarning("Please Complete Clear yourself");
        }

    }
}

