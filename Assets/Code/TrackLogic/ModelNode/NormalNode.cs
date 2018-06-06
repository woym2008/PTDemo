/* ========================================================
 *	作 者：ZhangShouYang 
 *	创建时间：2018/05/16 11:29:21
 *	版 本：v 1.0
 *	描 述：普通类型的节点。具有一般特性
* ========================================================*/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Demo.TileTrack
{
    public class NormalNode : NodeObject
    {
        public SpriteRenderer imgRender;
        public GameObject blastParticle;    // 爆炸特效，触碰后消失效果
        public GameObject objLine;
        public GameObject disapearParticle;  // 死亡消失的特效
        public float scalelenRatio = 0.5f;    // 缩放比率,与单位长度的比率

        float deathTime = 0f;              // 死亡后的时间
        float deathTweenRatio = 0.5f;
        protected override void Awake()
        {
            base.Awake();
            if(imgRender == null)
            {
                SpriteRenderer sr = null;
                for(int i = 0; i< _trans.childCount; ++ i)
                {
                    sr = _trans.GetChild(i).GetComponent<SpriteRenderer>();
                    if(sr != null)
                    {
                        imgRender = sr;
                        break;
                    }
                }
            }
        }


        //protected override void OnTriggerEnter(Collider collider)
        //{

        //}

        protected override void DisplayClickEffect()
        {
            if(blastParticle != null)
            {
                GameObject effect = GameObject.Instantiate(blastParticle);
                effect.transform.position = _trans.position;
            }
            
            if(base._data.triggleChanging)
            {
              
                int trackNum = XSingleton<TrackManager>.GetInstance().trackNum;
                int maxTrackNum = XSingleton<TrackManager>.GetInstance().maxTrackNum;
                if (trackNum < maxTrackNum)
                {
                    XSingleton<TrackManager>.GetInstance().ResetTracklineNum(trackNum + 1, true);
                }
            }
        }

        //public override void ResetDataInfo(NodeManager.NodeData dataInfo)
        //{
        //    base.ResetDataInfo(dataInfo);            
        //}

        public override void Appear(int trackIndex)
        {
            base.Appear(trackIndex);
            
            if(objLine != null)
            {
                objLine.SetActive(false);
           
                /*
                if(_data.subIndex <= 0)
                {
                    objLine.SetActive(false);
                }
                else
                {
                    NodeObject lastNote = TrackManager.instance.lastNote;
                    if (lastNote != null && lastNote.IsAlive(TrackManager.instance.runningTime, TrackManager.instance.circleTime))
                    {
                        objLine.SetActive(true);
                        Vector3 lastPos = lastNote.transform.position;
                        lastPos.z += .0f;
                        objLine.transform.LookAt(lastPos);
                        Vector3 sub = lastNote.transform.position - _trans.position;
                        //Debug.LogWarning(sub + " " + lastNote.transform.position + " " + _trans.position);

                        //objLine.transform.localRotation = Quaternion.FromToRotation(_trans.position, lastNote.transform.position);

                        objLine.transform.localPosition = 0.5f * sub;

                        float len = sub.magnitude;
                        Vector3 scale = objLine.transform.localScale;
                        scale.z = len * scalelenRatio;
                        objLine.transform.localScale = scale;
                    }
                    else
                    {
                        objLine.SetActive(false);
                    }
                }
                 * */
            }
        }


        protected override void AddScore()
        {
            //GameManager.GetInstance().touchOne();
        }

        public override void ResetDataInfo(NodeManager.NodeData dataInfo)
        {
            base.ResetDataInfo(dataInfo);
            if (dataInfo.subIndex > 0)
            {
                voiceable = false;
            }
            else
            {
                voiceable = true;
            }
        }

        public override void DisappearEffect()
        {
            //if(disapearParticle!= null)
            //{
            //    StartCoroutine(ShowFireParticles(2f));
            //}

            deathTime = 0f;

            if(objLine != null)
            {
                objLine.SetActive(false);
            }
            if (imgRender != null)
            {
                StartCoroutine(deadTapEffect(4.0f));
            }

        }
        
        protected IEnumerator ShowFireParticles(float time)
        {
            ParticleEmitter emitter = disapearParticle.GetComponent<ParticleEmitter>();
            emitter.ClearParticles();
            emitter.emit = true;

            yield return new WaitForSeconds(time);

            emitter.emit = false;
            emitter.ClearParticles();
        }
        
        IEnumerator deadTapEffect(float fInterval)
        {
            while(true)
            {
                deathTime += (4*Time.deltaTime);
                if (deathTime > fInterval)
                {
                    _trans.localScale = Vector3.one;
                    imgRender.color = Color.white;
                    yield break;
                }
                float sinValue = Mathf.Sin(deathTime);
                _trans.localScale = (1 + sinValue * deathTweenRatio) * Vector3.one;
                imgRender.color = new Color(1, 1, 1, 1 + sinValue * deathTweenRatio);
                yield return null;
            }
            
        }
    }
}

