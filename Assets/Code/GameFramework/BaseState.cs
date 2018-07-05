/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/25 10:43:42
 *	版 本：v 1.0
 *	描 述：状态父类
* ========================================================*/

using System;

namespace Demo.FrameWork
{
    public class BaseState : IState
    {
        protected string _name;
        protected BaseState()
        {
            _name = base.GetType().Name;
        }

        
        public virtual void OnStateEnter()
        { }

        // 离开当前状态
        public virtual void OnStateLeave()
        { }

        // 自身状态被覆盖，自己暂时入栈挂起
        public virtual void OnStateOverride()
        { }

        // 自身状态重新被唤醒
        public virtual void OnStateResume()
        { }

        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        { }

        public virtual string name {
            get
            {
                return _name;
            }
        }

        
    }

}
