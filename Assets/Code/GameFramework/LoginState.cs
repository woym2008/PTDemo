/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/25 10:58:16
 *	版 本：v 1.0
 *	描 述：登录状态，负责登录场景的加载和显示以及登录界面的显示
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Demo.GameSys;
namespace Demo.FrameWork
{
    [GameState]
    public class LoginState : BaseState
    {
        public override void OnStateEnter()
        {
            //Singleton<ResourceLoader>.GetInstance().LoadScene("EmptyScene", new ResourceLoader.LoadCompletedDelegate(this.OnLoginSceneCompleted));
            ResourceManager.LoadScene("Login", new LoadSceneCallbacks(this.OnLoginSceneCompleted));
        }

        public override void OnStateLeave()
        {
            base.OnStateLeave();

            //XSingleton<CLoginSystem>.GetInstance().CloseLogin();
            
            //ResourceManager.RemoveCachedResources(new enResourceType[5]
            //  {
            //    enResourceType.BattleScene,
            //    enResourceType.UI3DImage,
            //    enResourceType.UIForm,
            //    enResourceType.UIPrefab,
            //    enResourceType.UISprite
            //  });
        }

        private void OnLoginSceneCompleted(string sceneName, float duration, object userData)
        {           
            // 显示登录场景界面
            LoginSystem.GetInstance().ShowLoginPanel();
                     
        }
    }
}

