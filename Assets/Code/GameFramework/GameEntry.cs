/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/27 14:11:45
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FrameWork
{
    public class GameEntry : MonoSingleton<GameEntry>
    {

        protected override void Init()
        {
            Screen.sleepTimeout = -1;
            
            this.SetTargetFrameRate();
        }

        public virtual void Start()
        {
            Application.runInBackground = true;
            
            // Buggly 工具可以在此设置

            this.InitBaseSys();

                        
            XSingleton<GameStateCtrlMgr>.GetInstance().Initialize();
            XSingleton<GameStateCtrlMgr>.GetInstance().GotoState("LaunchState");
        }

        // 基础模块初始化
        protected void InitBaseSys()
        {
            XSingleton<UIManager>.CreateInstance();
            XSingleton<GameStateCtrlMgr>.CreateInstance();

        }

        protected void InitPeripherySys()
        {
            
        }

        private void Update()
        {
            // 更新当前的游戏状态
            XSingleton<GameStateCtrlMgr>.GetInstance().OnUpdate();

        }

        public void LateUpdate()
        { }

        protected override void OnDestroy()
        {
            base.OnDestroy();

        }

        // 帧率设置
        public void SetTargetFrameRate()
        {

        }
    }
}

