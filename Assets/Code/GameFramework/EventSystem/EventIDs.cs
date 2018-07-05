/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/02 14:04:29
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System;

namespace Demo.FrameWork
{
    public class EventIDs 
    {
        internal class EventSpawn
        {
            public const int Login = 500;   // 登录事件 500 - 1000；
 
            public const int UI = 3000;     // UI事件范围 3000 - 5999；
        }

        # region 登录事件ID
        
        #endregion

        #region UI事件ID
        // 登录界面 3050 - 3069
        public const int UIEventShowLoginPanel = EventSpawn.UI + 50;


        #endregion

    }
}

