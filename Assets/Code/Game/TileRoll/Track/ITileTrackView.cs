using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public interface ITileTrackView
    {
        /// <summary>
        /// 根据轨道上运行的物体的运行时间 寻找其在轨道上的点 返回的是个轨道上的localposition
        /// </summary>
        Vector3 GetPosition(float t);

        /// <summary>
        /// 根据轨道上运行的物体的运行时间 返回当前点的旋转值
        /// </summary>
        Quaternion GetRotate(float t);

        /// <summary>
        /// 设置对应的轨道数据
        /// </summary>
        void SetTrack(TileTrack track);
    }
}
