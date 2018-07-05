/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/15 15:47:04
 *	版 本：v 1.0
 *	描 述：昼夜系统，控制日夜的交替
 *	1.修改天空盒颜色实现昼夜天空的变化
 *	2.黑夜显现星空mesh
 *	3.控制日月模型的旋转，实现东升西落，太阳的颜色变化
 *	4.修改场景中的亮度
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunshine.Weather
{
    public class DaynightTransform :IPhenomena
    {
        public static int _TintColorValue = Shader.PropertyToID("_TintColor");

        private WeatherSystem _weatherSys = null;

        private Transform SunMoon = null;

        private float starSpeed = 0.4f;
        private float fadeStars = 1.0f;

        private Vector2 staroffset = Vector2.zero;
        private Renderer starPlaneRenderer = null;

        public DaynightTransform(WeatherSystem sys ,GameObject objSunMoon)
        {
            _weatherSys = sys;

            if(objSunMoon != null)
            {
                SunMoon = objSunMoon.transform;
            }
           
            if (_weatherSys.StarPlane != null)
            {
                starPlaneRenderer = _weatherSys.StarPlane.GetComponent<Renderer>();
            }
            fadeStars = 1.0f;
        }  
        
        public void OnUpdate(float dt)
        {
            UpdateSunMoonRotation(dt);

            UpdateStarClouds(dt);
        }

        private void UpdateSunMoonRotation(float dt)
        {
            if(SunMoon != null)
            {
                float dayProgress = _weatherSys.dayProgress;
                float sunAngle = _weatherSys.sunAngle;
                SunMoon.rotation = Quaternion.AngleAxis(dayProgress * 360 - 90, Vector3.back + Vector3.left * sunAngle * .1f);
            }
        }

        private void UpdateStarClouds(float dt)
        {
            if (_weatherSys.StarPlane == null)
                return;

            float brightRatio = (_weatherSys.brightfadeValue - _weatherSys.minbrightpercent) / (_weatherSys.maxbrightpercent - _weatherSys.minbrightpercent);
            if (brightRatio > 0.1f)
            {
                starPlaneRenderer.enabled = false;
            }
            else
            {
                starPlaneRenderer.enabled = true;
                starPlaneRenderer.sharedMaterial.SetColor(_TintColorValue, Color.white * (1 - brightRatio));
            }

            CalculateCloudSpeed(dt);

            staroffset.x += (dt * starSpeed);
            staroffset.x %= 100;
            _weatherSys.StarPlane.GetComponent<Renderer>().sharedMaterial.mainTextureOffset = staroffset;

        }

        void CalculateCloudSpeed(float dt)
        {
            if (_weatherSys.dayLength >= 0 && _weatherSys.dayLength <= 9)
            {
                starSpeed = 0.7f;
            }
            else if (_weatherSys.dayLength >= 10 && _weatherSys.dayLength <= 19)
            {
                starSpeed = 0.4f;
            }
            else if (_weatherSys.dayLength >= 20 && _weatherSys.dayLength <= 29)
            {
                starSpeed = 0.09f;
            }
            else if (_weatherSys.dayLength >= 30 && _weatherSys.dayLength <= 39)
            {
                starSpeed = 0.04f;
            }
            else
            {
                starSpeed = 0.001f;
            }
        }

        public void OnApplicationQuit()
        {
        }
    }
}

