/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/15 13:54:01
 *	版 本：v 1.0
 *	描 述：天气系统，需要实现的效果 昼夜交替、 晴天、多云、阴天、雨天、雪天、风、沙尘等
 *	1. 昼夜交替(未实现)，日月东升西落，光照明暗交替
 *	2.注意：默认美术设计是晴天且为白天时刻（即中午时刻），所有系统中的0点表示的中午12点，计时是从中午12点开始
 *	
 *  设计结构：WeatherSystem天气系统的控制和执行逻辑，
 *  BaseWeather 天气基类，包含 Sunny(晴天) Cloudy(多云)Overcast(阴天)Rain(雨) Snowy(雪)等派生类
 *  Weather控制天气(Weather)的切换(各个天气独立控制自身的过渡)，通过提供不同的数据来控制不同的气象表现
 *  IPhenomena:气象现象单元接口，包含众多独立的具体气象表现，昼夜交替(日升月落，星空),云层，光照强度控制等,所有的天气现象彼此独立
 *      所有的天气现象都是由所有的气象单元相互组合作用产生
* ========================================================*/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunshine.Weather
{
    public class WeatherSystem : MonoBehaviour
    {
        private List<IPhenomena> _phenomenaList = new List<IPhenomena>();

        #region Weather Data

        // Time keeping Variables
        public float minuteCounter = 0;
        public float hourCounter = 0;
        public float dayCount= 0;

        public float monthCount = 0;
        public float yearCount = 0;
        public float dayLength = 10;     // 每日时长(秒S)
        [Range(0,1.0f)]
        public float brightLengthRatio = 0.5f; // 白天时长所占的比率

        [Range(0,1.0f)]
        public float dayProgress = 0;   // 当日时间的进度
        
        
        // Sun intensity control
        public Light SunLight;
        public float sunIntensity;
        public float maxSunIntensity;
        // Bright Value 控制场景中亮度，其他气象单元通过此值来计算自身的亮度 0没有光 1表示使用最大亮度
        public float brightfadeValue = 1;
        [Range(0,1.0f)]
        public float maxbrightpercent = 1.0f;
        [Range(0,1.0f)]
        public float minbrightpercent = 0f;
        
        // Sun angle control
        public float sunAngle = 0;
        // Sun Color 
        public Color sunDayColor;
        public Color sunDuskColor;
        public Color sunNightColor;
        //Ambient light colors
        public Color NightAmbientLight;     // 夜晚的环境色
        public Color DuskAmbientLight;      // 黄昏的颜色
        public Color MiddayAmbientLight;    // 中午的颜色
        //Camera Background colors
        public Color backgroundNightColor;
        public Color backgroundDuskColor;
        public Color backgroundMiddayColor;
        // Sky box Color
        public Color skyDuskColor;
        public Color skyNightColor;
        // Clouds Gameobject
        public GameObject rainClouds;
        public GameObject firstlayerClouds;
        public GameObject secondlayerClouds;
        public GameObject thirdlayerClouds;
        public int cullingCloudslayers = 1;

        #endregion Weather Data

        // 星云的速度
        //private float m_starSpeed;
        

        // SunMoon Object
        public GameObject SunMoonObject;
        // start plane object
        public GameObject StarPlane;
               


        private float sunRotate = 0;


        private float Hour = 0f;
        private float minuteCountereCalculater = 0;
        private float yearCounterCalculater = 0f;
        //private float m_timeofDay;       //当天的时间显示(h) 0-12之间
        private float calculate2;

        // systems 
        //private Daynight _daylight;     // 昼夜交替现象(由天气系统统一控制)


        // Use this for initialization
        void Start()
        {
            //_daylight = new Daynight(this, SunMoonObject);
            
            _phenomenaList.Add(new BrightnessPhenomena(this));
            _phenomenaList.Add(new DaynightTransform(this, SunMoonObject));
            _phenomenaList.Add(new CloudsPhenomena(this));

            
        }

        public float DayProgress
        {
            get { return dayProgress; }
        }
        //public float StarSpeed
        //{ 
        //    get { return m_starSpeed; } 
        //}

        public float timeofDay
        {
            //get { return m_timeofDay; }
            get { return this.Hour; }
        }

        // Update is called once per frame
        void Update()
        {
            float deltaTime = Time.deltaTime;
            
            UpdateTimeCount(deltaTime);

            UpdatePhenomenas(deltaTime);
        }

        private void UpdateTimeCount(float dt)
        {
            Hour = dayProgress * 24;        // 一天24小时
            //m_timeofDay = calculate2 * 24;
            dayProgress = dayProgress + dt / dayLength;

            // 时间超过半天时间，保持计时可以循环计算
            if (dayProgress < 0.5f)
            {
                calculate2 = dayProgress;
            }
            if (dayProgress > 0.5f)
            {
                calculate2 = 1 - dayProgress;
            }

            hourCounter = Hour;
            minuteCountereCalculater = Hour * 60;
            minuteCounter = minuteCountereCalculater;
            sunRotate = minuteCounter;

            if (minuteCounter > 60)
            {
                minuteCounter = minuteCountereCalculater % 60;
            }

            if (Hour >= 24)
            {
                dayProgress = 0;
                calculate2 = 0;
                Hour = 0;
                dayCount += 1;
            }


            if (dayCount >= 30)
            {
                dayCount = dayCount % 30;
                monthCount += 1;
            }
            if (monthCount > 12)
            {
                monthCount = monthCount % 12;
                yearCount += 1;
            }
        }

        private void UpdatePhenomenas(float dt)
        {
            int len = _phenomenaList.Count;

            for(int i = 0;i < len; ++ i)
            {
                _phenomenaList[i].OnUpdate(dt);
            }
        }


        private void   OnApplicationQuit()
        {
            int len = _phenomenaList.Count;
            for (int i = 0; i < len; ++i)
            {
                _phenomenaList[i].OnApplicationQuit();
            }
        }
    }
}

