/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/07/03 15:05:00
 *	版 本：v 1.0
 *	描 述：界面UI系统基类
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.FrameWork
{
    public class UIBase 
    {
        // UI界面信息，留作以后功能开发使用
        internal class UIInfo
        {
            public string assetName;    // 界面名称
            public int priority = 0;    // 加载优先级
            // ...
        }

        // ui系统中所有的ui界面信息,如果想成功打开一个界面，必须先注册界面信息
        private Dictionary<string, UIInfo> m_uiInfo;

        // 存放打开的UI界面
        private Dictionary<string, UIForm> m_uiFormSet;

        private string mainAssetName;    // 默认ui名称
        private object[] userDataTemp;  

        public void Init()
        {
            m_uiInfo = new Dictionary<string, UIInfo>();
            m_uiFormSet = new Dictionary<string, UIForm>();

            Create();
        }

        protected virtual void Create()
        {

        }

        // 在Create操作时注册UI信息,可以多次调用注册
        protected void AddUI(string assetName)
        {            
            if(m_uiFormSet.ContainsKey(assetName))
            {
                return;
            }

            if(mainAssetName == null)
            {
                mainAssetName = assetName;
            }

            UIInfo info = new UIInfo();
            info.assetName = assetName;
            m_uiInfo.Add(assetName, info);
        }

        /// <summary>
        /// 界面打开显示，采用的是异步加载的形式，所以此方法没有返回值,
        /// 派生类直接调用即可，没有特殊需求不需要重写
        /// </summary>
        /// <param name="assetName">界面的路径名称</param>
        /// <param name="userData">用户数据</param>
        public void Show(string assetName, params object[] userData)
        {
            userDataTemp = null;

            if(string.IsNullOrEmpty(assetName))
            {
                Debug.LogError("Open Form Failed ,the name is a null value");
                return;
            }
            if(!m_uiInfo.ContainsKey(assetName))
            {
                Debug.LogError("Not Contain this UI Form "+assetName);
                return;
            }

            UIForm uiForm = null;
            if(m_uiFormSet.TryGetValue(assetName,out uiForm))
            {
                // 当界面已经处于打开状态时，直接调用OnShow
                this.OnShow(assetName, userData);
                return;
            }

            
            //UIManager.LoadUIForm();
        }
        
        /// <summary>
        /// 界面资源加载完成后会自动回调此方法(采用的是异步加载)，界面UI的初始化操作可以在此回调方法中完成
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="asset"></param>
        /// <param name="userData"></param>
        protected virtual void OnLoaded(string assetName, UIForm form, params object[] userData)
        {

        }

        /// <summary>
        /// 界面显示，此方法是界面显示过程中最后调用的部分，当此房调用时表明界面已经加载完成并已经显示出来，具体的界面刷新逻辑可以在此处添加
        /// </summary>
        /// <param name="assetName"></param>
        /// <param name="param"></param>
        protected virtual void OnShow(string assetName, params object[] param)
        {

        }

        /// <summary>
        /// 关闭对饮的界面,子类直接调用即可
        /// </summary>
        /// <param name="assetName">界面名称，如果=null表示关闭所有的界面</param>
        public void Hide(string assetName = null)
        {
            userDataTemp = null;

            if(string.IsNullOrEmpty(assetName))
            {
                HideAll();
                return;
            }


        }

        protected void HideAll()
        {
            userDataTemp = null;
        }

        /// <summary>
        /// 界面关闭结束回调，可以在此类中做数据释放和清空操作,
        /// </summary>
        /// <param name="assetName">关闭的界面名称,null表示所有的界面都关闭了</param>
        protected virtual void OnHide(string assetName = null)
        {

        }

        // 判断界面是否开着
        public bool IsOpen(string assetName = null)
        {
            if(assetName == null)
            {
                assetName = mainAssetName;
            }
            
            return m_uiFormSet.ContainsKey(assetName);
        }
    }
}

