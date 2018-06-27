/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/24 17:08:13
 *	版 本：v 1.0
 *	描 述：
* ========================================================*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Demo.FrameWork
{
    public class GameStateMachine
    {
        private Dictionary<string, IState> _registedState = new Dictionary<string,IState>();
        private Stack<IState> _stateStack = new Stack<IState>();
        public IState tarState { get; private set; }

        /// <summary>
        /// 修改当前状态，并返回所替换的状态
        /// </summary>
        /// <param name="state"></param>
        /// <returns></returns>
        public IState ChangeState(IState state)
        {
            if(state == null)
            { return null;}

            this.tarState = state;
            IState lastState = null;

            if(this._stateStack.Count > 0)
            {
                lastState = this._stateStack.Pop();
                lastState.OnStateLeave();
            }

            this._stateStack.Push(state);
            state.OnStateEnter();

            return lastState;
        }

        public IState ChangeState(string strName)
        {
            IState state;
            if(string.IsNullOrEmpty(strName))
            { return null; }

            if(!this._registedState.TryGetValue(strName, out state))
            {
                return null;
            }
            return this.ChangeState(state);
        }

        public void Clear()
        {
            while(this._stateStack.Count > 0)
            {
                this._stateStack.Pop().OnStateLeave();
            }
        }

        public IState GetState(string name)
        {
            IState state;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            this._registedState.TryGetValue(name, out state);
            return state;
        }

        public string GetStateName(IState state)
        {
            if(state != null)
            {
                return state.name;
            }
            return null;
        }

        public IState PopState()
        {
            if(this._stateStack.Count<= 0)
            {
                return null;
            }
            IState state = this._stateStack.Pop();
            state.OnStateLeave();

            // 弹出栈顶，同时将新的栈顶状态重新激活
            if(this._stateStack.Count > 0)
            {
                this._stateStack.Peek().OnStateResume();
            }
            return state;
        }

        public void Push(IState state)
        {
            if (state == null)
                return;
            if(this._stateStack.Count > 0)
            {
                this._stateStack.Peek().OnStateOverride();
            }

            this._stateStack.Push(state);
            state.OnStateEnter();
        }

        public void Push(string name)
        {
            if(string.IsNullOrEmpty(name))
                return;
            IState state = null;
            if (this._registedState.TryGetValue(name, out state))
            {
                this.Push(state);
            }
        }

        public void RegisterState(string name, IState state)
        {
            if (((name != null) && (state != null)) && !this._registedState.ContainsKey(name))
            {
                this._registedState.Add(name, state);                
            }
        }

        public void RegisterState<TStateImplType>(TStateImplType State, string name) where TStateImplType : IState
        {
            this.RegisterState(name, State);
        }

       
        public ClassEnumerator RegisterStateByAttributes<T>(Assembly InAssembly) where T : AutoRegisterAttribute
        {
            ClassEnumerator enumerator = new ClassEnumerator(typeof(T), typeof(IState), InAssembly, true, false, false);
            List<Type>.Enumerator enumerator2 = enumerator.results.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                Type current = enumerator2.Current;
                IState state = (IState)Activator.CreateInstance(current);
                this.RegisterState<IState>(state, state.name);
            }
            return enumerator;
        }

        public ClassEnumerator RegisterStateByAttributes<T>(Assembly InAssembly, params object[] args) where T : AutoRegisterAttribute
        {
            ClassEnumerator enumerator = new ClassEnumerator(typeof(T), typeof(IState), InAssembly, true, false, false);
            List<Type>.Enumerator enumerator2 = enumerator.results.GetEnumerator();
            while (enumerator2.MoveNext())
            {
                Type current = enumerator2.Current;
                IState state = (IState)Activator.CreateInstance(current, args);
                this.RegisterState<IState>(state, state.name);
            }
            return enumerator;
        }

        public IState TopState()
        {
            if(this._stateStack.Count <= 0)
            { return null; }

            return this._stateStack.Peek();
        }

        public string TopStateName()
        {
            if (this._stateStack.Count <= 0)
                return null;
            IState state = this._stateStack.Peek();
            return state.name;
        }

        public IState UnregisterState(string name)
        {
            IState state;
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }
            if (!this._registedState.TryGetValue(name, out state))
            {
                return null;
            }
            this._registedState.Remove(name);
            return state;
        }

        public int Count
        {
            get
            {
                return this._stateStack.Count;
            }
        }
    }
}

