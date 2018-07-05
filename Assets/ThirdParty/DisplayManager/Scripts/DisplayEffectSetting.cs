/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/13 13:12:21
 *	版 本：v 1.0
 *	描 述：具体的效果表现信息,提供数据编辑接口，接收用户或是系统传来的数据信息
 *	    并将这些数据使用具体的效果脚本表现出来
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunshine.DisplayManager
{
    
    [System.Serializable]
    public class RSettingInfo:System.Object
    {
        public bool fog;
        public Color fogColor;
        public FogMode fogMode;
        public float fogDensity;
        public float fogStartDistance;
        public float fogEndDistance;
        public Color ambientLight;
        
        public void Reset()
        {
            RenderSettings.fog = fog;
            RenderSettings.fogColor = fogColor;
            RenderSettings.fogMode = fogMode;
            RenderSettings.fogDensity = fogDensity;
            RenderSettings.fogStartDistance = fogStartDistance;
            RenderSettings.fogEndDistance = fogEndDistance;
            RenderSettings.ambientLight = ambientLight;
        }
        public void Generate()
        {
            this.fog = RenderSettings.fog;
            this.fog = RenderSettings.fog;
            this.fogColor = RenderSettings.fogColor;
            this.fogMode = RenderSettings.fogMode;
            this.fogDensity = RenderSettings.fogDensity;
            this.fogStartDistance = RenderSettings.fogStartDistance;
            this.fogEndDistance = RenderSettings.fogEndDistance;
            this.ambientLight = RenderSettings.ambientLight;
        }
        public void Generate(ref RSettingInfo info)
        {
            this.fog = info.fog;
            this.fog = info.fog;
            this.fogColor = info.fogColor;
            this.fogMode = info.fogMode;
            this.fogDensity = info.fogDensity;
            this.fogStartDistance = info.fogStartDistance;
            this.fogEndDistance = info.fogEndDistance;
            this.ambientLight = info.ambientLight;
        }
    }

    [CreateAssetMenu(menuName = "DisplayEffect Setting")]
    public class DisplayEffectSetting : ScriptableObject
    {
        [Header("RenderSettings")]
        public RSettingInfo m_RenderSettingInfo = new RSettingInfo();

        public DisplayEffectSetting()
        {

        }

        public void InitMember()
        { }

        
        public void GenerateEnvInfo()
        {
            m_RenderSettingInfo.Generate();
        }
        public void GenerateEnvInfo(ref RSettingInfo info)
        {
            m_RenderSettingInfo.Generate(ref info);
        }
        // 重置场景渲染环境
        public void RevertEnvInfo()
        {
            m_RenderSettingInfo.Reset();           
        }
    }
}

