/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/02 14:19:39
 *	版 本：v 1.0
 *	描 述：登录系统
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Demo.FrameWork;

namespace Demo.GameSys
{
    public class LoginSystem : XSingleton<LoginSystem>
    {

        //public override void Init()
        //{
            
        //}
        // 显示登录界面
        public void ShowLoginPanel()
        {
            GameEventSystem.SendEvnet(EventIDs.UIEventShowLoginPanel);
        }
    }
}

