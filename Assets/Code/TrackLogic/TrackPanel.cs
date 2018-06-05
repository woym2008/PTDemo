/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/23 11:26:45
 *	版 本：v 1.0
 *	描 述：轨道面板
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.GameSystem
{
    public class TrackPanel : MonoBehaviour
    {
        public GameObject line2Effect;
        public GameObject line3Effect;
        public GameObject line4Effect;
        public Transform transPanel;

        void Awake()
        {
            if(transPanel == null)
            {
                transPanel = transform.Find("TrackPanel");
            }

        }
        public void PlayStartEffect(int trackNum)
        {
            GameObject objEffect = null;
            switch(trackNum)
            {
                case 2:
                    objEffect = line2Effect;
                    break;
                case 3:
                    objEffect = line3Effect;
                    break;
                case 4:
                    objEffect = line4Effect;
                    break;
                default :
                    objEffect = line4Effect;
                    break;
            }
            if(objEffect != null)
            {
                StartCoroutine(CoroutinePlayEffect(objEffect, 0, 1.0f));
            }
        }

        private IEnumerator CoroutinePlayEffect(GameObject objEffect, float fDelay, float time)
        {
            if(fDelay > 0)
            {
                yield return new WaitForSeconds(fDelay);
            }

            objEffect.SetActive(true);

            if(time > 0)
            {
                yield return new WaitForSeconds(time);

                objEffect.SetActive(false);
            }
            yield break;
        }
    }
}

