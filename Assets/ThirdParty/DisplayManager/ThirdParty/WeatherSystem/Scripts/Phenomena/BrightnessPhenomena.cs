/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/20 16:25:31
 *	版 本：v 1.0
 *	描 述：场景中的亮度控制单元
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunshine.Weather
{
    
    public class BrightnessPhenomena:IPhenomena
    {
        public static int _Tint = Shader.PropertyToID("_Tint");

        WeatherSystem _weatherSys;

        Color skyDefualtColor = Color.white;
        Material skyMaterail = null;

        public BrightnessPhenomena(WeatherSystem sys)
        {
            _weatherSys = sys;

            Camera mainCamera = Camera.main;
            if(mainCamera.GetComponent<Skybox>() != null)
            {
                skyMaterail = mainCamera.GetComponent<Skybox>().material;
            }
            else
            {
                skyMaterail = RenderSettings.skybox;
            }
            if(skyMaterail != null)
            {
                skyDefualtColor = skyMaterail.GetColor(_Tint);
            }
        }

        public void OnUpdate(float dt)
        {
            UpdateBrightFadedvalue(dt);

            UpdateSunBrightnes(dt);
        }

        // 更新一天中的亮度变化,注意0点表示中午12点时刻，因为美术默认制作是按照白天最亮的时刻制作的
        private void UpdateBrightFadedvalue(float dt)
        {
            float brightTimeLength = _weatherSys.brightLengthRatio * 24;
            float fullbrightTimeLength = brightTimeLength * 0.5f; //白昼1/2时间是亮度变化的部分，1/2时间是亮度不变的部分

            float timeofDay = _weatherSys.timeofDay;
            if(_weatherSys.timeofDay > 12)
            {
                timeofDay = 24 - _weatherSys.timeofDay;
            }
                      

            if (timeofDay <= (fullbrightTimeLength *0.5f))
            {
                _weatherSys.brightfadeValue = _weatherSys.maxbrightpercent;

                if (skyMaterail != null)
                {
                    skyMaterail.SetColor(_Tint, skyDefualtColor);
                }
            }
            else if ( timeofDay <= (brightTimeLength * 0.5f))
            {
                _weatherSys.brightfadeValue = Mathf.Lerp(_weatherSys.minbrightpercent,_weatherSys.maxbrightpercent, (brightTimeLength * 0.5f - timeofDay));

                if (skyMaterail != null)
                {
                    skyMaterail.SetColor(_Tint, Color.Lerp(_weatherSys.skyDuskColor, skyDefualtColor, (brightTimeLength * 0.5f - timeofDay)));
                }
            }
            else
            {
                _weatherSys.brightfadeValue = _weatherSys.minbrightpercent;
                if (skyMaterail != null)
                {
                    skyMaterail.SetColor(_Tint, _weatherSys.skyNightColor);
                }
                
            }

        }

        private void UpdateSunBrightnes(float dt)
        {
            float brightRatio = (_weatherSys.brightfadeValue - _weatherSys.minbrightpercent) / (_weatherSys.maxbrightpercent - _weatherSys.minbrightpercent);
           
            if(brightRatio >= 0.98)
            {
                RenderSettings.ambientLight = _weatherSys.MiddayAmbientLight;
                Camera.main.backgroundColor = _weatherSys.backgroundMiddayColor;
                _weatherSys.SunLight.color = _weatherSys.sunDayColor;
            }
            else if(brightRatio <= 0.01f)
            {
                RenderSettings.ambientLight = _weatherSys.NightAmbientLight;
                Camera.main.backgroundColor = _weatherSys.backgroundNightColor;
                _weatherSys.SunLight.color = _weatherSys.sunNightColor;
            }
            else
            {
                RenderSettings.ambientLight = Color.Lerp(_weatherSys.DuskAmbientLight, _weatherSys.MiddayAmbientLight, brightRatio);

                Camera.main.backgroundColor = Color.Lerp(_weatherSys.backgroundDuskColor, _weatherSys.backgroundMiddayColor, brightRatio);

                _weatherSys.SunLight.color = Color.Lerp(_weatherSys.sunDuskColor, _weatherSys.sunDayColor, brightRatio);
            }

        }

        public void OnApplicationQuit()
        {
            if(skyMaterail != null)
            {
                skyMaterail.SetColor(_Tint, skyDefualtColor);
            }
        }
    }
}

