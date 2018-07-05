/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/28 14:59:21
 *	版 本：v 1.0
 *	描 述：UI拓展基类
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Demo.FrameWork
{
    public class UIComponent : MonoBehaviour
    {
        [HideInInspector]
        public UIForm m_belongedFormScript;

        [HideInInspector]
        public int m_indexInlist;
        protected bool m_isInitialized;
        public GameObject[] m_widgets = new GameObject[0];
        
        public virtual void Appear()
        { }

        public virtual void Close()
        { }

        public virtual void Hide()
        {

        }

        protected void DispatchUIEvent()
        { }

        protected T GetComponentInChildren<T>(GameObject go) where T : Component
        {
            T component = go.GetComponent<T>();
            if(component != null)
            {
                return component;
            }

            for(int i = 0;i < go.transform.childCount; ++ i)
            {
                component = this.GetComponentInChildren<T>(go.transform.GetChild(i));
                if(component != null)
                {
                    return component;
                }
            }
            return null;
        }
        public GameObject GetWidget(int index)
        {
            if(index >=0 && index < this.m_widgets.Length)
            {
                return this.m_widgets[index];
            }
            return null;
        }

        public virtual void Initialize(UIForm form)
        {
            if(!this.m_isInitialized)
            {
                this.m_belongedFormScript = form;
                if(form != null)
                {
                    form.AddUIComponent(this);
                    SetSortingOrder(form.GetSortingOrder());
                }
                this.m_isInitialized = true;
            }
        }

        protected void InitializeComponent(GameObject root)
        {
            UIComponent[] components = root.GetComponents<UIComponent>();
            if ((components != null) && (components.Length > 0))
            {
                for (int j = 0; j < components.Length; j++)
                {
                    components[j].Initialize(this.m_belongedFormScript);
                }
            }
            for (int i = 0; i < root.transform.childCount; i++)
            {
                this.InitializeComponent(root.transform.GetChild(i).gameObject);
            }
        }
        protected GameObject Instantiate(GameObject srcGameObject)
        {
            GameObject obj2 = Object.Instantiate(srcGameObject) as GameObject;
            obj2.transform.SetParent(srcGameObject.transform.parent);
            RectTransform transform = srcGameObject.transform as RectTransform;
            RectTransform transform2 = obj2.transform as RectTransform;
            if ((transform != null) && (transform2 != null))
            {
                transform2.pivot = transform.pivot;
                transform2.anchorMin = transform.anchorMin;
                transform2.anchorMax = transform.anchorMax;
                transform2.offsetMin = transform.offsetMin;
                transform2.offsetMax = transform.offsetMax;
                transform2.localPosition = transform.localPosition;
                transform2.localRotation = transform.localRotation;
                transform2.localScale = transform.localScale;
            }
            return obj2;
        }

        public virtual void SetSortingOrder(int sortingOrder)
        {
        }
    }
}

