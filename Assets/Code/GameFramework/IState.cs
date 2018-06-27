/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/27 15:04:43
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FrameWork
{
    public interface IState
    {
        // 进入当前状态
        void OnStateEnter();
        // 离开当前状态
        void OnStateLeave();
        // 自身状态被覆盖，自己暂时入栈挂起
        void OnStateOverride();
        // 自身状态重新被唤醒
        void OnStateResume();

        void OnUpdate();
        string name { get; }
    }
}

