/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/25 10:41:56
 *	版 本：v 1.0
 *	描 述：Loading，负责显示Loading界面，释放上一场景中的资源
 *	        新场景的资源预加载是否放在此暂时待定
* ========================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FrameWork
{
    [GameState]
    public class LoadingState : BaseState
    {
        
        public override void OnStateEnter()
        {
            // 释放上一场景资源, 显示加载界面

            //Singleton<CResourceManager>.GetInstance().UnloadUnusedAssets();
            //Singleton<CUILoadingSystem>.instance.ShowLoading();
            //Singleton<CSoundManager>.GetInstance().PostEvent("Login_Stop", null);
            //Singleton<CSoundManager>.GetInstance().PostEvent("Play_Hall_Ending", null);
            //SLevelContext curLvelContext = Singleton<BattleLogic>.instance.GetCurLvelContext();
            //string str = (curLvelContext == null) ? string.Empty : curLvelContext.m_musicBankResName;
            //if ((str != this.LastLevelBank) && !string.IsNullOrEmpty(this.LastLevelBank))
            //{
            //    Singleton<CSoundManager>.instance.UnLoadBank(this.LastLevelBank, CSoundManager.BankType.LevelMusic);
            //}
            //if (!string.IsNullOrEmpty(str))
            //{
            //    this.LastLevelBank = str;
            //    Singleton<CSoundManager>.instance.LoadBank(str, CSoundManager.BankType.LevelMusic);
            //}
            //CUICommonSystem.OpenFps();

        }

        public override void OnStateLeave()
        {
            // 资源的释放
            //Singleton<CResourceManager>.GetInstance().UnloadUnusedAssets();
            //Singleton<CResourceManager>.GetInstance().UnloadAssetBundlesByTag("CharLoading");
        }
    }
}

