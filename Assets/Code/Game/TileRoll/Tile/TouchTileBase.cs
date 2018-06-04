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
        protected TileTrack m_Parent;

        protected MidiTile m_TileData;

        [SerializeField]
        private float m_MoveTime;

        //bool m_bIsTouched = false;
        //bool m_bCanTouch = false;
        TouchState m_TouchState = TouchState.NotTouch;
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
        public virtual void InitTile(MidiTile data, float scale)
        {
            m_TileData = data;
            m_MoveSpeed = 1.0f;

            m_MoveTime = 0;

            //m_bCanTouch = false;

            m_bIsUseing = true;

            //m_bIsTouched = false;
            m_TouchState = TouchState.NotTouch;
        }
        
        public virtual void FrameUpdate(float realpassedtime)
        {
            //Vector3 offset = m_Parent.TransformVector(0, m_MoveSpeed * Time.deltaTime, 0);
            //this.transform.position = new Vector3(
            //    this.transform.position.x + offset.x,
            //    this.transform.position.y + offset.y,
            //    this.transform.position.z + offset.z);
            m_MoveTime += Time.deltaTime;
            Vector3 pos = m_Parent.GetPos(m_MoveTime);

            Quaternion rot = m_Parent.GetRot(m_MoveTime);

            this.transform.position = pos;
            this.transform.rotation = rot;

            if (m_TouchState == TouchState.Touched)
            {
                if(m_TileData.UpdatePlayTile(Time.deltaTime))
                {
                    ReleaseSelf();
                }
            }
            //m_TileData.PlayTile(realpassedtime);
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
        //--------------------------------------------
        public void SetSpeed(float speed)
        {
            m_MoveSpeed = speed;
        }

        public void AttachParent(TileTrack parent)
        {
            m_Parent = parent;
        }
        //--------------------------------------------
        float getStartTime()
        {
            return (float)m_TileData.StartTime;
        }

        float getProcess()
        {

        }

        void setPosition(Vector3 pos)
        {

        }

        void setRotation(Quaternion quat)
        {

        }

        void setScale(Vector3 scale)
        {

        }
        //--------------------------------------------
        public virtual void OnEndTouch()
        {
        }

        public virtual void OnTouchBeat(Vector3 touchpos)
        {
            if(m_TouchState == TouchState.CanTouch)
            {
                Debug.Log("OnTouchBeat");
                m_TouchState = TouchState.Touched;
                m_TileData.PlayTile();
            }            
        }

        public virtual void OnTouching(Vector3 touchpos)
        {
        }
        //----------------------------------------------------
        public void OnTriggerEnter(Collider other)
        {
            if(other.gameObject.tag == "TouchArea")
            {
                //m_bCanTouch = true;
                m_TouchState = TouchState.CanTouch;
            }

            if (other.gameObject.tag == "AutoTouch" && m_TouchState == TouchState.CanTouch)
            {
                m_TileData.PlayTile();
                m_TouchState = TouchState.Touched;
            }
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.gameObject.tag == "TouchArea")
            {
                Debug.Log("exit touch area release");
                if(m_TouchState == TouchState.NotTouch)
                {
                    ReleaseSelf();
                }                
            }
        }
        //----------------------------------------------------
        public void ReleaseSelf()
        {
            m_bIsUseing = false;

            TouchTileFactory.ReleaseTile(this);
        }
    }
}
