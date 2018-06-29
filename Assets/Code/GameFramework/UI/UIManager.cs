/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/27 20:30:38
 *	版 本：v 1.0
 *	描 述：UI 控制管理
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Demo.FrameWork
{
    internal sealed partial class UIManager : XSingleton<UIManager>
    {

        private readonly List<int> m_UIFormsBeingLoaded;        // 正在被加载的UI界面
        private readonly List<string> m_UIFormAssetNamesBeingLoaded;    // 正在被加载的UI界面名称
        private readonly HashSet<int> m_UIFormsToReleaseOnLoad; // 需要被取消加载的界面

        private List<UIFormScript> m_forms;     // 所有打开的界面链表
        private List<UIFormScript> m_formsRecyclepool;  // 待回收的缓存列表
        private int m_formSequence;             // 界面序列。递增唯一
        private List<int> m_existFormSequences; // 存在的界面的序列列表

        private GameObject m_uiRoot;
        private EventSystem m_uiInputEventSystem;
        private Camera m_uiCamera;


        public override void Init()
        {
            this.m_forms = new List<UIFormScript>();
            this.m_formsRecyclepool = new List<UIFormScript>();
            this.m_formSequence = 0;
            this.m_existFormSequences = new List<int>();

            CreateUIRoot();
            CreateEventSystem();
            CreateUICamera();
            CreateUISecen();
        }

        private void CreateUIRoot()
        {
            this.m_uiRoot = new GameObject("UIRoot");
            this.m_uiRoot.transform.localPosition = Vector3.zero;

            GameObject.DontDestroyOnLoad(this.m_uiRoot);
        }

        private void CreateEventSystem()
        {
            this.m_uiInputEventSystem = Object.FindObjectOfType<EventSystem>();
            if(this.m_uiInputEventSystem == null)
            {
                GameObject obj = new GameObject("EventSystem");
                this.m_uiInputEventSystem = obj.AddComponent<EventSystem>();

#if ( (UNITY_ANDROID || UNITY_IPHONE || UNITY_WP8 || UNITY_BLACKBERRY) && !UNITY_EDITOR)
                obj.AddComponent<TouchInputModule>();  
#else
                obj.AddComponent<StandaloneInputModule>();
#endif
            }
            this.m_uiInputEventSystem.gameObject.transform.parent = this.m_uiRoot.transform;
        }
        
        private void CreateUICamera()
        {
            GameObject obj = new GameObject("UICamera");
            obj.transform.SetParent(this.m_uiRoot.transform, true);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localRotation = Quaternion.identity;
            obj.transform.localScale = Vector3.one;

            Camera camera = obj.AddComponent<Camera>();
            camera.orthographic = false;    // 广角摄像机
            camera.fieldOfView = 40f;
            //camera.orthographic = true;
            //camera.orthographicSize = 50f;
            camera.clearFlags = CameraClearFlags.Depth;
            camera.cullingMask = 1 << 5;    // UI
            camera.depth = 10f;

            this.m_uiCamera = camera;
        }

        private void CreateUISecen()
        { }

        public void Update()
        {
        }

        public void LateUpdate()
        {

        }

        /// <summary>
        /// 打开UI界面
        /// </summary>
        /// <param name="uiFormAssetName">界面资源名称(界面路径)</param>
        /// <param name="pauseCoveredUIForm">是否暂停被覆盖的UI界面</param>
        /// <returns>返回界面的序列ID编号</returns>
        public int OpenUIForm(string uiFormAssetName, bool pauseCoveredUIForm)
        {

            int sequenceId = 0;

            return sequenceId;
            
        }

    }
}

