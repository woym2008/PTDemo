/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/25 10:58:16
 *	版 本：v 1.0
 *	描 述：登录状态，负责登录场景的加载和显示以及登录界面的显示
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FrameWork
{
    [GameState]
    public class LoginState : BaseState
    {
        public override void OnStateEnter()
        {

            //CUIManager.GetInstance().CloseAllForm((string[])null, true, true);

            //ResourceManager.LoadScene("login", new ResourceManager.LoadCompletedDelegate(this.OnLoginSceneCompleted));
            
            //Singleton<CSoundManager>.CreateInstance();
        }

        public override void OnStateLeave()
        {
            base.OnStateLeave();

            //XSingleton<CLoginSystem>.GetInstance().CloseLogin();

            Debug.Log("CloseLogin...");
            
            //ResourceManager.RemoveCachedResources(new enResourceType[5]
            //  {
            //    enResourceType.BattleScene,
            //    enResourceType.UI3DImage,
            //    enResourceType.UIForm,
            //    enResourceType.UIPrefab,
            //    enResourceType.UISprite
            //  });
        }

        private void OnLoginSceneCompleted()
        {
            Debug.Log("Enter Login scene ...");
           
            // 显示登录场景界面
            //CLoginSystem.GetInstance().Draw();
                     
        }
    }
}

