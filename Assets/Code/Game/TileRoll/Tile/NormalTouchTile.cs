using PTAudio.Midi.Builder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Demo
{
    public class NormalTouchTile : TouchTile<TouchTileEvent>
    {
        //--------------------------------------
        //public static string m_TileName = "NormalTile";
        public static string m_TileName = "Effect_01_Tile";
        //--------------------------------------
        public bool m_bCanDestory;
        Material m_SelfMaterial;
        //--------------------------------------
        public MorphCube RenderModel;
        //--------------------------------------
        public float m_Height = 0.2f;
        float m_PressedHeight = 0;
        float m_speed = 2f;

        Quaternion m_CurQuat;

        bool m_EnablePressed;
        //--------------------------------------
        public GameObject m_partical;
        //--------------------------------------

        public override void InitTile(
            MidiTile data, float scale, 
            float musictime, float delaytime,
            float startpresstime
            )
        {
            base.InitTile(data, scale, musictime, delaytime, startpresstime);

            RenderModel = this.GetComponent<MorphCube>();

            m_SelfMaterial = this.GetComponent<MeshRenderer>().material;

            RenderModel.SetLength(m_Height);
            RenderModel.SetWidth(0.2f);
            RenderModel.SetDepth(scale * 1f);

            if(scale > 2)
            {
                RenderModel.segment_z = (int)scale + 1;
            }
            // 临时注释掉
            //RenderModel.collidescale = 1.5f;

            m_EnablePressed = false;

            m_PressedHeight = 0;
        }

        public override void CreateMesh(Vector3[] points, Vector3[] normals)
        {
            RenderModel.CreateMesh();

            m_SelfMaterial = RenderModel.GetMeshMaterial();
        }

        public override string GetRecycleType()
        {
            return m_TileName;
        }

        public override void ShowPress()
        {
            m_EnablePressed = true;

            StartCoroutine(DisplayTouchedEffect());
            
        }

        private IEnumerator DisplayTouchedEffect()
        {
            // 临时显示效果，所以直接创建了，后面改为缓存形式

            if (m_partical == null)
                yield break;

            m_partical.SetActive(true);
            yield return new WaitForSeconds(0.8f);
            m_partical.SetActive(false);
        }

        public override void onUpdate()
        {
            base.onUpdate();

            if(m_EnablePressed)
            {
                if(m_TouchState != TouchState.Delete && m_PressedHeight > 2*m_Height)
                {
                    m_TouchState = TouchState.Touched;
                    return;
                }
                m_PressedHeight += Time.deltaTime * m_speed;
            }
            
        }

        public override void setPosition(Vector3 pos)
        {
            Vector3 offset = m_CurQuat * (new Vector3(0, -m_PressedHeight, 0));
            this.transform.position =
                new Vector3(pos.x, pos.y, pos.z) + offset;

            //this.transform.position =
            //    new Vector3(pos.x, pos.y, pos.z);
        }

        public override void setRotation(Quaternion rot)
        {
            base.setRotation(rot);

            m_CurQuat = rot;
        }

        public override void Appear(int trackid)
        {
            GameObject obj = new GameObject();
            obj.name = "t:" + trackid;
            obj.transform.parent = this.transform;
        }

        public override void OnPress<TouchTileEvent>(TouchTileEvent e)
        {
            GameObject touchobj = e.m_TouchObj;
            if(touchobj == this.gameObject)
            {
                OnTouchBeat(e.m_TouchPos);
            }
        }

        public override void OnEnd<TouchTileEvent>(TouchTileEvent e)
        {
        }

        public override void OnPressing<TouchTileEvent>(TouchTileEvent e)
        {
        }
    }
}
