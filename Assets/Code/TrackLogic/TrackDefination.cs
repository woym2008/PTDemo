/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/05 14:18:38
 *	版 本：v 1.0
 *	描 述：轨道数据定义
* ========================================================*/

using System;

namespace Demo.GameSystem
{
    public class TrackDefination
    {
        public enum enTrackType
        {
            Linear,     // 线性，一条线运动
            Spline,     // 样条曲线运动
        }

        static public float tilespace = 2f;     //音符间隔
        static public float tileLifeTime = 1f;// 生存的时长
        static public float tileLifeProgress = 0.5f;
        static public float preTileSpace = 10; // 生成块提前的位置(有几个空余块)
        static public float tileSpeed = 1f;
    }
}

