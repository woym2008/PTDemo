/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/06/20 17:55:11
 *	版 本：v 1.0
 *	描 述：云层控制单元
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sunshine.Weather
{
    public class CloudsPhenomena : IPhenomena
    {
        public enum enCloudslayer
        {
            rain = 1,
            first = 2,
            second = 4,
            third = 8,
        }

        public static int _ShaderColorProperty = Shader.PropertyToID("_Color");

        private int cullmasklayer = 0;

        private WeatherSystem _weatherSys;

        private MeshRenderer rainCloud;
        private MeshRenderer firstlayerCloud;
        private MeshRenderer secondlayerCloud;
        private MeshRenderer thirdlayerCloud;
        Color[] cloudsOriColors = new Color[4];

        float m_cloudsSpeed = 0.4f;
        Vector2 cloudsOffset = Vector2.zero;
        float offsetValue;

        public CloudsPhenomena(WeatherSystem sys)
        {
            this._weatherSys = sys;

            this.rainCloud = sys.rainClouds.GetComponent<MeshRenderer>(); ;
            this.firstlayerCloud = sys.firstlayerClouds.GetComponent<MeshRenderer>();
            this.secondlayerCloud = sys.secondlayerClouds.GetComponent<MeshRenderer>();
            this.thirdlayerCloud = sys.thirdlayerClouds.GetComponent<MeshRenderer>();

            if(this.rainCloud != null)
            {
                cloudsOriColors[0] = this.rainCloud.sharedMaterial.color;
            }
            if(this.firstlayerCloud != null)
            {
                cloudsOriColors[1] = this.firstlayerCloud.sharedMaterial.color;
            }
            if(this.secondlayerCloud != null)
            {
                cloudsOriColors[2] = this.secondlayerCloud.sharedMaterial.color;
            }
            if(this.thirdlayerCloud != null)
            {
                this.cloudsOriColors[3] = this.thirdlayerCloud.sharedMaterial.color;
            }
        }

        public void SetCloudslayerVisible(int maskLayer)
        {
            this.cullmasklayer = maskLayer;

            if(rainCloud != null)
            {
                rainCloud.enabled = (maskLayer & (int)enCloudslayer.rain) > 0;
            }else
            {
                this.cullmasklayer = this.cullmasklayer ^ (int)enCloudslayer.rain;
            }

            if(firstlayerCloud != null)
            {
                firstlayerCloud.enabled = (maskLayer & (int)enCloudslayer.first) > 0;
            }else
            {
                this.cullmasklayer = this.cullmasklayer ^ (int)enCloudslayer.first;
            }

            if(secondlayerCloud != null)
            {
                secondlayerCloud.enabled = (maskLayer & (int)enCloudslayer.second) > 0;
            }else
            {
                this.cullmasklayer = this.cullmasklayer ^ (int)enCloudslayer.second;
            }

            if(thirdlayerCloud != null)
            {
                thirdlayerCloud.enabled = (maskLayer & (int)enCloudslayer.third) > 0;
            }else {
                this.cullmasklayer = this.cullmasklayer ^ (int)enCloudslayer.third;
            }
        }

        public void OnUpdate(float dt)
        {
            if(_weatherSys.cullingCloudslayers != this.cullmasklayer)
            {
                SetCloudslayerVisible(_weatherSys.cullingCloudslayers);
            }

            CalculateCloudsSpeed(dt);
            UpdateCloudsOffset(dt);
            UpdateCloudsBright(dt);
        }
                

        void CalculateCloudsSpeed(float dt)
        {
            if (_weatherSys.dayLength >= 0 && _weatherSys.dayLength <=9)
	        {
		        m_cloudsSpeed = 0.5f;
	        }else if(_weatherSys.dayLength >= 10 && _weatherSys.dayLength <= 19)
            {
                m_cloudsSpeed = 0.3f;
            }else if(_weatherSys.dayLength >= 20 && _weatherSys.dayLength <= 29)
            {
                m_cloudsSpeed = 0.1f;
            }else if(_weatherSys.dayLength >= 30 && _weatherSys.dayLength <= 39)
            {
                m_cloudsSpeed = 0.05f;
            }else if(_weatherSys.dayLength >= 40)
            {
                m_cloudsSpeed = 0.05f;
            }
        }

        private void UpdateCloudsOffset(float dt)
        {
            offsetValue += (dt * m_cloudsSpeed);
            offsetValue %= 100;
            cloudsOffset.y = offsetValue;

            if ((this.cullmasklayer & (int)enCloudslayer.rain) > 0)
            {
                rainCloud.sharedMaterial.mainTextureOffset = cloudsOffset;
            }
            if((this.cullmasklayer &(int)enCloudslayer.first)> 0)
            {
                cloudsOffset.y = offsetValue * 0.3f;
                firstlayerCloud.sharedMaterial.mainTextureOffset = cloudsOffset;
            }
            
            if((this.cullmasklayer &(int)enCloudslayer.second)>0)
            {
                cloudsOffset.y = offsetValue * 0.1f;
                secondlayerCloud.sharedMaterial.mainTextureOffset = cloudsOffset;
            }
            
            if((this.cullmasklayer &(int)enCloudslayer.third) > 0)
            {
                cloudsOffset.y = offsetValue * 0.05f;
                thirdlayerCloud.sharedMaterial.mainTextureOffset = cloudsOffset;
            }
                       
        }

        int circleCount = 0;
        private void UpdateCloudsBright(float dt)
        {
            circleCount++;
            circleCount %= 5;
            if(circleCount > 0)
            {
                return;
            }

            Color color = Color.Lerp(Color.black, Color.white, _weatherSys.brightfadeValue);
            //rainCloud.sharedMaterial.SetColor(_ShaderColorProperty, color);
            firstlayerCloud.sharedMaterial.SetColor(_ShaderColorProperty, color);
            secondlayerCloud.sharedMaterial.SetColor(_ShaderColorProperty, color);
            thirdlayerCloud.sharedMaterial.SetColor(_ShaderColorProperty, color);

        }

        public void OnApplicationQuit()
        {

        }
    }
}


