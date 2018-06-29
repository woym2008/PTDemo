/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/28 11:20:14
 *	版 本：v 1.0
 *	描 述：UI 窗口管理类
* ========================================================*/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.FrameWork
{
    [ExecuteInEditMode]
    public class UIFormScript : MonoBehaviour ,IComparable
    {
        public bool m_isAlwaysKeepVisible = false;
        public int m_sortingOrder = 0;      // 显示排序数级
        public bool m_disableInput = false; // 禁止
        public Vector2 m_referenceResolution = new Vector2(640f,960f);
        public bool m_fullScreen = false;
        public enFormPriority m_priority;
        public GameObject[] m_formWidgets;

        private Canvas m_Canvas;
        private GraphicRaycaster m_graphicRaycaster;
        private CanvasScaler m_CanvasScaler;
        private int m_hideFlags;
        private bool m_isHided;
        private List<UIComponent> m_uiComponents;
        private bool m_isNeedClose;         // 是否需要隐藏
        private bool m_isClosed;            // 界面关闭
        private int m_sequence;
        private int m_openOrder;
        private enFormPriority m_defaultPriority;
        
        private bool m_isInitialized;
        private string m_formAssetName;


        private void Awake()
        {
            InitializeCanvas();
        }
        

        public int CompareTo(object obj)
        {
            UIFormScript script = obj as UIFormScript;
            if(this.m_sortingOrder > script.m_sortingOrder)
            {
                return 1;
            }
            if(this.m_sortingOrder == script.m_sortingOrder)
            {
                return 0;
            }
            return -1;           
        }

        public void CustomUpdate()
        {
            
        }

        public void CustomLateUpdate()
        {

            //UpdateFadeIn();
            //UpdateFadeOut();
        }

        /// <summary>
        /// 界面显示操作
        /// </summary>
        /// <param name="hideFlag"></param>
        /// <param name="dispatchVisibleChangedEvent"></param>
        public void Appear(enFormHideFlag hideFlag = enFormHideFlag.HideByCustom, bool dispatchVisibleChangedEvent = true)
        {
            if(!this.m_isAlwaysKeepVisible)
            {
                this.m_hideFlags &= (~(int)hideFlag);
                if ((this.m_hideFlags == 0) && this.m_isHided)
                {
                    this.m_isHided = false;
                    if(this.m_Canvas != null)
                    {
                        this.m_Canvas.enabled = true;
                        this.m_Canvas.sortingOrder = this.m_sortingOrder;
                    }
                    if((this.m_graphicRaycaster != null) && !this.m_disableInput)
                    {
                        this.m_graphicRaycaster.enabled = true;
                    }

                    AppearComponent();
                    //DispatchRevertVisibleFormEvent();

                    //if(this.dispatchVisibleChangedEvent != null)
                    //{
                    //    this.DispatchVisibleChangedEvent();
                    //}
                }
            }
        }

        /// <summary>
        /// 打开界面，界面请求打开后由UIManager来首先调用此方法
        /// </summary>
        /// <param name="sequence"> 界面的序列号</param>
        /// <param name="exist">界面是否之前存在缓存中</param>
        /// <param name="openOrder">显示优先级</param>
        public void Open(int sequence,bool exist, int openOrder)
        {
            this.m_isNeedClose = false;
            this.m_isClosed = false;
            this.m_sequence = sequence;

            SetDisplayOrder(openOrder);

            if(!exist)
            {
                this.Initialize();
                // ToDO发送事件消息

                //if(IsNeedFadeIn())
                //{
                //    StartFadeIn();
                //}
            }
        }

        public void Open(string formAssetName, Camera camera, int sequeence,bool exist,int openOrder)
        {
            this.m_formAssetName = formAssetName;
            if(this.m_Canvas != null)
            {
                this.m_Canvas.worldCamera = camera;
                if(camera == null)
                {
                    if(this.m_Canvas.renderMode != RenderMode.ScreenSpaceOverlay)
                    {
                        this.m_Canvas.renderMode = RenderMode.ScreenSpaceOverlay;
                    }
                }else if(this.m_Canvas.renderMode != RenderMode.ScreenSpaceCamera)
                {
                    m_Canvas.renderMode = RenderMode.ScreenSpaceCamera;
                }
                m_Canvas.pixelPerfect = true;
            }
            RefreshCanvasScaler();
            this.Open(sequeence, exist, openOrder);
        }

        /// <summary>
        /// 界面关闭操作
        /// </summary>
        public void Close()
        {
            if(!this.m_isNeedClose)
            {
                this.m_isNeedClose = true;
                
                // TODO 关闭事件通知

                this.CloseComponent();
            }
        }

        /// <summary>
        /// 界面隐藏操作
        /// </summary>
        public void Hide(enFormHideFlag hideFlag = enFormHideFlag.HideByCustom, bool dispatchVisibleChangedEvent = true)
        {
            if(!this.m_isAlwaysKeepVisible)
            {
                this.m_hideFlags |= (int)hideFlag;
                if (this.m_hideFlags != 0 && !this.m_isHided)
                {
                    this.m_isHided = true;
                    if(this.m_Canvas != null)
                    {
                        this.m_Canvas.enabled = false;
                    }
                    if(this.m_graphicRaycaster != null)
                    {
                        this.m_graphicRaycaster.enabled = false;
                    }

                    HideComponent();

                    // TODO 发送隐藏事件通知
                }
            }
        }


        private void AppearComponent()
        {
            int count = this.m_uiComponents.Count;
            for(int i =0; i<count; ++i )
            {
                this.m_uiComponents[i].Appear();
            }
        }

        private void HideComponent()
        {
            for(int i = 0; i< this.m_uiComponents.Count; ++ i)
            {
                m_uiComponents[i].Hide();
            }
        }
        private void CloseComponent()
        {
            int count = m_uiComponents.Count;
            for(int i =0; i< count; ++ i)
            {
                m_uiComponents[i].Close();
            }
        }

        public void Initialize()
        {
            if(! this.m_isInitialized)
            {
                this.m_defaultPriority = this.m_priority;
                this.InitializeComponent(gameObject);
                this.m_isInitialized = true;
            }
        }

        public void InitializeComponent(GameObject go)
        {
            UIComponent[] components = go.GetComponents<UIComponent>();
            if(components != null && components.Length > 0)
            {
                for(int i = 0; i< components.Length; ++ i)
                {
                    components[i].Initialize(this);
                }
            }

            for(int j= 0; j< go.transform.childCount; ++ j)
            {
                InitializeComponent(go.transform.GetChild(j).gameObject);
            }
        }

        public void InitializeCanvas()
        {
            this.m_Canvas = gameObject.GetComponent<Canvas>();
            this.m_graphicRaycaster = gameObject.GetComponent<GraphicRaycaster>();
            this.m_CanvasScaler = gameObject.GetComponent<CanvasScaler>();
            if(this.m_graphicRaycaster != null && this.m_disableInput)
            {
                m_graphicRaycaster.enabled = false;
            }
            MatchScreen();
        }

        public void MatchScreen()
        {
            if(this.m_CanvasScaler != null)
            {
                this.m_CanvasScaler.referenceResolution = m_referenceResolution;
                if( ((float)Screen.width/ this.m_CanvasScaler.referenceResolution.x) >
                    ((float)Screen.height /this.m_CanvasScaler.referenceResolution.y))
                {
                    if(this.m_fullScreen)
                    {
                        this.m_CanvasScaler.matchWidthOrHeight = 0f;
                    }
                    else
                    {
                        this.m_CanvasScaler.matchWidthOrHeight = 1.0f;
                    }
                }
                else if (this.m_fullScreen)
                {
                    this.m_CanvasScaler.matchWidthOrHeight = 1f;
                }
                else
                {
                    this.m_CanvasScaler.matchWidthOrHeight = 0f;
                }
            }
        }

        private int CalculateSortingOrder(enFormPriority priority, int openOrder)
        {
            if((openOrder * 10)>= 1000)
            {
                openOrder = openOrder % 100;
            }
            
            int order = openOrder * 10;
            if(!this.IsOverlay())
            {
                order += ( (int)enFormPriority.Priority0 + (int)priority * 1000); 
            }
            else
            {
                order += (10000 + (int)priority * 1000);
            }
            return order;
        }

        public void SetDisplayOrder(int openOrder)
        {
            this.m_openOrder = openOrder;
            if(this.m_Canvas != null)
            {
                this.m_sortingOrder = CalculateSortingOrder(this.m_priority, this.m_openOrder);
                this.m_Canvas.sortingOrder = this.m_sortingOrder;

                try
                {
                    if(m_Canvas.enabled)
                    {
                        // 修改完显示层级后没法立马刷新，采用这个方法来重新绘制界面
                        m_Canvas.enabled = false;
                        m_Canvas.enabled = true;
                    }
                }catch(Exception e)
                {}
                SetComponentSortingOrder(this.m_sortingOrder);
            }
        }

        public void SetPriority(enFormPriority priority)
        {
            if(this.m_priority != priority)
            {
                this.m_priority = priority;
                SetDisplayOrder(this.m_openOrder);
            }
        }

        public void ResetPriority()
        {
            this.SetPriority(this.m_defaultPriority);

        }

        public void AddUIComponent(UIComponent component)
        {
            if(component != null && (!m_uiComponents.Contains(component)))
            {
                m_uiComponents.Add(component);
            }
        }
        public void RemoveUIComponent(UIComponent component)
        {
            if(m_uiComponents.Contains(component))
            {
                m_uiComponents.Remove(component);
            }
        }
        public void SetComponentSortingOrder(int sortingOrder)
        {
            for (int i = 0; i < this.m_uiComponents.Count; ++ i )
            {
                this.m_uiComponents[i].SetSortingOrder(sortingOrder);
            }
        }

        private void RefreshCanvasScaler()
        {
            if(m_CanvasScaler != null)
            {
                m_CanvasScaler.enabled = false;
                m_CanvasScaler.enabled = true;
            }
        }

        private bool IsOverlay()
        {
            if(this.m_Canvas == null)
            {
                return false;
            }
            if(this.m_Canvas.renderMode == RenderMode.ScreenSpaceOverlay ||
                this.m_Canvas.worldCamera == null)
            {
                return true;
            }
            return false;
        }

        public bool IsCanvasEnable()
        {
            if(m_Canvas == null)
            { return false; }
            return m_Canvas.enabled;
        }

        public bool IsClosed()
        {
            return this.m_isClosed;
        }
        public bool IsNeedClos()
        {
            return this.m_isNeedClose;
        }

        public bool IsHide()
        {
            return this.m_isHided;
        }

        public Camera GetCamera()
        {
            if(this.m_Canvas != null && this.m_Canvas.renderMode != RenderMode.ScreenSpaceOverlay)
            {
                return this.m_Canvas.worldCamera;
            }
            return null;
        }

        public GraphicRaycaster GetGraphicRaycaster()
        {
            return this.m_graphicRaycaster;
        }
        public Vector2 GetRefrenceResolution()
        {
            if(this.m_CanvasScaler != null)
            {
                return this.m_CanvasScaler.referenceResolution;
            }
            else
            {
                return Vector2.zero;
            }
        }
        public float GetScreenScaleValue()
        {
            float value = 1f;
            RectTransform component = base.GetComponent<RectTransform>();
            
            if ((component != null) && (m_CanvasScaler.matchWidthOrHeight == 0f))
            {
                value = (component.rect.width / component.rect.height) / (m_CanvasScaler.referenceResolution.x / this.m_CanvasScaler.referenceResolution.y);
            }
            return value;
        }

        public int GetSequence()
        {
            return this.m_sequence;
        }

        public int GetSortingOrder()
        {
            return this.m_sortingOrder;
        }
        public GameObject GetWidget(int index)
        {
            if(index >= 0 && index < this.m_formWidgets.Length)
            {
                return this.m_formWidgets[index];
            }
            return null;
        }

        private void OnDestroy()
        {

        }
    }

}
