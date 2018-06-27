/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/28 11:10:40
 *	版 本：v 1.0
 *	描 述：启动游戏状态，在这个状态中可以添加开场动画或是版本检测等
* ========================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FrameWork
{
    [GameState]
    public class LaunchState : BaseState
    {

        public override void OnStateEnter()
        {
            this.GoNextState();  
        }

        private void GoNextState()
        {
            GameStateCtrlMgr.GetInstance().GotoState("LoginState");
        }
    
    }
}

