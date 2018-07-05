/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/24 17:05:23
 *	版 本：v 1.0
 *	描 述：游戏State 管理控制
* ========================================================*/

using System;
using UnityEngine;

namespace Demo.FrameWork 
{
    public class GameStateCtrlMgr : XSingleton<GameStateCtrlMgr>
    {
        
        private GameStateMachine gameStateMachine = new GameStateMachine();

        public IState GetCurrentState()
        {
            return this.gameStateMachine.TopState();
        }

        public void GotoState(string name)
        {
            //Debug.LogWarning(string.Format("GameStateCtrl Goto State {0}" ,name));
            this.gameStateMachine.ChangeState(name);
        }

        public void Initialize()
        {
            // 注册当前游戏状态，注册所有的GameState属性类
            this.gameStateMachine.RegisterStateByAttributes<GameStateAttribute>(typeof(GameStateAttribute).Assembly);            
        }

        public void UnInitialize()
        {
            this.gameStateMachine.Clear();
            this.gameStateMachine = null;
        }

        /// <summary>
        /// 资源管理器轮询。
        /// </summary>
        /// <param name="elapseSeconds">逻辑流逝时间，以秒为单位。</param>
        /// <param name="realElapseSeconds">真实流逝时间，以秒为单位。</param>
        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            IState state = this.GetCurrentState();
            if(state != null)
            {
                state.OnUpdate(elapseSeconds, realElapseSeconds);
            }
        }

        public string currentStateName
        {
            get {
                IState state = this.GetCurrentState();
                if(state == null)
                {
                    return "None State";
                }
                else
                {
                    return state.name;
                }
            }
        }

        // 是否是战斗状态,现在还没有用
        public bool isBattleState
        {
            get {
                return false;
            }
        }

        public bool isLoadingState
        {
            get { return this.gameStateMachine.TopState() is LoadingState; }
        }

        public bool isLoginState
        {
            get
            {
                return (this.gameStateMachine.TopState() is LoginState);
            }
        }
    }
}


