/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/13 11:47:37
 *	版 本：v 1.0
 *	描 述：用于控制当前场景的各种显示效果管理
 *	控制入口
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunshine.DisplayManager
{
    public class DisplayManager : MonoBehaviour
    {
        protected static DisplayManager _instance;

        public static RenderTextureFormat rtFormat = RenderTextureFormat.Default;
        
        private GameObject m_pMainLight;
        private DisplayEffectSetting m_pCurInfo;
        public string strCurrentEffect;
        private string m_strNextEffect;
        private string m_strDefault = "default";

        private DisplayEffectSetting m_pTransitionStart;
        private DisplayEffectSetting m_pTransitionEnd;
        private float m_fUseTime;
        private float m_fStartTime = float.MaxValue;
        private bool m_bInitStart = false;

        public Dictionary<string, DisplayEffectSetting> m_effects = new Dictionary<string, DisplayEffectSetting>();
        protected AnimationCurve transitionCurve;

        // Use this for initialization
        void Start()
        {

        }

        public static DisplayManager initialization()
        {
            if(_instance == null)
            {
                GameObject go = new GameObject("DisplayManager");
                _instance = go.AddComponent<DisplayManager>();
                _instance.Init();
            }
            return _instance;
        }

        public static DisplayManager Instance
        {
            get { return _instance; }
        }
        public string defaultEffect
        {
            get { return m_strDefault; }
        }
        public void Init()
        {
            if(m_pCurInfo == null)
            {
                //m_pCurInfo = new DisplayEffectSetting();
                m_pCurInfo = ScriptableObject.CreateInstance<DisplayEffectSetting>();
            }
        }

        public bool Transition(string name, DisplayEffectSetting endEffect, float fUseTime, float fDelay, AnimationCurve transitionCurve = null)
        {
            m_strNextEffect = name;

            // 每一次变换期间暂时不允许插入打断，中断操作留待以后实现
            if(m_bInitStart == true)
            {
                this.m_bInitStart = false;
                fUseTime = Time.time - this.m_fStartTime;
            }

            //DisplayEffectSetting tempCur = ObtianCurrentEffect();

            Transition(this.m_pCurInfo, endEffect, fUseTime, fDelay);
            return true;
        }

        public bool Transition(string name, float fUseTime, float fDelay, AnimationCurve transitionCurve = null)
        {
            this.m_strNextEffect = name;
            DisplayEffectSetting pInfo = GetEffectSetting(name);
            if(pInfo == null)
            {
                Debug.LogError("not contain this effect setting info " +name);
                return false;
            }

            // 每一次变换期间暂时不允许插入打断，中断操作留待以后实现
            if(this.m_bInitStart == true)
            {
                fUseTime = Time.time - this.m_fStartTime;
                m_bInitStart = false;
                if(pInfo != null)
                {
                    Transition(name, pInfo, fUseTime, 0, transitionCurve);
                }else
                {
                    Transition(strCurrentEffect, this.m_pCurInfo, this.m_fUseTime, 0, transitionCurve);
                }

                return false;
            }

            Transition(this.m_pCurInfo, pInfo,fUseTime, fDelay,transitionCurve);
            return true;
        }
        public void Transition(DisplayEffectSetting start, DisplayEffectSetting end, float fUseTime, float fDelay, AnimationCurve transitionCurve = null)
        {
            if(fUseTime < 0 || end == null)
            {
                return;
            }

            this.m_fStartTime = Time.time + fDelay;
            this.m_fUseTime = fUseTime;

            if(start == null)
            {
                start = ObtianCurrentEffect();
            }
            this.m_pTransitionStart = start;

            this.m_pTransitionEnd = end;
        }

        public DisplayEffectSetting ObtianCurrentEffect()
        {
            //DisplayEffectSetting pRecord = new DisplayEffectSetting();
            DisplayEffectSetting pRecord = ScriptableObject.CreateInstance<DisplayEffectSetting>();

            pRecord.GenerateEnvInfo();
            return pRecord;
        }


        public void SetDefaultEffect(string name)
        {
            DisplayEffectSetting pInfo = GetEffectSetting(name);
            if(pInfo == null)
            {
                return ;
            }
            m_effects[this.m_strDefault] = pInfo;
        }

        public DisplayEffectSetting GetDefaultEffect()
        {
            DisplayEffectSetting pInfo = null;
            if (m_effects.TryGetValue(m_strDefault, out pInfo))
            {
                return pInfo;
            }
            Debug.LogError("DisplayManager can't find default effectInfo....");
            return null;
        }

        public void SetCurrentEffect(string name, DisplayEffectSetting pInfo)
        {
            if(m_bInitStart == true)
            {
                m_bInitStart = false;
                Transition(name, pInfo, Time.time - this.m_fStartTime, 0);
            }
            else
            {
                m_pCurInfo = pInfo;
                pInfo.RevertEnvInfo();
                this.strCurrentEffect = name;
            }
        }

        public void AddEffect(string name,DisplayEffectSetting pInfo)
        {
            if(!m_effects.ContainsKey(name))
            {
                m_effects.Add(name, pInfo);
            }
        }
        
        public DisplayEffectSetting GetEffectSetting(string name)
        {
            DisplayEffectSetting pInfo = null;
            if(this.m_effects.TryGetValue(name, out pInfo))
            {
                return pInfo;
            }

            return pInfo;
        }

       
        public void ResetMember()
        {
            // TODO   成员数据重置
            // ... ...
        }


        // Update is called once per frame
        void Update()
        {
            UpdateTransition();
        }

        void UpdateTransition()
        { 
            if(Time.time < this.m_fStartTime)
            {
                return;
            }

            float fInterval = Time.time - this.m_fStartTime;
            float progress = fInterval / m_fUseTime;
            if(m_bInitStart == false)
            {
                m_bInitStart = true;

                this.strCurrentEffect = m_strNextEffect;
                InitTransition(this.m_pTransitionStart, this.m_pTransitionEnd);
            }

            LerpRenderSetting(this.m_pTransitionStart, this.m_pTransitionEnd, progress);
            LerpColorCorrection(this.m_pTransitionStart, this.m_pTransitionEnd, progress);

            if (progress >= 1.0f)
            {
                m_bInitStart = false;
                SetCurrentEffect(m_strNextEffect, m_pTransitionEnd);
                m_strNextEffect = "";
                m_fStartTime = float.MaxValue;
                m_pTransitionStart = null;
                m_pTransitionEnd = null;
            }
        }


        private void InitTransition(DisplayEffectSetting start, DisplayEffectSetting end)
        {

        }

        // 插值环境属性
        private void LerpRenderSetting(DisplayEffectSetting start, DisplayEffectSetting end, float progress)
        {
            RSettingInfo startInfo = start.m_RenderSettingInfo;
            RSettingInfo endInfo = end.m_RenderSettingInfo;

            RenderSettings.fog = endInfo.fog;
            if (endInfo.fog == false)
                return;

            RenderSettings.fogColor = Color.Lerp(startInfo.fogColor, endInfo.fogColor, progress);

            RenderSettings.ambientLight = Color.Lerp(startInfo.ambientLight, endInfo.ambientLight, progress);

            if(startInfo.fogMode != endInfo.fogMode)
            {
                RenderSettings.fogMode = endInfo.fogMode;
                RenderSettings.fogDensity = endInfo.fogDensity;
                return;
            }

            RenderSettings.fogDensity = Mathf.Lerp(startInfo.fogDensity, endInfo.fogDensity, progress);
            RenderSettings.fogStartDistance = Mathf.Lerp(startInfo.fogStartDistance, endInfo.fogStartDistance, progress);
            RenderSettings.fogEndDistance = Mathf.Lerp(startInfo.fogEndDistance, endInfo.fogEndDistance, progress);
        }
        
        // 插值颜色校正
        private void LerpColorCorrection(DisplayEffectSetting start, DisplayEffectSetting end, float progress)
        { }
    }
}

