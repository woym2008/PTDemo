/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/02 14:32:47
 *	版 本：v 1.0
 *	描 述：登录界面控制类
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Demo.FrameWork;

namespace Demo.GameSys
{
    public class UILoginSystem : UIBase
    {

        private string loginPanelName = "UI/UILogin/UILogin";

        private UIForm uiForm;

        // 注册系统后会调用此函数
        protected override void Create()
        {
            GameEventSystem.RegisterEvent(EventIDs.UIEventShowLoginPanel, ShowLoginPanel);

            uiForm = null;
        }



        void ShowLoginPanel(int eventID, params object[] param)
        {
            //uiForm = UIManager
        }
    }
}

