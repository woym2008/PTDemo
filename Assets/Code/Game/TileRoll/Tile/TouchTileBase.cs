using PTAudio.Midi.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public abstract class TouchTileBase : MonoBehaviour, IRecyclableObject, IPTTile
    {
        public enum TouchState
        {
            NotTouch,
            CanTouch,
            Touched,
            Touching,
            Delete,
        }
        public void Dispose()
        {
            try
            {
                GameObject.Destroy(this.gameObject);
            }
            catch (Exception e)
            {

            }
        }

        public virtual string GetRecycleType()
        {
            return this.GetType().FullName;
        }
        //--------------------------------------------
        protected float m_MoveSpeed;

        //protected Transform m_Parent;
        //protected TileTrack m_Parent;

        protected MidiTile m_TileData;
        protected float m_CurProcess;
        protected float m_DelayProcess;
        protected float m_EndProcess;
        protected float m_DelayDisappearProcess;
        protected float m_StartpressTime;
        protected float m_LeastMoveTime;
        float disppeartime = 5.0f;

        [SerializeField]
        public float m_MoveTime;

        public float m_DelayTime;

        //bool m_bIsTouched = false;
        //bool m_bCanTouch = false;
        protected TouchState m_TouchState = TouchState.CanTouch;
        //protected float scale;
        [SerializeField]
        bool m_bIsUseing = false;
        public bool IsUseing
        {
            get
            {
                return m_bIsUseing;
            }
        }
        
        //--------------------------------------------
        public virtual void InitTile(MidiTile data, float lenght, 
            float musictime, 
            float delaytime,
            float startpresstime)
        {
            
            m_TileData = data;

            float alltime = delaytime + musictime;
            m_EndProcess = (float)m_TileData.EndTime / alltime;
            m_DelayProcess = delaytime / alltime;

            //m_StartProcess = startprocess;
            //m_EndProcess = endprocess;
            m_DelayDisappearProcess = disppeartime / alltime;

            m_CurProcess = 0;

            m_StartpressTime = startpresstime;

            m_DelayTime = delaytime;

            m_MoveTime = 0;

            m_LeastMoveTime = m_DelayTime
                - m_StartpressTime;

            //m_bCanTouch = false;

            m_bIsUseing = true;

            //m_bIsTouched = false;
            m_TouchState = TouchState.CanTouch;
        }

        public virtual void CreateMesh(Vector3[] points = null, Vector3[] normals = null)
        {

        }

        public void AutoUpdate()
        {
            if (m_TouchState  == TouchState.CanTouch)
            {
                if (m_MoveTime > m_DelayTime)
                {
                    OnTouchBeat(this.transform.position);
                }
            }            
        }
        //--------------------------------------------
        //public void SetPosition(Vector3 pos)
        //{
        //    this.transform.position = pos;
        //}

        //public void SetRotation(Quaternion quat)
        //{
        //    this.transform.rotation = quat;
        //}

        //public void AttachParent(TileTrack parent)
        //{
        //    m_Parent = parent;
        //}
        //--------------------------------------------
        public float getStartTime()
        {
            return (float)m_TileData.StartTime;
        }

        public float getStartProcess()
        {
            return (float)m_TileData.Process;
        }

        public float getEndProcess()
        {
            return m_EndProcess + m_DelayDisappearProcess;
        }

        public float getProcess()
        {
            return m_CurProcess;

        }

        public virtual void Appear(int trackid)
        {

        }

        public void setProcess(float process)
        {
            m_CurProcess = process;
        }

        public float getPositionProgress()
        {
            return m_DelayProcess/(1- m_DelayProcess);
        }

        public virtual void setPosition(Vector3 pos)
        {
            this.transform.position = new Vector3(pos.x, pos.y, pos.z);
        }

        public virtual void setRotation(Quaternion rot)
        {
            this.transform.rotation = rot;
        }

        public void setScale(Vector3 scale)
        {
            if(this.transform.childCount >0)
            {
                this.transform.GetChild(0).name = "trackmgr t id: " + scale.x;
            }
            this.transform.localScale = scale;
        }
        //--------------------------------------------
        public void EnableTouch()
        {
            if (m_TouchState == TouchState.NotTouch)
            {
                m_TouchState = TouchState.CanTouch;
            }
        }

        public virtual void OnTouchBeat(Vector3 touchpos)
        {
            if(m_TouchState == TouchState.CanTouch)
            {
                //Debug.Log("OnTouchBeat");
                m_TouchState = TouchState.Touched;
                m_TileData.PlayTile();

                ShowPress();
            }            
        }

        public virtual void OnTouching(Vector3 touchpos)
        {
        }

        public virtual void OnEndTouch()
        {
        }
        //----------------------------------------------------
        public virtual void ShowPress()
        {

        }
        //----------------------------------------------------
        //public void OnTriggerEnter(Collider other)
        //{
        //    if(other.gameObject.tag == "TouchArea")
        //    {
        //        //m_bCanTouch = true;
        //        m_TouchState = TouchState.CanTouch;
        //    }

        //    if (other.gameObject.tag == "AutoTouch" && m_TouchState == TouchState.CanTouch)
        //    {
        //        m_TileData.PlayTile();
        //        m_TouchState = TouchState.Touched;
        //    }
        //}

        //public void OnTriggerExit(Collider other)
        //{
        //    if (other.gameObject.tag == "TouchArea")
        //    {
        //        Debug.Log("exit touch area release");
        //        if(m_TouchState == TouchState.NotTouch || m_TouchState == TouchState.CanTouch)
        //        {
        //            ReleaseSelf();
        //        }                
        //    }
        //}
        public bool IsUseless()
        {
            if(m_TouchState == TouchState.Delete)
            {
                return true;
            }

            return false;
        }
        //----------------------------------------------------
        public void ReleaseSelf()
        {
            m_bIsUseing = false;

            TouchTileFactory.ReleaseTile(this);
        }
        //----------------------------------------------------
        public void Disappear()
        {
            ReleaseSelf();
        }

        public virtual void onUpdate()
        {
            m_MoveTime += Time.deltaTime;
            //if (m_MoveTime > m_DelayTime * 2)
            //{
            //    //m_TouchState = TouchState.Touched;
            //    m_TouchState = TouchState.Delete;
            //}
            if(TileManager.s_AutoPlay)
            {
                AutoUpdate();
            }
        }

        public virtual bool onAction()
        {
            if(m_MoveTime > m_DelayTime + m_TileData.EndTime - m_TileData.StartTime)
            {
                //Debug.LogError("error m_MoveTime" + m_MoveTime + " m_DelayTime: " + m_DelayTime);
                return false;
            }
            
            if (m_MoveTime < m_LeastMoveTime)
            {
                return false;
            }

            if(m_TouchState == TouchState.CanTouch)
            {
                OnTouchBeat(this.transform.position);
                return true;
            }
            //Debug.LogError("error m_TouchState" );


            return false;
        }
        //----------------------------------------------------
        private void OnMouseDown()
        {
            ;
        }
        private void OnMouseUp()
        {
            
        }
    }
}
